using Locked_IN_Backend.DTOs.User;
using Locked_IN_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get user profile details by ID
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>User profile data including availability</returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var result = await _userService.GetUserProfileAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Update basic user profile information
        /// </summary>
        /// <param name="userId">The ID of the user to update (should match authenticated user)</param>
        /// <param name="dto">New profile data</param>
        /// <returns>Updated profile</returns>
        [HttpPut("profile/{userId}")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UpdateUserProfileDto dto)
        {
            var result = await _userService.UpdateUserProfileAsync(userId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Update user availability calendar
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="dto">Availability dictionary (Day -> Hours)</param>
        /// <returns>Updated profile</returns>
        [HttpPut("availability/{userId}")]
        public async Task<IActionResult> UpdateAvailability(int userId, [FromBody] UpdateAvailabilityDto dto)
        {
            var result = await _userService.UpdateAvailabilityAsync(userId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}