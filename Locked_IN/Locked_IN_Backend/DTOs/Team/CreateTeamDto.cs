using FluentValidation;
using Locked_IN_Backend.Misc;
using Microsoft.AspNetCore.Http;

namespace Locked_IN_Backend.DTOs.Team;

public class CreateTeamDto
{
    public string Name { get; set; } = null!;

    public int GameId { get; set; }

    public int MaxMembers { get; set; }

    public bool IsPrivate { get; set; }

    public bool AutoAccept { get; set; }

    public IFormFile? PreviewImage { get; set; }

    public List<int> Tags { get; set; } = new();

    public int Experience { get; set; }

    public int? MinCompetitiveScore { get; set; }

    public int? CommunicationService { get; set; }

    public string? CommunicationServiceLink { get; set; }

    public string? Description { get; set; }
}

public class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
{
    public CreateTeamDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(20).WithMessage("Name cannot exceed 20 characters");

        RuleFor(x => x.GameId)
            .NotEmpty().WithMessage("GameId is required");

        RuleFor(x => x.MaxMembers)
            .InclusiveBetween(2, ValidationConstraints.DefaultMaxTeamSize).WithMessage($"MaxMembers must be between 2 and {ValidationConstraints.DefaultMaxTeamSize}");

        RuleFor(x => x.Experience)
            .NotEmpty().WithMessage("Experience is required");

        RuleFor(x => x.Description)
            .MaximumLength(50).WithMessage("Description cannot exceed 50 characters");
            
        RuleFor(x => x.CommunicationServiceLink)
            .MaximumLength(255).WithMessage("CommunicationServiceLink cannot exceed 255 characters");
        RuleFor(x => x.MinCompetitiveScore)
            .GreaterThanOrEqualTo(0).WithMessage("Minimal competitive score cannot be negative");
    }
}
