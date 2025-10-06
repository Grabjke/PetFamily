using CSharpFunctionalExtensions;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Contracts;

public interface IAccountContract
{
    Task<HashSet<string>> GetUserPermissionsCode(Guid userId);

    Task<UnitResult<Error>> BannedUserApplication(
        BannedUserApplicationRequest request,
        CancellationToken cancellationToken);

    Task<UnitResult<Error>> CanSubmitApplication(
        CanSubmitApplicationRequest request,
        CancellationToken cancellationToken);
    Task<UnitResult<Error>> CreateVolunteer(CreateVolunteerRequest request, CancellationToken cancellationToken);
}