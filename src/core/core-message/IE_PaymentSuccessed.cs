using core_domain.Abstractions;

namespace core_messages
{
    public class IE_PaymentSuccessed : IntegrationEventBase
    {
        public Guid OrderNo { get; set; }
    }
}
