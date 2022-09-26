using core_domain.Abstractions;
using core_message.Common;

namespace core_messages
{
    public class IE_ProductAdded : IntegrationEventBase
    { 
        public Guid OrderNo { get; set; }

        public OrderItem Item { get; set; }
    }
}
