using Microsoft.AspNetCore.Http;

namespace Locked_IN_Backend.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadUserAvatarAsync(IFormFile file);
    Task<string> UploadTeamIconAsync(IFormFile file);
    Task<string> UploadAttachmentAsync(IFormFile file);
    Task<IFormFile?> GetFileAsync(string bucketName, string fileName);
    Task DeleteFileAsync(string bucketName, string fileName);
}