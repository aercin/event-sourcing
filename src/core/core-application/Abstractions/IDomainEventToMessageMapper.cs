using core_domain.Abstractions;

namespace core_application.Abstractions
{
    public interface IDomainEventToMessageMapper
    {
        IntegrationEventBase GetIntegrationEvent(DomainEventBase domainEvent);
    }
}
