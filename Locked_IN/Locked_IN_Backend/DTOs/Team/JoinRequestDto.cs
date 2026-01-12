using FluentValidation;

namespace Locked_IN_Backend.DTOs
{
    public class JoinRequestDto
    {
        public int UserId { get; set; }
    }

    public class JoinRequestDtoValidator : AbstractValidator<JoinRequestDto>
    {
        public JoinRequestDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");
        }
    }
}