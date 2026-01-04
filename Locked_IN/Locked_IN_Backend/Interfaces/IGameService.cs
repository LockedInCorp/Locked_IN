using Locked_IN_Backend.DTOs;

namespace Locked_IN_Backend.Interfaces;

public interface IGameService
{ 
    Task<List<GetGamesResponceModel>> GetGamesByNameAsync(string searchTerm);
}