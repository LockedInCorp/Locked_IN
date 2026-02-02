using Locked_IN_Backend.Data.Entities;

namespace Locked_IN_Backend.DTOs.Tags
{
    public class TagsResponseDto
    {
        public List<Game> Games { get; set; } = new();
        public List<ExperienceTag> ExperienceTags { get; set; } = new();
        public List<PreferenceTag> PreferenceTags { get; set; } = new();
    }
}