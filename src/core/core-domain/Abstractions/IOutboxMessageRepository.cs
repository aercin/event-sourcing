using core_domain.Entitites;

namespace core_domain.Abstractions
{
    public interface IOutboxMessageRepository : IGenericRepository<OutboxMessage>
    {
    }
}
