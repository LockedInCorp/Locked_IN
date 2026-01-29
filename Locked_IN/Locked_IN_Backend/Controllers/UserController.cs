using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentValidation;
using Locked_IN_Backend.DTOs.User;
using Locked_IN_Backend.Interfaces.Services;
using Locked_IN_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<UpdateUserProfileDto> _updateProfileValidator;
        private readonly IValidator<UpdateAvailabilityDto> _updateAvailabilityValidator;

        public UserController(
            IUserService userService,
            IValidator<RegisterDto> registerValidator,
            IValidator<LoginDto> loginValidator,
            IValidator<UpdateUserProfileDto> updateProfileValidator,
            IValidator<UpdateAvailabilityDto> updateAvailabilityValidator)
        {
            _userService = userService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _updateProfileValidator = updateProfileValidator;
            _updateAvailabilityValidator = updateAvailabilityValidator;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="dto">User registration data</param>
        /// <returns>Registered user profile</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
            var validationResult = await _registerValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors.First().ErrorMessage });
            }

            var result = await _userService.RegisterAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="dto">User login credentials</param>
        /// <returns>Logged in user profile</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var validationResult = await _loginValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors.First().ErrorMessage });
            }

            var result = await _userService.LoginAsync(dto);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns>Logout status message</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _userService.LogoutAsync();
            return Ok(result);
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
        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileDto dto)
        {
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
            var userId = int.Parse(userIdClaim);
            var validationResult = await _updateProfileValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors.First().ErrorMessage });
            }

            var result = await _userService.UpdateUserProfileAsync(userId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Update user availability calendar
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="dto">Availability dictionary (Day -> Hours)</param>
        /// <returns>Updated profile</returns>
        [Authorize]
        [HttpPut("availability")]
        public async Task<IActionResult> UpdateAvailability([FromBody] UpdateAvailabilityDto dto)
        {
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
            var userId = int.Parse(userIdClaim);
            var validationResult = await _updateAvailabilityValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors.First().ErrorMessage });
            }

            var result = await _userService.UpdateAvailabilityAsync(userId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}