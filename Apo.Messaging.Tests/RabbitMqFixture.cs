using Testcontainers.RabbitMq;
using Xunit;

namespace Apo.Messaging.Testcontainers
{
    public class RabbitMqFixture : IAsyncLifetime
    {
        public RabbitMqContainer Container { get; private set; }

        public RabbitMqFixture()
        {
            Container = new RabbitMqBuilder()
                .WithImage("rabbitmq:3.12-management")
                .WithUsername("guest")
                .WithPassword("guest")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await Container.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await Container.StopAsync();
        }
    }

}
