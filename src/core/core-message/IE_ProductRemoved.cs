using core_domain.Abstractions;

namespace core_messages
{
    public class IE_ProductRemoved : IntegrationEventBase
    {
        public Guid OrderNo { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
