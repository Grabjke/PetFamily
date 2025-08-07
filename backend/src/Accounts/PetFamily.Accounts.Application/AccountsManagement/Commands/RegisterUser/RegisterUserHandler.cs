using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterUser;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(
        UserManager<User> userManager,
        ILogger<RegisterUserHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var userExists = await _userManager.FindByEmailAsync(command.Email);
        if (userExists != null)
            return Errors.General.AllReadyExist().ToErrorList();

        var user = new User
        {
            Email = command.Email,
            UserName = command.UserName
        };
        var result = _userManager
            .CreateAsync(user, command.Password);

        if (result.Result.Succeeded)
        {
            _logger.LogInformation("Successfully registered user with name:{name}",command.UserName);
            return Result.Success<ErrorList>();
        }
        
        var errors = result.Result.Errors
            .Select(e => Error.Failure(e.Code, e.Description))
            .ToList();
        return new ErrorList(errors);

    }
}