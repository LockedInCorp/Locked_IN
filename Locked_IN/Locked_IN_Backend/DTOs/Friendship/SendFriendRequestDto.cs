using FluentValidation;

namespace Locked_IN_Backend.DTOs.Friendship
{
    public class SendFriendRequestDto
    {
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
    }

    public class SendFriendRequestValidator : AbstractValidator<SendFriendRequestDto>
    {
        public SendFriendRequestValidator()
        {
            RuleFor(x => x.RequesterId)
                .NotEmpty()
                .WithMessage("Requester ID is required.");

            RuleFor(x => x.ReceiverId)
                .NotEmpty()
                .WithMessage("Receiver ID is required.")
                .NotEqual(x => x.RequesterId)
                .WithMessage("Cannot send friend request to yourself.");
        }
    }
}