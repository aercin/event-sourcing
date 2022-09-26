using Confluent.Kafka;
using core_application.Abstractions;

namespace core_infrastructure.Services
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IProducer<Null, string> _producer;
        public EventDispatcher(IProducer<Null, string> producer)
        {
            this._producer = producer;
        }

        public async Task DispatchEvent<T>(T topic, string integrationEvent) where T : class
        {
            await this._producer.ProduceAsync(topic.ToString(), new Message<Null, string>
            {
                Value = integrationEvent
            });
        }
    }
}
