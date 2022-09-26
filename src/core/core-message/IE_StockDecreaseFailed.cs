using core_domain.Abstractions;

namespace core_messages
{
    public class IE_StockDecreaseFailed : IntegrationEventBase
    {
        public Guid OrderNo { get; set; }
    }
}
