using FluentValidation;
using Locked_IN_Backend.Misc;

namespace Locked_IN_Backend.DTOs.Team;

public class AdvancedSearchDto
{
    public List<int> GameIds { get; set; } = new List<int>();
    public List<int> PreferenceTagIds { get; set; } = new List<int>();
    public string SearchTerm { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = ValidationConstraints.DefaultPageSize;
    public string SortBy { get; set; } = string.Empty;
    public bool ShowPendingRequests { get; set; } = false;
}

public class AdvancedSearchDtoValidator : AbstractValidator<AdvancedSearchDto>
{
    public AdvancedSearchDtoValidator()
    {
        RuleFor(x => x.PageSize)
            .Must(pageSize => ValidationConstraints.AllowedPageSizes.Contains(pageSize))
            .WithMessage($"PageSize must be one of the following: {string.Join(", ", ValidationConstraints.AllowedPageSizes)}");

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1");
    }
}
