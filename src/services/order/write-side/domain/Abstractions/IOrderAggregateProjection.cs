using core_domain.Abstractions;
using domain.Entities;

namespace domain.Abstractions
{
    public interface IOrderAggregateProjection
    {
        void ProjectEventToAggregate(DomainEventBase domainEvent, OrderAggregate aggregate);
    }
}
