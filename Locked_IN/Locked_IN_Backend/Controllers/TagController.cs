using Locked_IN_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Get all available tags, games, and preferences for filtering
        /// </summary>
        /// <returns>Lists of all tag entities</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var result = await _tagService.GetAllTagsAsync();
            return Ok(result);
        }
    }
}