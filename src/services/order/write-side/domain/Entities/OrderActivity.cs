using core_domain.Abstractions;

namespace domain.Entities
{
    public class OrderActivity : IAggregateRoot
    {
        public int Id { get; private set; }
        public Guid AggregateId { get; private set; }
        public string EventType { get; private set; }
        public string EventPayload { get; private set; }
        public DateTime CreatedOn { get; private set; }

        private OrderActivity(Guid aggregateId, string eventType, string eventPayload)
        {
            this.AggregateId = aggregateId;
            this.EventType = eventType;
            this.EventPayload = eventPayload;
            this.CreatedOn = DateTime.Now;
        }

        public static OrderActivity Create(Guid aggregateId, string eventType, string eventPayload)
        {
            return new OrderActivity(aggregateId, eventType, eventPayload);
        }
    }
}
