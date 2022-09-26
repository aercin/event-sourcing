using core_domain.Abstractions;

namespace domain.Events
{
    public class PaymentSuccessedEvent : DomainEventBase
    {
        public Guid OrderNo { get; set; }
    }
}
