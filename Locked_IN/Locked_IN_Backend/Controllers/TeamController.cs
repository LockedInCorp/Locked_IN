using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Locked_IN_Backend.Services;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.DTOs.Team;
using Locked_IN_Backend.Misc;
using FluentValidation;
using Locked_IN_Backend.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IValidator<AdvancedSearchDto> _advancedSearchValidator;
    private readonly IValidator<CreateTeamDto> _createTeamValidator;

    public TeamController(ITeamService teamService, 
        IValidator<AdvancedSearchDto> advancedSearchValidator,
        IValidator<CreateTeamDto> createTeamValidator)
    {
        _teamService = teamService;
        _advancedSearchValidator = advancedSearchValidator;
        _createTeamValidator = createTeamValidator;
    }

    /// <summary>
    /// Get team by ID
    /// </summary>
    /// <param name="id">Team ID</param>
    /// <returns>Team details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetTeamDto>> GetTeamById(int id)
    {
        var team = await _teamService.GetTeamByIdAsync(id);
        
        if (team == null)
            return NotFound($"Team with ID {id} not found");

        return Ok(team);
    }

    /// <summary>
    /// Get teams by game ID
    /// </summary>
    /// <param name="gameId">Game ID</param>
    /// <returns>List of teams for the specified game</returns>
    [HttpGet("game/{gameId}")]
    public async Task<ActionResult<List<GetTeamDto>>> GetTeamsByGameId(int gameId)
    {
        var teams = await _teamService.GetTeamsByGameIdAsync(gameId);
        return Ok(teams);
    }
    
    /// <summary>
    /// Search teams by name
    /// </summary>
    /// <param name="searchTerm">Search term to find teams by name</param>
    /// <returns>List of teams matching the search term, ordered by relevance. Returns all games if searchTerm is null or empty</returns>
    [HttpGet("search")]
    public async Task<ActionResult<List<GetTeamDto>>> SearchTeamsByName([FromQuery] string searchTerm = "")
    {
        var teams = await _teamService.GetTeamsByNameSearchAsync(searchTerm);
        return Ok(teams);
    }

    /// <summary>
    /// Advanced search for teams by optional filters: games, preference tags, and search term.
    /// </summary>
    /// <param name="searchDto">Search filters and pagination details.</param>
    /// <returns>List of teams that match all provided filters.</returns>
    [Authorize]
    [HttpPost("search/advanced")]
    public async Task<ActionResult<PagedResult<GetTeamsCardDto>>> AdvancedSearch([FromBody] AdvancedSearchDto searchDto)
    {
        var userId = -1;
        if (searchDto.ShowPendingRequests)
        {
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
            userId = int.Parse(userIdClaim);
        }

        var validationResult = await _advancedSearchValidator.ValidateAsync(searchDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }

        var result = await _teamService.SearchTeamsAdvancedAsync(
            searchDto.GameIds ?? new List<int>(), 
            searchDto.PreferenceTagIds ?? new List<int>(), 
            searchDto.SearchTerm ?? "", 
            searchDto.Page, 
            searchDto.PageSize, 
            searchDto.SortBy ?? "",
            userId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<GetTeamDto>> CreateTeam([FromForm] CreateTeamDto createTeamDto)
    {
        var validationResult = await _createTeamValidator.ValidateAsync(createTeamDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }

        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
        var userId = int.Parse(userIdClaim);

        var team = await _teamService.CreateTeamAsync(createTeamDto, userId);
        return CreatedAtAction(nameof(GetTeamById), new { id = team.Id }, team);
    }
}