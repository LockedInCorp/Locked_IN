using Locked_IN_Backend.Data;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services;

public class ExperienceTagService : IExperienceTagService
{
    private readonly AppDbContext _context;

    public ExperienceTagService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetPreferanceTagsDto>> GetExperienceTagsAsync()
    {
        return await _context.ExperienceTags
            .Select(t => new GetPreferanceTagsDto
            {
                Id = t.Id,
                Name = t.Experiencelevel
            })
            .ToListAsync();
    }
}
