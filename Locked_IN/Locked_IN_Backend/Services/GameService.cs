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
    public async Task<List<GetGameDto>> GetGamesByNameAsync(string searchTerm = "")
    {
        var query = _context.Games.AsQueryable();
    
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var trimmedSearch = searchTerm.Trim();
            var searchArray = trimmedSearch.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var searchWithStar = string.Join(" & ", searchArray) + ":*";
            
            query = query.Where(t => EF.Functions.ToTsVector("english", t.Name).Matches(EF.Functions.ToTsQuery("english", searchWithStar)));
        }
    
        var games = await query
            .Select(t => new GetGameDto()
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync();
        
        return games;
    }

}
