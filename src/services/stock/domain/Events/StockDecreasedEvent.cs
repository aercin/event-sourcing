using core_domain.Abstractions;

namespace domain.Events
{
    public class StockDecreasedEvent : DomainEventBase
    {
        public Guid OrderNo { get; set; }
    }
}
