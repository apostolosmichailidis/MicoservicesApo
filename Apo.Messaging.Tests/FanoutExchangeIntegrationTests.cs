using System.Text;
using Apo.Messaging.Testcontainers;
using RabbitMQ.Client;

namespace Apo.Messaging.Tests
{
    public class FanoutExchangeIntegrationTests : IClassFixture<RabbitMqFixture>
    {
        private readonly RabbitMqFixture _fixture;

        public FanoutExchangeIntegrationTests(RabbitMqFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task FanoutExchange_Should_Deliver_Message_To_All_Bound_Queues()
        {
            // Arrange
            var connectionString = _fixture.Container.GetConnectionString();

            var factory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            var exchange = "fanout.test.exchange";
            var queue1 = "fanout.queue.1";
            var queue2 = "fanout.queue.2";

            // Declare fanout exchange
            await channel.ExchangeDeclareAsync(
                exchange: exchange,
                type: "fanout",
                durable: true
            );

            // Declare queues
            await channel.QueueDeclareAsync(queue1, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueDeclareAsync(queue2, durable: true, exclusive: false, autoDelete: false);

            // Bind queues to fanout exchange
            await channel.QueueBindAsync(queue1, exchange, routingKey: "");
            await channel.QueueBindAsync(queue2, exchange, routingKey: "");

            var message = "Hello Fanout!";
            var body = Encoding.UTF8.GetBytes(message);

            // Act — publish to fanout exchange
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: "",
                mandatory: false,
                basicProperties: new BasicProperties { Persistent = true },
                body: body
            );

            // Assert — both queues must receive the message
            var result1 = await channel.BasicGetAsync(queue1, autoAck: true);
            var result2 = await channel.BasicGetAsync(queue2, autoAck: true);

            Assert.NotNull(result1);
            Assert.NotNull(result2);

            var body1 = Encoding.UTF8.GetString(result1.Body.ToArray());
            var body2 = Encoding.UTF8.GetString(result2.Body.ToArray());

            Assert.Equal(message, body1);
            Assert.Equal(message, body2);
        }
    }
}
