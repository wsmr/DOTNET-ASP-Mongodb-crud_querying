using DiyawannaSupBackend.Api.Models.DTOs;
using DiyawannaSupBackend.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DiyawannaSupBackend.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        [Authorize]
        [HttpPost("validate")]
        public IActionResult ValidateToken()
        {
            // If the request reaches here, the token is valid due to [Authorize]
            var username = User.Identity.Name; // Get username from token claims
            // In a real application, you might return more detailed user info or token expiry
            return Ok(new { valid = true, username = username, expiresAt = DateTime.UtcNow.AddMinutes(60) }); // Placeholder for expiresAt
        }
    }
}
