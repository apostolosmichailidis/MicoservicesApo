using System.Text;
using Apo.Messaging.RabbitMq;
using Apo.Messaging.Testcontainers;
using RabbitMQ.Client;

namespace Apo.Messaging.Tests
{
    public class RabbitMqPublisherIntegrationTests : IClassFixture<RabbitMqFixture>
    {
        private readonly RabbitMqFixture _fixture;

        public RabbitMqPublisherIntegrationTests(RabbitMqFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task PublishAsync_Should_Deliver_Message_To_Queue()
        {
            // Arrange
            var options = new RabbitMqOptions
            {
                ConnectionString = _fixture.Container.GetConnectionString(),
                DefaultExchange = "test.exchange",
                DefaultRoutingKey = "test.key"
            };

            var publisher = new RabbitMqMessagePublisher(options);

            // Act
            await publisher.PublishAsync(
                new { Name = "IntegrationTest", Body = "This is a test from Apo" },
                exchange: "test.exchange",
                routingKey: "test.key"
            );

            // Assert: consume the message from RabbitMQ
            var factory = new ConnectionFactory
            {
                Uri = new Uri(options.ConnectionString)
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var queueName = "test_key"; // όπως το φτιάχνει ο publisher

            var result = await channel.BasicGetAsync(queueName, autoAck: true);

            Assert.NotNull(result);

            var body = Encoding.UTF8.GetString(result.Body.ToArray());
            Assert.Contains("IntegrationTest", body);
            Assert.Contains("This is a test from Apo", body);
        }
    }

}
