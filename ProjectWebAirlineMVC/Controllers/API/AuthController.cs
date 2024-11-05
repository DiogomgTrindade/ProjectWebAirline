using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using ProjectWebAirlineMVC.Data.Interfaces;
using ProjectWebAirlineMVC.Models;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var token = await _authService.AuthenticateAsync(model.EmailAddress, model.Password);
            if (token == null)
            {
                return Unauthorized(new {Message = "Invalid login attempt."});
            }

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterNewUserViewModel model)
        {
            var success = await _authService.RegisterAsync(model.EmailAddress, model.Password, model.FirstName, model.LastName);
            if (!success)
            {
                return BadRequest(new { Message = "Registration failed." });
            }

            return Ok(new {Message = "User registred successfully." });
        }
    }
}
