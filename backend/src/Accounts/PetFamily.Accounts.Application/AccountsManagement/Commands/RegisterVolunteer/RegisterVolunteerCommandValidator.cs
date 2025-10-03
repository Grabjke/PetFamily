using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterVolunteer;

public class RegisterVolunteerCommandValidator : AbstractValidator<RegisterVolunteerCommand>
{
    public RegisterVolunteerCommandValidator()
    {
        RuleFor(x => x.userId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}