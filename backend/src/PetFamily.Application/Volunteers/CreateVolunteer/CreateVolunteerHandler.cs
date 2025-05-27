using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    
    public async Task<Result<Guid, Error>> Handle(
    CreateVolunteerRequest request, CancellationToken cancellationToken = default)
{
    var volunteerId = VolunteerId.NewVolunteerId();
    
    var fullNameResult = FullName.Create(request.Name, request.Surname, request.Patronymic);
    if (fullNameResult.IsFailure)
        return fullNameResult.Error;
    
    var emailResult = Email.Create(request.Email);
    if (emailResult.IsFailure)
        return emailResult.Error;
    
    var phoneNumberResult = OwnersPhoneNumber.Create(request.PhoneNumber);
    if (phoneNumberResult.IsFailure)
        return phoneNumberResult.Error;
    
    var volunteerByNameResult = await _volunteersRepository.GetByName(fullNameResult.Value.Name);
    if (volunteerByNameResult.IsSuccess)
        return Errors.Volunteer.AllReadyExist();
    
    var volunteerResult = Volunteer.Create(
        volunteerId,
        fullNameResult.Value,
        emailResult.Value,
        request.Description,
        request.Experience,
        phoneNumberResult.Value
    );
    
    if (volunteerResult.IsFailure)
        return volunteerResult.Error;

    var volunteer = volunteerResult.Value;
    
    if (request.SocialNetworks?.Any() == true)
    {
        foreach (var (url, name) in request.SocialNetworks)
        {
            var socialNetworkResult = SocialNetwork.Create(url, name);
            if (socialNetworkResult.IsFailure)
                return socialNetworkResult.Error;

            var addResult = volunteer.AddSocialNetwork(socialNetworkResult.Value);
            if (addResult.IsFailure)
                return addResult.Error;
        }
    }

    if (request.Requisites?.Any() == true)
    {
        foreach (var (title, description) in request.Requisites)
        {
            var requisitesResult = Requisites.Create(title, description);
            if (requisitesResult.IsFailure)
                return requisitesResult.Error;

            var addRequisitesResult = volunteer.AddRequisites(requisitesResult.Value);
            if (addRequisitesResult.IsFailure)
                return addRequisitesResult.Error;
        }
    }

    await _volunteersRepository.Add(volunteer, cancellationToken);
    
    return volunteer.Id.Value;
}
   
}