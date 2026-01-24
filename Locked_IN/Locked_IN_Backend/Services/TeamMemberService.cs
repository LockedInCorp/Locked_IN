using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Misc;
using Locked_IN_Backend.Misc.Enum;

namespace Locked_IN_Backend.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IUserRepository _userRepository;

        public TeamMemberService(ITeamRepository teamRepository, ITeamMemberRepository teamMemberRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _teamMemberRepository = teamMemberRepository;
            _userRepository = userRepository;
        }

        public async Task<TeamJoinResult> RequestToJoinTeamAsync(int teamId, int userId)
        {
            var team = await _teamRepository.GetTeamById(teamId);
            if (team == null)
            {
                return new TeamJoinResult(TeamJoinResultStatus.NotFound, "Team not found");
            }

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return new TeamJoinResult(TeamJoinResultStatus.NotFound, "User not found");
            }

            var activeMemberCount = await _teamMemberRepository.GetActiveMemberCountAsync(teamId);

            if (activeMemberCount >= team.MaxPlayerCount)
            {
                return new TeamJoinResult(TeamJoinResultStatus.BadRequest, "Team is already full");
            }

            var preexistentTeamMember = await _teamMemberRepository.GetTeamMemberAsync(teamId, userId);

            if (preexistentTeamMember != null)
            {
                if (preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_PENDING)
                {
                    return new TeamJoinResult(TeamJoinResultStatus.Conflict, "User already has a pending join request");
                }
                if (preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_REJECTED)
                {
                    return new TeamJoinResult(TeamJoinResultStatus.Conflict, "User has been rejected from the team");
                }
                if (preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_MEMBER ||
                    preexistentTeamMember.MemberStatusId == (int)TeamMemberStatus.STATUS_LEADER)
                {
                    return new TeamJoinResult(TeamJoinResultStatus.Conflict, "User is already a member of that team");
                }
            }

            var pendingRequests = await _teamMemberRepository.GetPendingRequestsAsync(userId);

            if (pendingRequests.Count >= ValidationConstraints.MaxActiveJoinRequestsPerUser)
            {
                return new TeamJoinResult(TeamJoinResultStatus.BadRequest, "User has reached the maximum number of active join requests");
            }

            int newMemberStatusId = team.Isprivate ? (int)TeamMemberStatus.STATUS_PENDING : (int)TeamMemberStatus.STATUS_MEMBER;

            var newTeamMember = new TeamMember
            {
                TeamId = teamId,
                UserId = userId,
                MemberStatusId = newMemberStatusId,
                Jointimestamp = DateTime.UtcNow,
                Isleader = false
            };

            await _teamMemberRepository.AddTeamMemberAsync(newTeamMember);

            string message = newMemberStatusId == (int)TeamMemberStatus.STATUS_PENDING
                ? "Join request sent successfully. Awaiting admin approval."
                : "Successfully joined public team.";
                
            return new TeamJoinResult(TeamJoinResultStatus.Success, message);
        }

        public async Task<List<TeamJoinResponceDto>> GetJoinRequestsAsync(int teamId)
        {
            var requests = await _teamMemberRepository.GetPendingRequestsByTeamIdAsync(teamId);
            return requests.Select(tm => new TeamJoinResponceDto
                {
                    TeamId = tm.TeamId,
                    UserId = tm.UserId,
                    Username = tm.User.UserName,
                    RequestTimestamp = tm.Jointimestamp
                })
                .ToList();
        }

        public async Task<TeamJoinResult> AcceptJoinRequestAsync(int teamId, int userId)
        {
            var request = await _teamMemberRepository.GetTeamMemberWithTeamByIdAsync(teamId, userId);

            if (request == null || request.MemberStatusId != (int)TeamMemberStatus.STATUS_PENDING)
            {
                return new TeamJoinResult(TeamJoinResultStatus.NotFound, "Join request not found or already handled");
            }

            var activeMemberCount = await _teamMemberRepository.GetActiveMemberCountAsync(request.TeamId);

            if (activeMemberCount >= request.Team.MaxPlayerCount)
            {
                return new TeamJoinResult(TeamJoinResultStatus.BadRequest, "Team is already full");
            }
            
            request.MemberStatusId = (int)TeamMemberStatus.STATUS_MEMBER;
            request.Jointimestamp = DateTime.UtcNow;

            await _teamMemberRepository.UpdateTeamMemberAsync(request);

            return new TeamJoinResult(TeamJoinResultStatus.Success, "User approved and added to team");
        }

        public async Task<TeamJoinResult> DeclineJoinRequestAsync(int teamId, int userId)
        {
            var request = await _teamMemberRepository.GetTeamMemberByIdAsync(teamId, userId);

            if (request == null || request.MemberStatusId != (int)TeamMemberStatus.STATUS_PENDING)
            {
                return new TeamJoinResult(TeamJoinResultStatus.NotFound, "Join request not found or already handled");
            }

            request.MemberStatusId = (int)TeamMemberStatus.STATUS_REJECTED;

            await _teamMemberRepository.UpdateTeamMemberAsync(request);

            return new TeamJoinResult(TeamJoinResultStatus.Success, "Join request declined");
        }

        public async Task<TeamJoinResult> CancelJoinRequestAsync(int teamId, int userId)
        {
            var request = await _teamMemberRepository.GetTeamMemberAsync(teamId, userId);

            if (request == null || request.MemberStatusId != (int)TeamMemberStatus.STATUS_PENDING)
            {
                return new TeamJoinResult(TeamJoinResultStatus.NotFound, "Pending join request not found for this user and team");
            }

            await _teamMemberRepository.DeleteTeamMemberAsync(request);

            return new TeamJoinResult(TeamJoinResultStatus.Success, "Join request successfully cancelled");
        }
    }
}