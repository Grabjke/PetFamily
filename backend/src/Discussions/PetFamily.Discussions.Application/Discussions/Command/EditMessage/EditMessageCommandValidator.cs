using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Core.ValueObjects.Discussion;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Discussions.Command.EditMessage;

public class EditMessageCommandValidator:AbstractValidator<EditMessageCommand>
{
    public EditMessageCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(c => c.DiscussionId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(c => c.MessageId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(x => x.Message)
            .NotEmpty()
            .MustBeValueObject(Text.Create);
    }
}