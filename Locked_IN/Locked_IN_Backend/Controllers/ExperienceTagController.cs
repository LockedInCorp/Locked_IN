using Locked_IN_Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExperienceTagController : ControllerBase
{
    private readonly IExperienceTagService _experienceTagService;

    public ExperienceTagController(IExperienceTagService experienceTagService)
    {
        _experienceTagService = experienceTagService;
    }

    [HttpGet]
    public async Task<IActionResult> GetExperienceTags()
    {
        var result = await _experienceTagService.GetExperienceTagsAsync();
        return Ok(result);
    }
}
