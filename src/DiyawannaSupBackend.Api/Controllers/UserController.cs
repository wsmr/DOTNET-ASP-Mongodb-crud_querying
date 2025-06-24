using DiyawannaSupBackend.Api.Models.Entities;
using DiyawannaSupBackend.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DiyawannaSupBackend.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize] // All user endpoints require authentication
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] User user)
        {
            await _userService.UpdateUserAsync(id, user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent(); // 204 No Content for successful deletion
        }

        [HttpGet("search/name")]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsersByName([FromQuery] string name)
        {
            var users = await _userService.SearchUsersByNameAsync(name);
            return Ok(users);
        }

        [HttpGet("university/{university}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersByUniversity(string university)
        {
            var users = await _userService.GetUsersByUniversityAsync(university);
            return Ok(users);
        }

        [HttpGet("age-range")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersByAgeRange([FromQuery] int minAge, [FromQuery] int maxAge)
        {
            var users = await _userService.GetUsersByAgeRangeAsync(minAge, maxAge);
            return Ok(users);
        }
    }
}
