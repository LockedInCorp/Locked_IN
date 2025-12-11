using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService teamService)
    {
        _tagService = teamService;
    }
    [HttpGet("")]
    public async Task<ActionResult<List<GetTagsResponceModel>>> GetAllTags()
    {
        try
        {
            var tags = await _tagService.GetTagsAsync();
            return Ok(tags);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}