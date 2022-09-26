using core_domain.Abstractions;

namespace domain.Events
{
    public class StockDecreaseFailedEvent : DomainEventBase
    {
        public Guid OrderNo { get; set; }
    }
}
