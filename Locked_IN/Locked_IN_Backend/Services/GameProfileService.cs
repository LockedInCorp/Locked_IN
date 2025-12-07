using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs.GameProfile;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services
{
    public class GameProfileService : IGameProfileService
    {
        private readonly AppDbContext _context;

        public GameProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GameProfileResult> GetUserFavoritesAsync(int userId)
        {
            var favorites = await _context.GameProfiles
                .Where(gp => gp.UserId == userId && gp.Isfavorite)
                .Include(gp => gp.Game)
                .Select(gp => new GameProfileDto
                {
                    Id = gp.Id,
                    UserId = gp.UserId,
                    GameId = gp.GameId,
                    GameName = gp.Game.Name,
                    IsFavorite = gp.Isfavorite,
                    Rank = gp.Rank.ToString()
                })
                .ToListAsync();

            return new GameProfileResult(true, "Favorites retrieved successfully.", favorites);
        }

        public async Task<GameProfileResult> AddGameToFavoritesAsync(int userId, int gameId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return new GameProfileResult(false, "User not found.");

            var game = await _context.Games.FindAsync(gameId);
            if (game == null) return new GameProfileResult(false, "Game not found.");

            var profile = await _context.GameProfiles
                .FirstOrDefaultAsync(gp => gp.UserId == userId && gp.GameId == gameId);

            if (profile != null)
            {
                if (profile.Isfavorite)
                {
                    return new GameProfileResult(false, "Game is already in favorites.");
                }
                
                profile.Isfavorite = true;
                _context.GameProfiles.Update(profile);
            }
            else
            {
                profile = new GameProfile
                {
                    UserId = userId,
                    GameId = gameId,
                    Isfavorite = true,
                    ExperienceTagId = 1, 
                    GameExpId = 1,       
                    Rank = null
                };
                _context.GameProfiles.Add(profile);
            }

            await _context.SaveChangesAsync();

            return new GameProfileResult(true, "Game added to favorites.", new GameProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                GameId = profile.GameId,
                GameName = game.Name,
                IsFavorite = profile.Isfavorite
            });
        }

        public async Task<GameProfileResult> RemoveGameFromFavoritesAsync(int userId, int gameId)
        {
            var profile = await _context.GameProfiles
                .Include(gp => gp.Game)
                .FirstOrDefaultAsync(gp => gp.UserId == userId && gp.GameId == gameId);

            if (profile == null)
            {
                return new GameProfileResult(false, "Game profile not found.");
            }

            if (!profile.Isfavorite)
            {
                return new GameProfileResult(false, "Game is not in favorites.");
            }

            profile.Isfavorite = false;
            _context.GameProfiles.Update(profile);
            await _context.SaveChangesAsync();

            return new GameProfileResult(true, "Game removed from favorites.", new GameProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                GameId = profile.GameId,
                GameName = profile.Game.Name,
                IsFavorite = profile.Isfavorite
            });
        }
    }
}