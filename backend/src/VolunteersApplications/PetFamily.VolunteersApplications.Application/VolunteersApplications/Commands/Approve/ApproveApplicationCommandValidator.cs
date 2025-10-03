using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Approve;

public class ApproveApplicationCommandValidator : AbstractValidator<ApproveApplicationCommand>
{
    public ApproveApplicationCommandValidator()
    {
        RuleFor(a => a.AdminId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(a => a.ApplicationId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}