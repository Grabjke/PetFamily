using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterUser;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserHandler(
        UserManager<User> userManager,
        ILogger<RegisterUserHandler> logger,
        RoleManager<Role> roleManager,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
        try
        {
            var userExists = await _userManager.FindByEmailAsync(command.Email);
            if (userExists is not null)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                _logger.LogWarning("User with email {Email} already exists", command.Email);
                return Errors.General.AllReadyExist().ToErrorList();
            }
            
            var participantRole = await _roleManager.FindByNameAsync(ParticipantAccount.RoleName)
                            ?? throw new ApplicationException("Participant role doesn't exist");

            var user = User.CreateParticipantAccount(command.UserName, command.Email, participantRole);
            
            var createResult = await _userManager.CreateAsync(user, command.Password);
            if (!createResult.Succeeded)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                var errors = createResult.Errors
                    .Select(e => Error.Failure(e.Code, e.Description))
                    .ToList();
                _logger.LogError("User creation failed: {Errors}", errors);
                return new ErrorList(errors);
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("User {UserName} registered successfully", command.UserName);
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error during user registration");
            return Errors.General.ServerError().ToErrorList();
        }
    }
}