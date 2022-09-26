using core_application.Abstractions;
using core_domain.Abstractions;
using core_messages;
using domain.Events;

namespace infrastructure.Services
{
    public class DomainEventToMessageMapper : IDomainEventToMessageMapper
    {
        public IntegrationEventBase GetIntegrationEvent(DomainEventBase domainEvent)
        {
            IntegrationEventBase resIntegrationEventBase = null;

            if (domainEvent is StockDecreasedEvent stockDecreasedEvent)
            {
                resIntegrationEventBase = new IS_StockDecreased
                {
                    OrderNo = stockDecreasedEvent.OrderNo,
                    EventType = typeof(IS_StockDecreased).FullName
                };
            }
            else if (domainEvent is StockDecreaseFailedEvent stockDecreaseFailedEvent)
            {
                resIntegrationEventBase = new IE_StockDecreaseFailed
                {
                    OrderNo = stockDecreaseFailedEvent.OrderNo,
                    EventType = typeof(IE_StockDecreaseFailed).FullName
                };
            }

            return resIntegrationEventBase;
        }
    }
}
