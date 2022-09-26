namespace core_domain.Abstractions
{
    public abstract class AggregateRootBase : IAggregateRoot
    {
        private List<DomainEventBase> domainEvents;

        public AggregateRootBase()
        {
            domainEvents = new List<DomainEventBase>();
        }

        public IReadOnlyCollection<DomainEventBase> DomainEvents
        {
            get
            {
                return domainEvents.AsReadOnly();
            }
        }

        public virtual void AddDomainEvent(DomainEventBase domainEvent)
        {
            domainEvents.Add(domainEvent);
        }
    }
}
