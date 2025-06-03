using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public class UpdateRequisitesHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateRequisitesCommand> _validator;
    private readonly ILogger<UpdateRequisitesHandler> _logger;

    public UpdateRequisitesHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateRequisitesCommand> validator,
        ILogger<UpdateRequisitesHandler> logger)
    {
        _logger = logger;
        _validator = validator;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateRequisitesCommand command,
        CancellationToken cancellationToken = default)
    {
        var resultValidation = await _validator.ValidateAsync(command,cancellationToken);
        if (resultValidation.IsValid == false)
            return resultValidation.ToErrorList();
        
        var volunteerResult=await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
         return  volunteerResult.Error.ToErrorList();
        
        var newRequisitesList = new List<Requisites>();
        
        foreach (var (title, description) in command.Requisites)
        {
            var requisites = Requisites.Create(title, description).Value;
            newRequisitesList.Add(requisites);
        }
        
        var addRequisitesResult = volunteerResult.Value.UpdateRequisites(newRequisitesList);
        if (addRequisitesResult.IsFailure)
            return addRequisitesResult.Error.ToErrorList();

        await _volunteersRepository.Save(volunteerResult.Value,cancellationToken);
       
        _logger.LogInformation("Updated volunteer requisites with id:{id}", command.VolunteerId);

        return command.VolunteerId;
    }
}