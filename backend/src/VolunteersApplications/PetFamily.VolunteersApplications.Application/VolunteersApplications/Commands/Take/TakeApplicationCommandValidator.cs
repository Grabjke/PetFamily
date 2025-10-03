using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteersApplications.Application.VolunteersApplications.Commands.Take;

public class TakeApplicationCommandValidator : AbstractValidator<TakeApplicationCommand>
{
    public TakeApplicationCommandValidator()
    {
        RuleFor(a => a.AdminId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(a => a.ApplicationId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}