using Apo.Messaging.Abstractions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Apo.Messaging.RabbitMq
{
    public class RabbitMqMessagePublisher : IMessagePublisher
    {
        private readonly RabbitMqOptions _options;

        public RabbitMqMessagePublisher(RabbitMqOptions options)
        {
            _options = options;
        }

        public async Task PublishAsync(
            object message,
            string? exchange = null,
            string? routingKey = null,
            Dictionary<string, object>? headers = null)
        {
            exchange ??= _options.DefaultExchange;
            routingKey ??= _options.DefaultRoutingKey;
            headers ??= new Dictionary<string, object>();

            var factory = new ConnectionFactory
            {
                Uri = new Uri(_options.ConnectionString)
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // 1. BasicReturn handler
            channel.BasicReturnAsync += async (sender, args) => 
            { 
                var returnedBody = Encoding.UTF8.GetString(args.Body.ToArray()); 
                Console.WriteLine("❗ Returned message from RabbitMQ");
                Console.WriteLine($"Exchange: {args.Exchange}"); 
                Console.WriteLine($"Routing Key: {args.RoutingKey}");
                Console.WriteLine($"Reply Code: {args.ReplyCode}"); 
                Console.WriteLine($"Reply Text: {args.ReplyText}"); 
                Console.WriteLine($"Message Body: {returnedBody}");
                // TODO: Handle the returned message (e.g., log, retry, etc.)
            };


            // Create exchange and queue if they do not exist
            await channel.ExchangeDeclareAsync(
                exchange: exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null
            );

            var queueName = routingKey.Replace(".", "_");

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Bind queue → exchange → routingKey
            await channel.QueueBindAsync(
                queue: queueName,
                exchange: exchange,
                routingKey: routingKey
            );

            // Serialize message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);


            var props = new BasicProperties
            {
                Persistent = true,
                Headers = headers
            };

            // Publish
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: true, 
                basicProperties: props,
                body: body
            );
        }
    }
}
