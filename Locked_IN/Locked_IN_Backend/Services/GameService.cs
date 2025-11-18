using Locked_IN_Backend.Data;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services;

public class GameService : IGameService
{
    private readonly AppDbContext _context;

    public GameService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<GetGamesResponceModel>> GetGamesByNameAsync(string searchTerm = "")
    {
        var query = _context.Games.AsQueryable();
    
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => EF.Functions.ToTsVector("english", t.Name).Matches(EF.Functions.WebSearchToTsQuery(searchTerm)));
        }
    
        var games = await query
            .Select(t => new GetGamesResponceModel()
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync();
        
        return games;
    }

}
