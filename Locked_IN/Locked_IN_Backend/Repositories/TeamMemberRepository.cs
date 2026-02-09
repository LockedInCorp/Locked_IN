using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Misc.Enum;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Repositories;

public class TeamMemberRepository : ITeamMemberRepository
{
    private readonly AppDbContext _context;

    public TeamMemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TeamMember>> GetTeamMembersAsync()
    {
        return await _context.TeamMembers.ToListAsync();
    }

    public async Task<TeamMember?> GetTeamMemberByIdAsync(int teamId, int userId)
    {
        return await _context.TeamMembers.FindAsync(teamId, userId);
    }

    public async Task AddTeamMemberAsync(TeamMember teamMember)
    {
        await _context.TeamMembers.AddAsync(teamMember);
    }

    public async Task UpdateTeamMemberAsync(TeamMember teamMember)
    {
        _context.TeamMembers.Update(teamMember);
    }

    public async Task DeleteTeamMemberAsync(TeamMember teamMember)
    {
        _context.TeamMembers.Remove(teamMember);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetActiveMemberCountAsync(int teamId)
    {
        return await _context.TeamMembers
            .CountAsync(tm => tm.TeamId == teamId &&
                             (tm.MemberStatusId == (int)TeamMemberStatus.STATUS_MEMBER ||
                              tm.MemberStatusId == (int)TeamMemberStatus.STATUS_LEADER));
    }

    public async Task<TeamMember?> GetTeamMemberAsync(int teamId, int userId)
    {
        return await _context.TeamMembers
            .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
    }

    public async Task<List<TeamMember>> GetPendingRequestsAsync(int userId)
    {
        return await _context.TeamMembers
            .Where(tm => tm.UserId == userId && tm.MemberStatusId == (int)TeamMemberStatus.STATUS_PENDING).ToListAsync();
    }

    public async Task<List<TeamMember>> GetPendingRequestsByTeamIdAsync(int teamId)
    {
        return await _context.TeamMembers
            .Include(tm => tm.User)
            .Where(tm => tm.TeamId == teamId && tm.MemberStatusId == (int)TeamMemberStatus.STATUS_PENDING)
            .ToListAsync();
    }

    public async Task<TeamMember?> GetTeamMemberWithTeamByIdAsync(int teamId, int userId)
    {
        return await _context.TeamMembers
            .Include(tm => tm.Team)
            .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
    }

    public async Task<List<TeamMember>> GetTeamMembersByTeamIdAsync(int teamId)
    {
        return await _context.TeamMembers.Where(tm => tm.TeamId == teamId).ToListAsync();
    }

    public async Task<TeamMember?> GetTeamLeaderAsync(int teamId)
    {
        return await _context.TeamMembers
            .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.Isleader);
    }
}