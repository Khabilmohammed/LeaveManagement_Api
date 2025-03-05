using LeaveManagement.DataAccess.Data.Repository.AuthRepo;
using LeaveManagement.Models.DTO.LoginAndRegister;
using LeaveManagement.Models.Models;
using LeaveManagement.Utility.Helper;
using LeaveManagement.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.DataAccess.Services.AuthServices
{
    public class AuthService:IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<APIResponse> RegisterAsync(RegisterRequestDTO model)
        {
            var userExists = await _authRepository.FindUserByNameAsync(model.UserName);
            if (userExists != null)
                return ResponseHelper.Error("User already exists!");

            var newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                FullName = model.FullName
            };

            var result = await _authRepository.CreateUserAsync(newUser, model.Password);
            if (!result.Succeeded)
                return ResponseHelper.Error(result.Errors.Select(e => e.Description).ToList());

            // Ensure roles exist
            if (!await _authRepository.RoleExistsAsync(SD.Role_Manager))
            {
                await _authRepository.CreateRoleAsync(SD.Role_Manager);
                await _authRepository.CreateRoleAsync(SD.Role_Employee);
            }

            // Assign role
            var role = model.Role.Equals(SD.Role_Manager, StringComparison.OrdinalIgnoreCase) ? SD.Role_Manager : SD.Role_Employee;
            await _authRepository.AddUserToRoleAsync(newUser, role);

            return ResponseHelper.Success(new { message = "User registered successfully!", userId = newUser.Id });
        }

        public async Task<APIResponse> LoginAsync(LoginRequestDTO model)
        {
            var user = await _authRepository.FindUserByNameAsync(model.UserName);
            if (user == null || !await _authRepository.CheckPasswordAsync(user, model.Password))
                return ResponseHelper.Error("Invalid username or password", HttpStatusCode.Unauthorized);

            var roles = await _authRepository.GetUserRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            var loginResponse = new LoginResponseDTO
            {
                Email = user.Email,
                Token = token
            };

            return ResponseHelper.Success(loginResponse);
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
