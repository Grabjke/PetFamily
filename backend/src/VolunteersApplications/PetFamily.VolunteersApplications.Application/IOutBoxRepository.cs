namespace PetFamily.VolunteersApplications.Application;

public interface IOutBoxRepository
{
    Task Add<T>(T message, CancellationToken cancellationToken);
}