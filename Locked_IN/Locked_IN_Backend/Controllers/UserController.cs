using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentValidation;
using Locked_IN_Backend.DTOs.User;
using Locked_IN_Backend.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers;

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
    /// Register a new user.
    /// </summary>
    /// <param name="dto">User registration data (multipart/form-data for avatar).</param>
    /// <returns>Registered user profile with token.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromForm] RegisterDto dto)
    {
        var validationResult = await _registerValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }

        var result = await _userService.RegisterAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Login a user.
    /// </summary>
    /// <param name="dto">User login credentials.</param>
    /// <returns>Logged in user profile with token.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var validationResult = await _loginValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }

        var result = await _userService.LoginAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Logout the current user.
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync();
        return Ok(new { Message = "Logged out successfully." });
    }

    /// <summary>
    /// Get user profile details by ID (Public).
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>User profile data.</returns>
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserProfile(int userId)
    {
        var result = await _userService.GetUserProfileAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Update basic user profile information.
    /// </summary>
    /// <param name="dto">New profile data.</param>
    /// <returns>Updated profile.</returns>
    [Authorize]
    [HttpPut("profile")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileDto dto)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        var validationResult = await _updateProfileValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }

        var result = await _userService.UpdateUserProfileAsync(userId, dto);
        return Ok(result);
    }

    /// <summary>
    /// Update user availability calendar.
    /// </summary>
    /// <param name="dto">Availability dictionary (Day -> Hours).</param>
    /// <returns>Updated profile.</returns>
    [Authorize]
    [HttpPut("availability")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateAvailability([FromBody] UpdateAvailabilityDto dto)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        var validationResult = await _updateAvailabilityValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }

        var result = await _userService.UpdateAvailabilityAsync(userId, dto);
        return Ok(result);
    }
}