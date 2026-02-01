namespace Apo.Messaging.Abstractions
{
    public interface IMessagePublisher
    {
        Task PublishAsync(object message, 
            string? exchange = null, 
            string? routingKey = null, 
            Dictionary<string, object>? headers = null);
    }
}
