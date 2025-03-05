using LeaveManagement.DataAccess.Data;
using LeaveManagement.Models.DTO.LoginAndRegister;
using LeaveManagement.Models.Models;
using LeaveManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LeaveManagement_Api.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthController(ApplicationDbContext db ,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = _db.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

            if (userExists != null)
                return BadRequest(new { message = "User already exists!" });

            var newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors); // Return errors if registration fails

            // Ensure roles exist in the database
            if (!await _roleManager.RoleExistsAsync(SD.Role_Manager))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Manager));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
            }

            // Assign user to the appropriate role
            if (model.Role.ToLower() == SD.Role_Manager.ToLower())
            {
                await _userManager.AddToRoleAsync(newUser, SD.Role_Manager);
            }
            else
            {
                await _userManager.AddToRoleAsync(newUser, SD.Role_Employee);
            }

            return Ok(new { message = "User registered successfully!", userId = newUser.Id });
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request data" });

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            bool isValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isValid)
                return Unauthorized(new { message = "Invalid username or password" });

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            LoginResponseDTO loginResponse = new()
            {
                Email = user.Email,
                Token = token,
            };

            return Ok(loginResponse);
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
