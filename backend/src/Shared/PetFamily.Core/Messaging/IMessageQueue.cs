namespace PetFamily.Core.Messaging;

public interface IMessageQueue<TMessage>
{  
    Task WriteAsync(TMessage paths, CancellationToken token = default);
    Task<TMessage> ReadAsync(CancellationToken token = default);
}