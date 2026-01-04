using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    
    private readonly IGameService _teamService;

    public GameController(IGameService teamService)
    {
        _teamService = teamService;
    }
    [HttpGet("search")]
    public async Task<ActionResult<List<GetGameDto>>> SearchGamesByName([FromQuery] string searchTerm = "")
    {
        try
        {
            var teams = await _teamService.GetGamesByNameAsync(searchTerm);
            return Ok(teams);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}