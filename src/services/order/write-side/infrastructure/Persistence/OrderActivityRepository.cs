using core_infrastructure.persistence;
using domain.Abstractions;
using domain.Entities;
using infrastructure.persistence;

namespace infrastructure.Persistence
{
    internal class OrderActivityRepository : GenericRepository<OrderActivity>, IOrderActivityRepository
    {
        public OrderActivityRepository(OrderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
