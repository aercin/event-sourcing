using application.Abstractions;
using core_domain.Abstractions;
using core_domain.Entitites;
using core_domain.Enums;
using domain.Abstractions;
using domain.Entities;
using System.Text.Json;

namespace infrastructure.Services
{
    internal class OrderActivityManagement : IOrderActivityManagement
    {
        private readonly IUnitOfWork _uow;
        public OrderActivityManagement(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task PersistOrderActivity(OrderAggregate aggregate)
        {  
            foreach (var domainEvent in aggregate.DomainEvents.Where(de => de.State == DomainEventState.Added))
            {
                domainEvent.State = DomainEventState.Unchanged;

                this._uow.OrderActivities.Add(OrderActivity.Create(aggregate.AggregateId, domainEvent.GetType().AssemblyQualifiedName, JsonSerializer.Serialize(domainEvent, domainEvent.GetType())));
            }

            foreach (var integrationEvent in aggregate.IntegrationEvents)
            {
                this._uow.OutboxMessages.Add(OutboxMessage.CreateOutboxMessage(integrationEvent.GetType().AssemblyQualifiedName, JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType()), DateTime.Now));
            }

            await this._uow.CompleteAsync();
        }

        public OrderAggregate ReproduceOrderAggregate(Guid orderNo, IOrderAggregateProjection orderAggregateProjection)
        {
            var orderAggregate = new OrderAggregate(orderAggregateProjection);

            var orderActivities = this._uow.OrderActivities.Find(x => x.AggregateId == orderNo).OrderBy(x => x.CreatedOn).ToList();

            foreach (var orderActivity in orderActivities)
            {
                var domainEvent = (DomainEventBase)JsonSerializer.Deserialize(orderActivity.EventPayload, Type.GetType(orderActivity.EventType));

                orderAggregate.AddDomainEvent(domainEvent);
            }

            return orderAggregate;
        }
    }
}
