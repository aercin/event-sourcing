using core_application.Abstractions;
using core_domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace core_infrastructure.persistence
{
    public class BaseUnitOfWork : IUnitOfWorkBase
    {
        private readonly DbContext _context;
        private readonly IEventKeeper _eventKeeper;
        private readonly IServiceProvider _serviceProvider;
        public BaseUnitOfWork(DbContext context, IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._context = context;
            this._eventKeeper = serviceProvider.GetRequiredService<IEventKeeper>();
        }

        public IOutboxMessageRepository OutboxMessages
        {
            get
            {
                return this._serviceProvider.GetRequiredService<IOutboxMessageRepository>();
            }
        }

        public async Task CompleteAsync()
        {
            await this._eventKeeper.StoreEventsAsync();

            await this._context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}
