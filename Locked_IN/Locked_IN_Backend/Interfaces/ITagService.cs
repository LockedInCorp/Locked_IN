using Locked_IN_Backend.DTOs.Tags;

namespace Locked_IN_Backend.Services
{
    public record TagResult(bool Success, string Message, TagsResponseDto? Data = null);

    public interface ITagService
    {
        Task<TagResult> GetAllTagsAsync();
    }
}