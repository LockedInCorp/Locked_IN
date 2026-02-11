using Locked_IN_Backend.Data;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services;

public class PreferanceTagsService : IPreferanceTagsService
{
    private readonly AppDbContext _context;

    public PreferanceTagsService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<GetPreferanceTagsDto>> GetPreferanceTagsAsync()
    {
        var query = _context.PreferenceTags.AsQueryable();
    
        var games = await query
            .Select(t => new GetPreferanceTagsDto()
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync();
        
        return games;
    }
    
}