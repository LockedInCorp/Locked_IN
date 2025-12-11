using Locked_IN_Backend.Data;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services;

public class TagsService : ITagService
{
    private readonly AppDbContext _context;

    public TagsService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<GetTagsResponceModel>> GetTagsAsync()
    {
        var query = _context.PreferenceTags.AsQueryable();
    
        var games = await query
            .Select(t => new GetTagsResponceModel()
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync();
        
        return games;
    }
    
}