using domain.Abstractions;
using domain.Entities;

namespace application.Abstractions
{
    public interface IOrderActivityManagement
    {
        Task PersistOrderActivity(OrderAggregate aggregate);
        OrderAggregate ReproduceOrderAggregate(Guid orderNo,IOrderAggregateProjection orderAggregateProjection);
    }
}
