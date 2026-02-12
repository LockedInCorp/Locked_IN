namespace Locked_IN_Backend.Interfaces.Services;

public interface IInviteTokenService
{
    string GenerateInviteToken(int teamId);
    int DecodeInviteToken(string token);
}
