using System.Text;
using Apo.Messaging.Testcontainers;
using RabbitMQ.Client;

namespace Apo.Messaging.Tests
{
    public class TopicRoutingIntegrationTests : IClassFixture<RabbitMqFixture>
    {
        private readonly RabbitMqFixture _fixture;

        public TopicRoutingIntegrationTests(RabbitMqFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task TopicExchange_Should_Route_Message_To_Matching_Queues_Only()
        {
            // Arrange
            var connectionString = _fixture.Container.GetConnectionString();

            var factory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            var exchange = "orders.exchange";
            var queueMatch = "orders.queue.match";
            var queueNoMatch = "orders.queue.nomatch";

            // Declare topic exchange
            await channel.ExchangeDeclareAsync(
                exchange: exchange,
                type: "topic",
                durable: true
            );

            // Declare queues
            await channel.QueueDeclareAsync(queueMatch, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueDeclareAsync(queueNoMatch, durable: true, exclusive: false, autoDelete: false);

            // Bind queues
            await channel.QueueBindAsync(queueMatch, exchange, routingKey: "order.*");
            await channel.QueueBindAsync(queueNoMatch, exchange, routingKey: "payment.*");

            var message = "Order Created!";
            var body = Encoding.UTF8.GetBytes(message);

            // Act — publish with routing key "order.created"
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: "order.created",
                mandatory: false,
                basicProperties: new BasicProperties { Persistent = true },
                body: body
            );

            // Assert — matching queue should receive the message
            var resultMatch = await channel.BasicGetAsync(queueMatch, autoAck: true);
            Assert.NotNull(resultMatch);

            var bodyMatch = Encoding.UTF8.GetString(resultMatch.Body.ToArray());
            Assert.Equal(message, bodyMatch);

            // Assert — non-matching queue should NOT receive anything
            var resultNoMatch = await channel.BasicGetAsync(queueNoMatch, autoAck: true);
            Assert.Null(resultNoMatch);
        }

        [Fact]
        public async Task TopicExchange_Should_Route_Message_To_Two_Queues_And_Skip_One()
        {
            // Arrange
            var connectionString = _fixture.Container.GetConnectionString();

            var factory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            var exchange = "orders.exchange";
            var queueA = "orders.queue.A";
            var queueB = "orders.queue.B";
            var queueC = "orders.queue.C";

            // Declare topic exchange
            await channel.ExchangeDeclareAsync(
                exchange: exchange,
                type: "topic",
                durable: true
            );

            // Declare queues
            await channel.QueueDeclareAsync(queueA, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueDeclareAsync(queueB, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueDeclareAsync(queueC, durable: true, exclusive: false, autoDelete: false);

            // Bind queues
            await channel.QueueBindAsync(queueA, exchange, routingKey: "order.*");
            await channel.QueueBindAsync(queueB, exchange, routingKey: "order.created");
            await channel.QueueBindAsync(queueC, exchange, routingKey: "payment.*");

            var message = "Order Created!";
            var body = Encoding.UTF8.GetBytes(message);

            // Act — publish with routing key "order.created"
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: "order.created",
                mandatory: false,
                basicProperties: new BasicProperties { Persistent = true },
                body: body
            );

            // Assert — Queue A should receive the message
            var resultA = await channel.BasicGetAsync(queueA, autoAck: true);
            Assert.NotNull(resultA);
            Assert.Equal(message, Encoding.UTF8.GetString(resultA.Body.ToArray()));

            // Assert — Queue B should receive the message
            var resultB = await channel.BasicGetAsync(queueB, autoAck: true);
            Assert.NotNull(resultB);
            Assert.Equal(message, Encoding.UTF8.GetString(resultB.Body.ToArray()));

            // Assert — Queue C should NOT receive the message
            var resultC = await channel.BasicGetAsync(queueC, autoAck: true);
            Assert.Null(resultC);
        }
    }

}
