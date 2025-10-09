using MassTransit;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterVolunteer;
using PetFamily.VolunteersApplications.Contracts.Messaging;

namespace PetFamily.Accounts.Infrastructure.Consumers;

public class RegisterVolunteerConsumer : IConsumer<ApprovedApplicationEvent>
{
    private readonly RegisterVolunteerHandler _handler;

    public RegisterVolunteerConsumer(
        RegisterVolunteerHandler handler)
    {
        _handler = handler;
    }

    public async Task Consume(ConsumeContext<ApprovedApplicationEvent> context)
    {
        await _handler.Handle(new RegisterVolunteerCommand(context.Message.UserId));
    }
}