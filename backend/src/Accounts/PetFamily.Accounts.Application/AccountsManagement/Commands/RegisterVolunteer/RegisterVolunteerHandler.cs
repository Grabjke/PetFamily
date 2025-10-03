using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterUser;
using PetFamily.Accounts.Domain;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterVolunteer;

public class RegisterVolunteerHandler : ICommandHandler<RegisterVolunteerCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterVolunteerHandler(
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

    public async Task<UnitResult<ErrorList>> Handle(RegisterVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
        try
        {
            var user = await _userManager.FindByIdAsync(command.userId.ToString());
            if (user is null)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                _logger.LogWarning("User with id: {id} not found", command.userId);
                return Errors.General.AllReadyExist().ToErrorList();
            }

            var volunteerRole = await _roleManager.FindByNameAsync(VolunteerAccount.RoleName)
                                ?? throw new ApplicationException("Volunteer role doesn't exist");

            user.CreateVolunteerAccount(volunteerRole);

            var createResult = await _userManager.UpdateAsync(user);
            if (!createResult.Succeeded)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                var errors = createResult.Errors
                    .Select(e => Error.Failure(e.Code, e.Description))
                    .ToList();
                _logger.LogError("Volunteer creation failed: {Errors}", errors);
                return new ErrorList(errors);
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Volunteer id: {id} create successfully", command.userId);
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error during volunteer creation");
            return Errors.General.ServerError().ToErrorList();
        }
    }
}