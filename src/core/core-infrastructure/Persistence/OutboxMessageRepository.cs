using core_domain.Abstractions;
using core_domain.Entitites;
using Microsoft.EntityFrameworkCore;

namespace core_infrastructure.persistence
{
    public class OutboxMessageRepository<T> : GenericRepository<OutboxMessage>, IOutboxMessageRepository where T : DbContext
    {
        public OutboxMessageRepository(T context) : base(context)
        {

        }
    }
}
