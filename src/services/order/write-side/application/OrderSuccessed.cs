using application.Abstractions;
using domain.Abstractions;
using MediatR;

namespace application
{
    public static class OrderSuccessed
    {
        #region Command
        public class Command : IRequest<Response>
        {
            public Guid OrderNo { get; set; }
        }
        #endregion

        #region Command Handler
        public class CommandHandler : IRequestHandler<Command, Response>
        {
            private readonly IOrderActivityManagement _orderActivitiyManagement;
            private readonly IOrderAggregateProjection _orderAggregateProjection;
            public CommandHandler(IOrderActivityManagement orderActivitiyManagement, IOrderAggregateProjection orderAggregateProjection)
            {
                _orderActivitiyManagement = orderActivitiyManagement;
                _orderAggregateProjection = orderAggregateProjection;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var orderAggregate = this._orderActivitiyManagement.ReproduceOrderAggregate(request.OrderNo, this._orderAggregateProjection);

                orderAggregate.MarkAsSuccessed();

                await this._orderActivitiyManagement.PersistOrderActivity(orderAggregate);

                return new Response
                {
                    IsSuccess = true
                };
            }
        }
        #endregion

        #region Response
        public class Response
        {
            public bool IsSuccess { get; set; }
        }
        #endregion
    }
}
