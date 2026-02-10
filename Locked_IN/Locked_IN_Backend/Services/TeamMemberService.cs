using AutoMapper;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTO;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.DTOs.Team;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Misc;
using Locked_IN_Backend.Misc.Enum;
using Locked_IN_Backend.Exceptions;
using Microsoft.AspNetCore.SignalR;
using Locked_IN_Backend.Hubs;
using Locked_IN_Backend.Interfaces;

namespace Locked_IN_Backend.Services;

public class TeamMemberService : ITeamMemberService
{
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IChatService _chatService;
    private readonly IHubContext<TeamJoinHub, ITeamMemberHub> _hubContext;
    private readonly IMapper _mapper;
    
    public TeamMemberService(
        ITeamRepository teamRepository, 
        ITeamMemberRepository teamMemberRepository, 
        IUserRepository userRepository, 
        IHubContext<TeamJoinHub, ITeamMemberHub> hubContext, IMapper mapper, IChatRepository chatRepository, IChatService chatService)
    {
        _teamRepository = teamRepository;
        _teamMemberRepository = teamMemberRepository;
        _userRepository = userRepository;
        _hubContext = hubContext;
        _mapper = mapper;
        _chatService = chatService;
    }

    public async Task RequestToJoinTeamAsync(int teamId, int userId)
    {
        var team = await _teamRepository.GetTeamById(teamId);
        if (team == null) throw new NotFoundException("Team not found");

        var user = await _userRepository.GetUserById(userId);
        if (user == null) throw new NotFoundException("User not found");

        var activeMemberCount = await _teamMemberRepository.GetActiveMemberCountAsync(teamId);
        if (activeMemberCount >= team.MaxPlayerCount)
        {
            throw new BadRequestException("Team is already full");
        }

        var preexistentTeamMember = await _teamMemberRepository.GetTeamMemberAsync(teamId, userId);

        if (preexistentTeamMember != null)
        {
            if (preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_PENDING)
                throw new ConflictException("User already has a pending join request");
            
            if (preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_REJECTED)
                throw new ConflictException("User has been rejected from the team");
            
            if (preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_MEMBER ||
                preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_LEADER)
                throw new ConflictException("User is already a member of that team");
        }

        var pendingRequests = await _teamMemberRepository.GetPendingRequestsAsync(userId);
        if (pendingRequests.Count >= ValidationConstraints.MaxActiveJoinRequestsPerUser)
        {
            throw new BadRequestException("User has reached the maximum number of active join requests");
        }

        int newMemberStatusId = team.IsAutoaccept ? (int)TeamMemberStatus.STATUS_MEMBER : (int)TeamMemberStatus.STATUS_PENDING;

        var newTeamMember = new TeamMember
        {
            TeamId = teamId,
            UserId = userId,
            MemberStatusId = newMemberStatusId,
            Jointimestamp = DateTime.UtcNow,
            Isleader = false
        };

        await _teamMemberRepository.AddTeamMemberAsync(newTeamMember);
        if (team.IsAutoaccept) await _chatService.JoinChatGroupAsync(userId, team.Chats.First().Id);
        await _teamMemberRepository.SaveChangesAsync();

        var leader = await _teamMemberRepository.GetTeamLeaderAsync(teamId);
        if (leader != null)
        {
            await _hubContext.Clients.User(leader.UserId.ToString()).ReceiveJoinRequestStatus(new TeamJoinStatusDto
            {
                TeamId = teamId,
                TeamName = team.Name,
                Status = newMemberStatusId
            });
        }
    }

    public async Task<List<GetTeamMemberDto>> GetActiveTeamMembersAsync(int teamId)
    {
        var members = await _teamMemberRepository.GetActiveTeamMembersAsync(teamId);
        var result = _mapper.Map<List<GetTeamMemberDto>>(members);
        return result;
    }

    public async Task<List<TeamJoinResponceDto>> GetJoinRequestsAsync(int teamId, int requesterId)
    {
        await EnsureUserIsLeaderAsync(teamId, requesterId);

        var requests = await _teamMemberRepository.GetPendingRequestsByTeamIdAsync(teamId);
        return _mapper.Map<List<TeamJoinResponceDto>>(requests);
    }

    public async Task AcceptJoinRequestAsync(int leaderId, int teamId, int userIdToAccept)
    {
        await EnsureUserIsLeaderAsync(teamId, leaderId);

        var request = await _teamMemberRepository.GetTeamMemberWithTeamByIdAsync(teamId, userIdToAccept);

        if (request == null || request.MemberStatusId != (int)TeamMemberStatus.STATUS_PENDING)
        {
            throw new NotFoundException("Join request not found or already handled");
        }

        var activeMemberCount = await _teamMemberRepository.GetActiveMemberCountAsync(request.TeamId);
        if (activeMemberCount >= request.Team.MaxPlayerCount)
        {
            throw new BadRequestException("Team is already full");
        }
        
        request.MemberStatusId = (int)TeamMemberStatus.STATUS_MEMBER;
        request.Jointimestamp = DateTime.UtcNow;

        await _teamMemberRepository.UpdateTeamMemberAsync(request);
        await _chatService.JoinChatGroupAsync(userIdToAccept, request.Team.Chats.First().Id);
        await _teamMemberRepository.SaveChangesAsync();

        await _hubContext.Clients.User(userIdToAccept.ToString()).ReceiveJoinRequestStatus(new TeamJoinStatusDto
        {
            TeamId = teamId,
            TeamName = request.Team.Name,
            Status = (int)TeamMemberStatus.STATUS_MEMBER
        });
    }

    public async Task DeclineJoinRequestAsync(int leaderId, int teamId, int userIdToDecline)
    {
        await EnsureUserIsLeaderAsync(teamId, leaderId);

        var request = await _teamMemberRepository.GetTeamMemberWithTeamByIdAsync(teamId, userIdToDecline);

        if (request == null || request.MemberStatusId != (int)TeamMemberStatus.STATUS_PENDING)
        {
            throw new NotFoundException("Join request not found or already handled");
        }

        request.MemberStatusId = (int)TeamMemberStatus.STATUS_REJECTED;

        await _teamMemberRepository.UpdateTeamMemberAsync(request);
        await _teamMemberRepository.SaveChangesAsync();

        await _hubContext.Clients.User(userIdToDecline.ToString()).ReceiveJoinRequestStatus(new TeamJoinStatusDto
        {
            TeamId = teamId,
            TeamName = request.Team.Name,
            Status = (int)TeamMemberStatus.STATUS_REJECTED
        });
    }

    public async Task CancelJoinRequestAsync(int teamId, int userId)
    {
        var request = await _teamMemberRepository.GetTeamMemberWithTeamByIdAsync(teamId, userId);

        if (request == null || request.MemberStatusId != (int)TeamMemberStatus.STATUS_PENDING)
        {
            throw new NotFoundException("Pending join request not found for this user and team");
        }

        await _teamMemberRepository.DeleteTeamMemberAsync(request);
        await _teamMemberRepository.SaveChangesAsync();

        var leader = await _teamMemberRepository.GetTeamLeaderAsync(teamId);
        if (leader != null)
        {
            int statusNone = 0; 
            if (Enum.IsDefined(typeof(TeamMemberStatus), "STATUS_NONE"))
            {
                 statusNone = (int)Enum.Parse(typeof(TeamMemberStatus), "STATUS_NONE");
            }

            await _hubContext.Clients.User(leader.UserId.ToString()).ReceiveJoinRequestStatus(new TeamJoinStatusDto
            {
                TeamId = teamId,
                TeamName = request.Team.Name,
                Status = statusNone
            });
        }
    }

    public async Task LeaveTeamAsync(int teamId, int userId)
    {
        var member = await _teamMemberRepository.GetTeamMemberWithTeamByIdAsync(teamId, userId);

        if (member == null || (member.MemberStatusId != (int)TeamMemberStatus.STATUS_MEMBER && member.MemberStatusId != (int)TeamMemberStatus.STATUS_LEADER))
        {
            throw new NotFoundException("User is not a member of this team.");
        }

        if (member.Isleader)
        {
            var activeMembers = await _teamMemberRepository.GetActiveTeamMembersAsync(teamId);
            if (activeMembers.Count > 1)
            {
                var newLeader = activeMembers
                    .Where(m => m.UserId != userId)
                    .OrderBy(m => m.Jointimestamp)
                    .FirstOrDefault();

                if (newLeader != null)
                {
                    newLeader.Isleader = true;
                    newLeader.MemberStatusId = (int)TeamMemberStatus.STATUS_LEADER;
                    await _teamMemberRepository.UpdateTeamMemberAsync(newLeader);
                }
            }
        }

        if (member.Team.Chats != null && member.Team.Chats.Any())
        {
            foreach (var chat in member.Team.Chats)
            {
                try
                {
                    await _chatService.LeaveChatGroupAsync(userId, chat.Id);
                }
                catch (ForbiddenException)
                {
                }
            }
        }

        await _teamMemberRepository.DeleteTeamMemberAsync(member);
        await _teamMemberRepository.SaveChangesAsync();
    }

    private async Task EnsureUserIsLeaderAsync(int teamId, int userId)
    {
        var member = await _teamMemberRepository.GetTeamMemberAsync(teamId, userId);
        
        if (member == null)
        {
            throw new ForbiddenException("You are not a member of this team.");
        }

        if (!member.Isleader)
        {
            throw new ForbiddenException("Only the team leader can perform this action.");
        }
    }
}