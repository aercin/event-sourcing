using application.Notifications;
using core_application.Abstractions;
using domain.Entities;
using MediatR;

namespace application
{
    public static class ProjectionOfSuccessedOrderToOrderStatistic
    {
        public class NotificationHandler : INotificationHandler<OrderSuccessedNotification>
        {
            private readonly IMongoRepository<OrderStatistic> _mongoRepository;
            public NotificationHandler(IMongoRepository<OrderStatistic> mongoRepository)
            {
                this._mongoRepository = mongoRepository;
            }

            public async Task Handle(OrderSuccessedNotification request, CancellationToken cancellationToken)
            {
                var orderStatisticRecord = await this._mongoRepository.FindOneAsync(x => true);
                if (orderStatisticRecord == null)
                {//Henüz sipariş istatistik kaydı hiç yok.
                    var newOrderStatistic = new OrderStatistic
                    {
                        TotalFailedOrderCount = 0,
                        TotalSuccessedOrderCount = 1,
                        TotalOrderCount = 1
                    };

                    await this._mongoRepository.InsertOneAsync(newOrderStatistic);
                }
                else
                {
                    orderStatisticRecord.TotalSuccessedOrderCount = orderStatisticRecord.TotalSuccessedOrderCount + 1;
                    orderStatisticRecord.TotalOrderCount = orderStatisticRecord.TotalOrderCount + 1;

                    await this._mongoRepository.ReplaceOneAsync(orderStatisticRecord);
                }
            }
        }
    }
}