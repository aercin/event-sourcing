using core_domain.Abstractions;
using domain.Entities;

namespace domain.Events
{
    public class OrderPlaced : DomainEventBase
    {
        public Guid OrderNo { get; set; }
        public Guid CustomerId { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
