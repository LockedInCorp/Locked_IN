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

    public TeamController(ITeamService teamService, IValidator<AdvancedSearchDto> advancedSearchValidator)
    {
        _teamService = teamService;
        _advancedSearchValidator = advancedSearchValidator;
    }

    /// <summary>
    /// Get team by ID
    /// </summary>
    /// <param name="id">Team ID</param>
    /// <returns>Team details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetTeamDto>> GetTeamById(int id)
    {
        try
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            
            if (team == null)
                return NotFound($"Team with ID {id} not found");

            return Ok(team);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Get teams by game ID
    /// </summary>
    /// <param name="gameId">Game ID</param>
    /// <returns>List of teams for the specified game</returns>
    [HttpGet("game/{gameId}")]
    public async Task<ActionResult<List<GetTeamDto>>> GetTeamsByGameId(int gameId)
    {
        try
        {
            var teams = await _teamService.GetTeamsByGameIdAsync(gameId);
            return Ok(teams);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Search teams by name
    /// </summary>
    /// <param name="searchTerm">Search term to find teams by name</param>
    /// <returns>List of teams matching the search term, ordered by relevance. Returns all games if searchTerm is null or empty</returns>
    [HttpGet("search")]
    public async Task<ActionResult<List<GetTeamDto>>> SearchTeamsByName([FromQuery] string searchTerm = "")
    {
        try
        {
            var teams = await _teamService.GetTeamsByNameSearchAsync(searchTerm);
            return Ok(teams);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
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
        try
        {
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
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}