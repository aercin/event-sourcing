using application.Abstractions;
using domain.Abstractions;
using domain.Dtos;
using domain.Entities;
using MediatR;

namespace application
{
    static public class PlaceOrder
    {
        #region Command
        public class Command : IRequest<Response>
        {
            public Guid CustomerId { get; set; }

            public List<OrderProductDto> Items { get; set; }
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
                var orderAggregate = OrderAggregate.PlaceOrder(request.CustomerId, request.Items, this._orderAggregateProjection);

                await this._orderActivityManagement.PersistOrderActivity(orderAggregate);

                return new Response
                {
                    IsSuccess = true,
                    OrderNo = orderAggregate.OrderNo
                };
            }
        }
        #endregion

        #region Response
        public class Response
        {
            public bool IsSuccess { get; set; }

            public Guid OrderNo { get; set; }
        }
        #endregion
    }
}
