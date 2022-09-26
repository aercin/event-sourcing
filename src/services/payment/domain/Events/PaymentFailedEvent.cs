using core_domain.Abstractions;

namespace domain.Events
{
    public class PaymentFailedEvent : DomainEventBase
    {
        public Guid OrderNo { get; set; }
    }
}
