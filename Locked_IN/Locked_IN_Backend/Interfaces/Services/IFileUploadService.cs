using Microsoft.AspNetCore.Http;

namespace Locked_IN_Backend.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadUserAvatarAsync(IFormFile file);
    Task<string> UploadTeamIconAsync(IFormFile file);
    Task<IFormFile?> GetUserAvatarAsync(string fileName);
    Task<IFormFile?> GetTeamIconAsync(string fileName);
    Task DeleteUserAvatarAsync(string fileName);
    Task DeleteTeamIconAsync(string fileName);
}