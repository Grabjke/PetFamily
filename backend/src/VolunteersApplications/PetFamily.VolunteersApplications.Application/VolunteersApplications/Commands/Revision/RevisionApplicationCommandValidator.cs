using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Revision;

public class RevisionApplicationCommandValidator : AbstractValidator<RevisionApplicationCommand>
{
    public RevisionApplicationCommandValidator()
    {
        RuleFor(a => a.AdminId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(a => a.ApplicationId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(a => a.Comment)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}