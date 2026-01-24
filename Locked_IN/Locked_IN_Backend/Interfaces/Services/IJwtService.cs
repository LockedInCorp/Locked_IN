using Locked_IN_Backend.Data.Entities;

namespace Locked_IN_Backend.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}
