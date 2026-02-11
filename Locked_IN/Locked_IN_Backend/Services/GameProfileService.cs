using AutoMapper;
using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs.GameProfile;
using Locked_IN_Backend.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services;

public class GameProfileService : IGameProfileService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GameProfileService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<GameProfileDto>> GetUserFavoritesAsync(int userId)
    {
        var favorites = await _context.GameProfiles
            .Include(gp => gp.Game)
            .Where(gp => gp.UserId == userId && gp.Isfavorite)
            .ToListAsync();

        return _mapper.Map<List<GameProfileDto>>(favorites);
    }

    public async Task<List<GameProfileDto>> GetUserGameProfilesAsync(int userId)
    {
        var profiles = await _context.GameProfiles
            .Include(gp => gp.Game)
            .Include(gp => gp.GameprofilePreferencetagRelations)
            .ThenInclude(rel => rel.PreferenceTag)
            .Where(gp => gp.UserId == userId)
            .ToListAsync();

        return _mapper.Map<List<GameProfileDto>>(profiles);
    }

    public async Task<GameProfileDto> AddGameToFavoritesAsync(int userId, int gameId)
    {
        var game = await _context.Games.FindAsync(gameId);
        if (game == null) throw new NotFoundException($"Game with ID {gameId} not found.");

        var profile = await _context.GameProfiles
            .Include(gp => gp.Game)
            .FirstOrDefaultAsync(gp => gp.UserId == userId && gp.GameId == gameId);

        if (profile != null)
        {
            if (profile.Isfavorite)
            {
                throw new ConflictException("Game is already in favorites.");
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
            await _context.GameProfiles.AddAsync(profile);
            
            profile.Game = game;
        }

        await _context.SaveChangesAsync();
        return _mapper.Map<GameProfileDto>(profile);
    }

    public async Task<GameProfileDto> RemoveGameFromFavoritesAsync(int userId, int gameId)
    {
        var profile = await _context.GameProfiles
            .Include(gp => gp.Game)
            .FirstOrDefaultAsync(gp => gp.UserId == userId && gp.GameId == gameId);

        if (profile == null)
        {
            throw new NotFoundException("Game profile not found.");
        }

        if (!profile.Isfavorite)
        {
            throw new ConflictException("Game is not in favorites.");
        }

        profile.Isfavorite = false;
        _context.GameProfiles.Update(profile);
        await _context.SaveChangesAsync();

        return _mapper.Map<GameProfileDto>(profile);
    }

    public async Task<GameProfileDto> CreateGameProfileAsync(int userId, CreateGameProfileDto dto)
    {
        var existingProfile = await _context.GameProfiles
            .FirstOrDefaultAsync(gp => gp.UserId == userId && gp.GameId == dto.GameId);

        if (existingProfile != null)
        {
            throw new ConflictException("Profile for this game already exists.");
        }

        var game = await _context.Games.FindAsync(dto.GameId);
        if (game == null) throw new NotFoundException($"Game with ID {dto.GameId} not found.");

        var profile = _mapper.Map<GameProfile>(dto);
        profile.UserId = userId;

        await _context.GameProfiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        if (dto.PreferenceTagIds != null && dto.PreferenceTagIds.Any())
        {
            foreach (var tagId in dto.PreferenceTagIds)
            {
                var relation = new GameprofilePreferencetagRelation
                {
                    GameProfileId = profile.Id,
                    PreferenceTagId = tagId
                };
                await _context.GameprofilePreferencetagRelations.AddAsync(relation);
            }
            await _context.SaveChangesAsync();
        }

        await _context.Entry(profile).Reference(p => p.Game).LoadAsync();
        await _context.Entry(profile).Collection(p => p.GameprofilePreferencetagRelations).Query().Include(r => r.PreferenceTag).LoadAsync();

        return _mapper.Map<GameProfileDto>(profile);
    }

    public async Task<GameProfileDto> UpdateGameProfileAsync(int userId, int profileId, UpdateGameProfileDto dto)
    {
        var profile = await _context.GameProfiles
            .Include(gp => gp.Game)
            .Include(gp => gp.GameprofilePreferencetagRelations)
            .FirstOrDefaultAsync(gp => gp.Id == profileId && gp.UserId == userId);

        if (profile == null)
        {
            throw new NotFoundException("Game profile not found.");
        }
        
        profile.Rank = dto.Rank;
        profile.Isfavorite = dto.IsFavorite;
        profile.ExperienceTagId = dto.ExperienceTagId;
        profile.GameExpId = dto.GameExpId;
        profile.Role = dto.Role;
        profile.InGameNickname = dto.InGameNickname;

        if (profile.GameprofilePreferencetagRelations.Any())
        {
            _context.GameprofilePreferencetagRelations.RemoveRange(profile.GameprofilePreferencetagRelations);
        }
        
        if (dto.PreferenceTagIds != null && dto.PreferenceTagIds.Any())
        {
            foreach (var tagId in dto.PreferenceTagIds)
            {
                var relation = new GameprofilePreferencetagRelation
                {
                    GameProfileId = profile.Id,
                    PreferenceTagId = tagId
                };
                await _context.GameprofilePreferencetagRelations.AddAsync(relation);
            }
        }

        _context.GameProfiles.Update(profile);
        await _context.SaveChangesAsync();
        
        await _context.Entry(profile).Collection(p => p.GameprofilePreferencetagRelations).Query().Include(r => r.PreferenceTag).LoadAsync();

        return _mapper.Map<GameProfileDto>(profile);
    }

    public async Task DeleteGameProfileAsync(int userId, int profileId)
    {
        var profile = await _context.GameProfiles
            .FirstOrDefaultAsync(gp => gp.Id == profileId && gp.UserId == userId);

        if (profile == null)
        {
            throw new NotFoundException("Game profile not found.");
        }

        _context.GameProfiles.Remove(profile);
        await _context.SaveChangesAsync();
    }
}