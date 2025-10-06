using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Discussions.Command.Create;

public class CreateDiscussionCommandValidator : AbstractValidator<CreateDiscussionCommand>
{
    public CreateDiscussionCommandValidator()
    {
        RuleFor(c => c.RelationId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(c => c.UsersIds)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}