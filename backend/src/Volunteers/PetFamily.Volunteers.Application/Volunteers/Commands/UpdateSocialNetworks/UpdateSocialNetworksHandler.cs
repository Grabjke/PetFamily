using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Volunteer;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialNetworks;

public class UpdateSocialNetworksHandler : ICommandHandler<Guid, UpdateSocialNetworksCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateSocialNetworksCommand> _validator;
    private readonly ILogger<UpdateSocialNetworksHandler> _logger;

    public UpdateSocialNetworksHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateSocialNetworksCommand> validator,
        ILogger<UpdateSocialNetworksHandler> logger)
    {
        _logger = logger;
        _validator = validator;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSocialNetworksCommand command,
        CancellationToken cancellationToken)
    {
        var resultValidation = await _validator.ValidateAsync(command, cancellationToken);
        if (resultValidation.IsValid == false)
            return resultValidation.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var newSocialNetworks = new List<SocialNetwork>();

        foreach (var (url, name) in command.SocialNetworks)
        {
            var socialNetwork = SocialNetwork.Create(url, name).Value;
            newSocialNetworks.Add(socialNetwork);
        }

        var addSocialNetworksResult = volunteerResult.Value.UpdateSocialNetworks(newSocialNetworks);
        if (addSocialNetworksResult.IsFailure)
            return addSocialNetworksResult.Error.ToErrorList();

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Updated volunteer social networks with id:{id}", command.VolunteerId);

        return command.VolunteerId;
    }
}