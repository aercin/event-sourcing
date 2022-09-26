namespace core_domain.Abstractions
{
    public class IntegrationEventBase
    {  
        public string EventType { get; set; }
        public DateTime CreatedOn { get; set; } 
    }
}
