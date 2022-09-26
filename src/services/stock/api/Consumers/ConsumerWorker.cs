using application;
using core_application.Abstractions;
using core_messages;
using MediatR;
using System.Text.Json;

namespace api.Consumers
{
    public class ConsumerWorker : BackgroundService
    {
        private readonly IEventListener _eventListener;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider; 
        public ConsumerWorker(IEventListener eventListener,
                              IConfiguration configuration, 
                              IServiceProvider serviceProvider)
        {
            this._eventListener = eventListener;
            this._configuration = configuration; 
            this._serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this._eventListener.ConsumeEvent(this._configuration.GetValue<string>("Kafka:ConsumeTopic:FromOrderService"), async (message) =>
            {
                var messageBase = JsonSerializer.Deserialize<MessageBase>(message);

                if (messageBase.EventType == typeof(IE_OrderPlaced).FullName)
                {
                    var orderPlaced = JsonSerializer.Deserialize<IE_OrderPlaced>(message);

                    using(var scope = this._serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetService<IMediator>();

                        await mediator.Send(new DecreaseStock.Command
                        {
                            OrderNo = orderPlaced.OrderNo,
                            Items = orderPlaced.Items.Select(item => new domain.Dtos.OrderItem
                            {
                                ProductId = item.ProductId,
                                Quantity = item.Quantity
                            }).ToList()
                        });
                    } 
                }
            }, stoppingToken);
        }

        public class MessageBase
        {
            public string EventType { get; set; }
        }
    }
}
