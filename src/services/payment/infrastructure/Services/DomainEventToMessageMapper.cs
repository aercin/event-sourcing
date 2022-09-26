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

            if (domainEvent is PaymentSuccessedEvent paymentSuccessedEvent)
            {
                resIntegrationEventBase = new IE_PaymentSuccessed
                {
                    OrderNo = paymentSuccessedEvent.OrderNo,
                    EventType = typeof(IE_PaymentSuccessed).FullName
                };
            }
            else if (domainEvent is PaymentFailedEvent paymentFailedEvent)
            {
                resIntegrationEventBase = new IE_PaymentFailed
                {
                    OrderNo = paymentFailedEvent.OrderNo,
                    EventType = typeof(IE_PaymentFailed).FullName
                };
            }

            return resIntegrationEventBase;
        }
    }
}
