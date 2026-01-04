using AutoMapper;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Repositories;

namespace Locked_IN_Backend.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }
    
    public async Task<GetTeamDto?> GetTeamByIdAsync(int teamId)
    {
        var team = await _teamRepository.GetTeamById(teamId);
        return _mapper.Map<GetTeamDto>(team);
    }

    public async Task<List<GetTeamDto>> GetTeamsByGameIdAsync(int gameId)
    {
        var teams = await _teamRepository.GetTeamsByGameIdAsync(gameId);
        return _mapper.Map<List<GetTeamDto>>(teams);
    }

    public async Task<List<GetTeamDto>> GetTeamsByNameSearchAsync(string searchTerm = "")
    {
        var searchResults = await _teamRepository.GetTeamsByNameSearchAsync(searchTerm);
        return searchResults.Select(r =>
        {
            var dto = _mapper.Map<GetTeamDto>(r.Team);
            dto.SearchRank = r.SearchRank;
            return dto;
        }).ToList();
    }

    public async Task<PagedResult<GetTeamsCardDto>> SearchTeamsAdvancedAsync(List<int> gameIds, List<int> preferenceTagIds, string searchTerm, int page, int pageSize, string sortBy)
    {
        var pagedResults = await _teamRepository.GetTeamsAdvancedAsync(gameIds, preferenceTagIds, searchTerm, page, pageSize, sortBy);
        
        return new PagedResult<GetTeamsCardDto>
        {
            Items = pagedResults.Items.Select(r =>
            {
                var dto = _mapper.Map<GetTeamsCardDto>(r.Team);
                dto.SearchRank = r.SearchRank;
                dto.TeamLeaderNickname = r.TeamLeaderNickname;
                return dto;
            }).ToList(),
            TotalCount = pagedResults.TotalCount,
            Page = pagedResults.Page,
            PageSize = pagedResults.PageSize,
            TotalPages = pagedResults.TotalPages
        };
    }
}