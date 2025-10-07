using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterVolunteer;

public record RegisterVolunteerCommand(Guid userId) : ICommand;