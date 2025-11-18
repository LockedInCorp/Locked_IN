using Microsoft.AspNetCore.Mvc;
using Locked_IN_Backend.Services;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.Interfaces;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    /// <summary>
    /// Get team by ID
    /// </summary>
    /// <param name="id">Team ID</param>
    /// <returns>Team details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetTeamResponseModel>> GetTeamById(int id)
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
    public async Task<ActionResult<List<GetTeamResponseModel>>> GetTeamsByGameId(int gameId)
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
    public async Task<ActionResult<List<GetTeamResponseModel>>> SearchTeamsByName([FromQuery] string searchTerm = "")
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

}