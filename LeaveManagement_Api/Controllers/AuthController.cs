using LeaveManagement.DataAccess.Services.AuthServices;
using LeaveManagement.Models.DTO.LoginAndRegister;
using Microsoft.AspNetCore.Mvc;


namespace LeaveManagement_Api.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            var response = await _authService.RegisterAsync(model);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var response = await _authService.LoginAsync(model);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
