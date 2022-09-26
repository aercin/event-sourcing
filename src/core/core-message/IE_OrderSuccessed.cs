using core_domain.Abstractions;
using core_message.Common;

namespace core_messages
{
    public class IE_OrderSuccessed : IntegrationEventBase
    {
        public Guid OrderNo { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
