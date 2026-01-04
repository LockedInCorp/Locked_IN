using Locked_IN_Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreferanceTagController : ControllerBase
{
    private readonly IPreferanceTagsService _tagService;

    public PreferanceTagController(IPreferanceTagsService tagService)
    {
        _tagService = tagService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTagsAsync()
    {
        var result = await _tagService.GetPreferanceTagsAsync();
        return Ok(result);
    }
    
}