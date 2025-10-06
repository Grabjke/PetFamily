using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterVolunteer;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Presentation;

public class AccountsContract : IAccountContract
{
    private readonly PermissionManager _permissionManager;
    private readonly AccountDbContext _context;
    private readonly RegisterVolunteerHandler _registerVolunteerHandler;

    public AccountsContract(
        PermissionManager permissionManager,
        AccountDbContext context,
        RegisterVolunteerHandler registerVolunteerHandler)
    {
        _permissionManager = permissionManager;
        _context = context;
        _registerVolunteerHandler = registerVolunteerHandler;
    }

    public async Task<HashSet<string>> GetUserPermissionsCode(Guid userId)
    {
        return await _permissionManager.GetUserPermissionsCode(userId);
    }

    public async Task<UnitResult<Error>> BannedUserApplication(
        BannedUserApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user is null)
            return Errors.General.NotFound();

        user.BannedApplicationUntil = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success<Error>();
    }

    public async Task<UnitResult<Error>> CanSubmitApplication(
        CanSubmitApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            return Errors.General.NotFound();

        if (user.BannedApplicationUntil > DateTime.UtcNow)
            return Errors.User.VolunteerApplicationNotAllowed();

        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> CreateVolunteer(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var registerResult = await _registerVolunteerHandler.Handle(
            new RegisterVolunteerCommand(request.UserId), cancellationToken);

        if (registerResult.IsSuccess == false)
            Errors.User.FailedToCreateVolunteer();

        return UnitResult.Success<Error>();
    }
}