using application.Abstractions;
using domain.Abstractions;
using MediatR;

namespace application
{
    public static class OrderFailed
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
            private readonly IOrderActivityManagement _orderActivityManagement;
            private readonly IOrderAggregateProjection _orderAggregateProjection;
            public CommandHandler(IOrderActivityManagement orderActivityManagement, IOrderAggregateProjection orderAggregateProjection)
            {
                this._orderActivityManagement = orderActivityManagement;
                this._orderAggregateProjection = orderAggregateProjection;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var orderAggregate = this._orderActivityManagement.ReproduceOrderAggregate(request.OrderNo, this._orderAggregateProjection);

                orderAggregate.MarkAsFailed();

                await this._orderActivityManagement.PersistOrderActivity(orderAggregate);

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
