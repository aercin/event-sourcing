using core_domain.Abstractions;
using domain.Entities;

namespace domain.Events
{
    public class ProductAdded : DomainEventBase
    {  
        public OrderProduct AddedProduct { get; set; }
    }
}
