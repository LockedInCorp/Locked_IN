using FluentValidation;

namespace Locked_IN_Backend.DTOs.Friendship
{
    public class BlockUserDto
    {
        public int BlockerId { get; set; }
        public int UserToBlockId { get; set; }
    }

    public class BlockUserValidator : AbstractValidator<BlockUserDto>
    {
        public BlockUserValidator()
        {
            RuleFor(x => x.BlockerId)
                .NotEmpty()
                .WithMessage("Blocker ID is required.");

            RuleFor(x => x.UserToBlockId)
                .NotEmpty()
                .WithMessage("User to block ID is required.")
                .NotEqual(x => x.BlockerId)
                .WithMessage("Cannot block yourself.");
        }
    }

    public class UnblockUserDto
    {
        public int BlockerId { get; set; }
        public int UserToUnblockId { get; set; }
    }

    public class UnblockUserValidator : AbstractValidator<UnblockUserDto>
    {
        public UnblockUserValidator()
        {
            RuleFor(x => x.BlockerId)
                .NotEmpty()
                .WithMessage("Blocker ID is required.");

            RuleFor(x => x.UserToUnblockId)
                .NotEmpty()
                .WithMessage("User to unblock ID is required.")
                .NotEqual(x => x.BlockerId)
                .WithMessage("Cannot unblock yourself.");;
        }
    }
}
