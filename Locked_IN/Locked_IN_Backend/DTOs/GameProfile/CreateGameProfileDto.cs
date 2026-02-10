using FluentValidation;

namespace Locked_IN_Backend.DTOs.GameProfile;

public class CreateGameProfileDto
{
    public int GameId { get; set; }
    public int ExperienceTagId { get; set; }
    public int GameExpId { get; set; } 
    public int? Rank { get; set; }
    public string? Role { get; set; }
    public string? InGameNickname { get; set; }
    public bool IsFavorite { get; set; } = false;
}

public class CreateGameProfileDtoValidator : AbstractValidator<CreateGameProfileDto>
{
    public CreateGameProfileDtoValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty().WithMessage("GameId is required.");

        RuleFor(x => x.ExperienceTagId)
            .NotEmpty().WithMessage("ExperienceTagId is required.");
            
        RuleFor(x => x.GameExpId)
            .NotEmpty().WithMessage("GameExpId is required.");

        RuleFor(x => x.Rank)
            .GreaterThanOrEqualTo(0).When(x => x.Rank.HasValue)
            .WithMessage("Rank cannot be negative.");
    }
}