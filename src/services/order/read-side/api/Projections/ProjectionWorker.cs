using application.Common;
using application.Notifications;
using Confluent.Kafka;
using core_application.Abstractions;
using core_messages;
using MediatR;
using System.Text.Json;

namespace api.Projections
{
    public class ProjectionWorker : BackgroundService
    {
        private readonly IEventListener _eventListener;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        public ProjectionWorker(IEventListener eventListener,
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

                if (messageBase.EventType == typeof(IE_OrderSuccessed).FullName)
                {
                    var orderSuccessed = JsonSerializer.Deserialize<IE_OrderSuccessed>(message);
                    using (var scope = this._serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetService<IMediator>();
                        await mediator.Publish(new OrderSuccessedNotification
                        {
                            OrderNo = orderSuccessed.OrderNo,
                            Items = orderSuccessed.Items.Select(item => new OrderItem
                            {
                                ProductId = item.ProductId,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice
                            }).ToList()
                        });
                    }
                }
                else if (messageBase.EventType == typeof(IE_OrderFailed).FullName)
                {
                    var orderFailed = JsonSerializer.Deserialize<IE_OrderFailed>(message);

                    using (var scope = this._serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetService<IMediator>();
                        await mediator.Publish(new OrderFailedNotification());
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
