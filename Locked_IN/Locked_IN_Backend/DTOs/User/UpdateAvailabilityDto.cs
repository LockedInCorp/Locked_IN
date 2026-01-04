namespace Locked_IN_Backend.DTOs.User
{
    public class UpdateAvailabilityDto
    {
        public Dictionary<string, List<string>> Availability { get; set; } = new();
    }
}