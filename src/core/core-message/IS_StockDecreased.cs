using core_domain.Abstractions;

namespace core_messages
{
    public class IS_StockDecreased : IntegrationEventBase
    {
        public Guid OrderNo { get; set; }
    }
}
