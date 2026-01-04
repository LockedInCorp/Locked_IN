using Locked_IN_Backend.DTOs;

namespace Locked_IN_Backend.Interfaces;

public interface IPreferanceTagsService
{
    public Task<List<GetTagsResponceModel>> GetPreferanceTagsAsync();
}