using Confluent.Kafka;
using core_application.Abstractions;

namespace core_infrastructure.Services
{
    public class EventListener : IEventListener
    {
        private IConsumer<Null, string> _consumer;
        public EventListener(IConsumer<Null, string> consumer)
        {
            this._consumer = consumer;
        }

        public async Task ConsumeEvent<T>(T topic, Func<string, Task> callback, CancellationToken cancellationToken) where T : class
        {
            this._consumer.Subscribe(topic.ToString());

            var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(10));

            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                var response = this._consumer.Consume();

                await callback(response.Message.Value);
            }
        } 
    }
}
