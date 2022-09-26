using core_domain.Enums;

namespace core_domain.Abstractions
{
    public class DomainEventBase
    {
        public DomainEventState State { get; set; }
        public DateTime CreatedOn { get; private set; }
        public DomainEventBase()
        {
            this.CreatedOn = DateTime.Now;
        }
    }
}
