using core_application.Abstractions;
using domain.Entities;
using MediatR;

namespace application
{
    public static class ReportOrderStatistic
    {
        #region Command 
        public class Command : IRequest<OrderStatisticResponse>
        {

        }
        #endregion

        #region Command Handler
        public class CommandHandler : IRequestHandler<Command, OrderStatisticResponse>
        {
            private readonly IMongoRepository<OrderStatistic> _mongoRepository;
            public CommandHandler(IMongoRepository<OrderStatistic> mongoRepository)
            {
                this._mongoRepository = mongoRepository;

            }
            public async Task<OrderStatisticResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var storedOrderStatistic = await this._mongoRepository.FindOneAsync(x => true);
                if (storedOrderStatistic == null)
                    throw new ApplicationException("Herhangi bir sipariş istatistik kaydı sistemde bulunmamaktadır");

                return new OrderStatisticResponse
                {
                    TotalOrderCount = storedOrderStatistic.TotalOrderCount,
                    TotalFailedOrderCount = storedOrderStatistic.TotalFailedOrderCount,
                    TotalSuccessedOrderCount = storedOrderStatistic.TotalSuccessedOrderCount
                };
            }
        }
        #endregion

        #region Response
        public class OrderStatisticResponse
        {
            public int TotalOrderCount { get; set; }
            public int TotalSuccessedOrderCount { get; set; }
            public int TotalFailedOrderCount { get; set; }
        }
        #endregion
    }
}
