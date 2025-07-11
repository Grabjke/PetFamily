namespace PetFamily.Application.Abstractions;

public interface IQueryHandler<TResponse,in TQuery> where TQuery : IQuery
{
    public Task<TResponse> Handle(
        TQuery command,
        CancellationToken cancellationToken = default);
}