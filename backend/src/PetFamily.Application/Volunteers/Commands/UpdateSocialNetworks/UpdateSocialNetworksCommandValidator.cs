using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Application.Volunteers.Commands.UpdateSocialNetworks;

public class UpdateSocialNetworksCommandValidator:AbstractValidator<UpdateSocialNetworksCommand>
{
    public UpdateSocialNetworksCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(c => c.SocialNetworks)
            .MustBeValueObject(r => SocialNetwork.Create(r.Url, r.Name));
    }
}