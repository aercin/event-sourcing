using core_domain.Abstractions;

namespace domain.Events
{
    public class ProductRemoved : DomainEventBase
    { 
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
