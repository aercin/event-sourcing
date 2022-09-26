using core_infrastructure.persistence;
using domain.Abstractions;
using infrastructure.persistence;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.Persistence
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        public UnitOfWork(OrderDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IOrderActivityRepository OrderActivities
        {
            get
            {
                return this._serviceProvider.GetRequiredService<IOrderActivityRepository>();
            }
        }
    }
}
