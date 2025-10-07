using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Discussions.Command.DeleteMessage;

public class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
{
    public DeleteMessageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(x => x.DiscussionId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(x => x.MessageId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}