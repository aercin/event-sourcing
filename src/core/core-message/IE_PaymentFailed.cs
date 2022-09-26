using core_domain.Abstractions;

namespace core_messages
{
    public class IE_PaymentFailed : IntegrationEventBase
    {
        public Guid OrderNo { get; set; }
    }
}
