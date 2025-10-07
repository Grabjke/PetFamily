using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Core.ValueObjects.Discussion;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Discussions.Command.SendMessage;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(x => x.DiscussionId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(x => x.Message)
            .NotEmpty()
            .MustBeValueObject(Text.Create);
    }
}