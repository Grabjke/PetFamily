using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Reject;

public class RejectApplicationCommandValidator:AbstractValidator<RejectApplicationCommand>
{
    public RejectApplicationCommandValidator()
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