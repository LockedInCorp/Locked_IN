using FluentValidation;

namespace Locked_IN_Backend.DTOs.User
{
    public class UpdateAvailabilityDto
    {
        public Dictionary<string, List<string>> Availability { get; set; } = new();
    }

    public class UpdateAvailabilityDtoValidator : AbstractValidator<UpdateAvailabilityDto>
    {
        public UpdateAvailabilityDtoValidator()
        {
            RuleFor(x => x.Availability)
                .NotEmpty().WithMessage("Availability is required.");
        }
    }
}