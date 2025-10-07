using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Discussions.Command.Close;

public class CloseDiscussionCommandValidator : AbstractValidator<CloseDiscussionCommand>
{
    public CloseDiscussionCommandValidator()
    {
        RuleFor(d => d.DiscussionId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(d => d.AdminId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}