namespace core_domain.Abstractions
{
    public abstract class ESAggregateRootBase : AggregateRootBase
    {
        private List<IntegrationEventBase> integrationEvents;
        public ESAggregateRootBase()
        {
            integrationEvents = new List<IntegrationEventBase>();
        }

        public IReadOnlyCollection<IntegrationEventBase> IntegrationEvents
        {
            get
            {
                return integrationEvents.AsReadOnly();
            }
        }

        public void AddIntegrationEvent(IntegrationEventBase integrationEvent)
        {
            integrationEvent.EventType = integrationEvent.GetType().FullName;
            integrationEvent.CreatedOn = DateTime.Now;
            integrationEvents.Add(integrationEvent);
        } 
    }
}
