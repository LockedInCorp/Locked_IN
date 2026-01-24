using AutoMapper;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Locked_IN_Backend.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository teamRepository, ITeamMemberRepository teamMemberRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _teamMemberRepository = teamMemberRepository;
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

    public async Task<PagedResult<GetTeamsCardDto>> SearchTeamsAdvancedAsync(List<int> gameIds, List<int> preferenceTagIds, string searchTerm, int page, int pageSize, string sortBy, int ShowPendingRequestsUserId)
    {
        var teamIds = new List<int>();
        if (ShowPendingRequestsUserId >= 0)
        {
           teamIds = _teamMemberRepository.GetPendingRequestsAsync(ShowPendingRequestsUserId).Result.Select(tm => tm.TeamId).ToList();
        }
        var pagedResults = await _teamRepository.GetTeamsAdvancedAsync(gameIds, preferenceTagIds, searchTerm, page, pageSize, sortBy,teamIds);
        
        return new PagedResult<GetTeamsCardDto>
        {
            Items = pagedResults.Items.Select(r =>
            {
                var dto = _mapper.Map<GetTeamsCardDto>(r.Team);
                dto.SearchRank = r.SearchRank;
                dto.TeamLeaderUsername = r.TeamLeaderUsername;
                return dto;
            }).ToList(),
            TotalCount = pagedResults.TotalCount,
            Page = pagedResults.Page,
            PageSize = pagedResults.PageSize,
            TotalPages = pagedResults.TotalPages
        };
    }
}