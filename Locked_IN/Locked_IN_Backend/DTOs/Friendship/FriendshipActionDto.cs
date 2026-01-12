using FluentValidation;

namespace Locked_IN_Backend.DTOs.Friendship
{
    public class FriendshipActionDto
    {
        public int FriendshipId { get; set; }
        public int CurrentUserId { get; set; }
    }

    public class FriendshipActionValidator : AbstractValidator<FriendshipActionDto>
    {
        public FriendshipActionValidator()
        {
            RuleFor(x => x.FriendshipId)
                .NotEmpty()
                .WithMessage("Friendship ID is required.");

            RuleFor(x => x.CurrentUserId)
                .NotEmpty()
                .WithMessage("Current User ID is required.");
        }
    }
}
