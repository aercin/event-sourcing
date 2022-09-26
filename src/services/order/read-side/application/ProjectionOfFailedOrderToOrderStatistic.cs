using application.Notifications;
using core_application.Abstractions;
using domain.Entities;
using MediatR;

namespace application
{
    public static class ProjectionOfFailedOrderToOrderStatistic
    {
        public class NotificationHandler : INotificationHandler<OrderFailedNotification>
        {
            private readonly IMongoRepository<OrderStatistic> _mongoRepository;
            public NotificationHandler(IMongoRepository<OrderStatistic> mongoRepository)
            {
                this._mongoRepository = mongoRepository;
            }

            public async Task Handle(OrderFailedNotification request, CancellationToken cancellationToken)
            {
                var orderStatisticRecord = await this._mongoRepository.FindOneAsync(x => true);
                if (orderStatisticRecord == null)
                {//Henüz sipariş istatistik kaydı hiç yok.
                    var newOrderStatistic = new OrderStatistic
                    {
                        TotalFailedOrderCount = 1,
                        TotalSuccessedOrderCount = 0,
                        TotalOrderCount = 1
                    };

                    await this._mongoRepository.InsertOneAsync(newOrderStatistic);
                }
                else
                {
                    orderStatisticRecord.TotalFailedOrderCount = orderStatisticRecord.TotalFailedOrderCount + 1;
                    orderStatisticRecord.TotalOrderCount = orderStatisticRecord.TotalOrderCount + 1;

                    await this._mongoRepository.ReplaceOneAsync(orderStatisticRecord);
                }
            }
        }
    }
}