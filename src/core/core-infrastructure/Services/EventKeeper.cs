using core_application.Abstractions;
using core_domain.Abstractions;
using core_domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace core_infrastructure.Services
{
    public class EventKeeper<T> : IEventKeeper where T : DbContext
    {
        private readonly T _context;
        private readonly ISystemClock _systemClock;
        private readonly IServiceProvider _serviceProvider;

        public EventKeeper(T context, ISystemClock systemClock, IServiceProvider serviceProvider)
        {
            _context = context;
            _systemClock = systemClock;
            _serviceProvider = serviceProvider;
        }

        public async Task StoreEventsAsync()
        {
            var domainEntities = this._context.ChangeTracker.Entries<AggregateRootBase>()
                                              .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();

            var tasks = domainEvents.Select(async (domainEvent) =>
            {
                var domainEventToMessageMapper = this._serviceProvider.GetRequiredService<IDomainEventToMessageMapper>();
                var integrationEvent = domainEventToMessageMapper.GetIntegrationEvent(domainEvent);

                var integrationEventMessage = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType());
                  
                await _context.Set<OutboxMessage>().AddAsync(OutboxMessage.CreateOutboxMessage(integrationEvent.GetType().AssemblyQualifiedName, integrationEventMessage, _systemClock.Current));
            });

            await Task.WhenAll(tasks);
        }
    }
}
