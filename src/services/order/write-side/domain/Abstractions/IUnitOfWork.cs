using core_domain.Abstractions;

namespace domain.Abstractions
{
    public interface IUnitOfWork : IUnitOfWorkBase
    {
        IOrderActivityRepository OrderActivities { get; }
    }
}
