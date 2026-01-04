using Locked_IN_Backend.Data;
using Locked_IN_Backend.DTOs.Tags;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services
{
    public class TagService : ITagService
    {
        private readonly AppDbContext _context;

        public TagService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TagResult> GetAllTagsAsync()
        {
            var response = new TagsResponseDto
            {
                Games = await _context.Games.ToListAsync(),
                ExperienceTags = await _context.ExperienceTags.ToListAsync(),
                PreferenceTags = await _context.PreferenceTags.ToListAsync(),
                GameExperiences = await _context.GameExps.ToListAsync(),
                GameplayPreferences = await _context.GameplayPrefs.ToListAsync()
            };

            return new TagResult(true, "All tags retrieved successfully.", response);
        }
    }
}