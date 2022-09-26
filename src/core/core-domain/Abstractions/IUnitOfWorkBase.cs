namespace core_domain.Abstractions
{
    public interface IUnitOfWorkBase : IDisposable
    {
        Task CompleteAsync();
        IOutboxMessageRepository OutboxMessages { get; }
    }
}
