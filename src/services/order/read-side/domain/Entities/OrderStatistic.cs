using core_domain.Entities;

namespace domain.Entities
{
    public class OrderStatistic : MongoBaseEntity
    { 
        public int TotalOrderCount { get; set; }
        public int TotalSuccessedOrderCount { get; set; }
        public int TotalFailedOrderCount { get; set; }
    }
}
