using FluentValidation;

namespace Locked_IN_Backend.DTOs.Friendship
{
    public class SendFriendRequestDto
    {
        public int ReceiverId { get; set; }
    }

    public class SendFriendRequestValidator : AbstractValidator<SendFriendRequestDto>
    {
        public SendFriendRequestValidator()
        {
            RuleFor(x => x.ReceiverId)
                .NotEmpty().WithMessage("Receiver ID is required.")
                .GreaterThan(0).WithMessage("Invalid Receiver ID.");
        }
    }
}