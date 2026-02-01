namespace Apo.Messaging.RabbitMq
{
    public class RabbitMqOptions
    {
        public string ConnectionString { get; set; } = "amqp://guest:guest@localhost:5672/";
        public string DefaultExchange { get; set; } = "apo.default.exchange";
        public string DefaultRoutingKey { get; set; } = "apo.default.route";
    }
}
