using core_application.Abstractions;
using core_infrastructure;
using core_infrastructure.DependencyManagements;
using domain.Abstractions;
using infrastructure.Persistence;
using infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddAsyncIntegrationStyleDependency(options =>
            {
                options.MessageServiceType = IntegrationStyleDependency.MessageServiceType.Consumer;
                options.BrokerAddress = config.GetValue<string>("Kafka:BootstrapServers");
                options.ConsumerGroupId = config.GetValue<string>("Kafka:ConsumerGroupId");
            });

            services.AddCoreInfrastructure<StockDbContext>(x =>
            {
                x.ConnectionString = config.GetConnectionString("StockDb");
            });

            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IDomainEventToMessageMapper, DomainEventToMessageMapper>();

            return services;
        }
    }
}
