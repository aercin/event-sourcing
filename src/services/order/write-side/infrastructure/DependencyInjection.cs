using application.Abstractions;
using core_infrastructure;
using core_infrastructure.DependencyManagements;
using domain.Abstractions;
using infrastructure.persistence;
using infrastructure.Persistence;
using infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IOrderActivityManagement, OrderActivityManagement>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderActivityRepository, OrderActivityRepository>();
            services.AddSingleton<IOrderAggregateProjection, OrderAggregateProjection>();

            services.AddAsyncIntegrationStyleDependency(options =>
            {
                options.MessageServiceType = IntegrationStyleDependency.MessageServiceType.Consumer;
                options.BrokerAddress = config.GetValue<string>("Kafka:BootstrapServers");
                options.ConsumerGroupId = config.GetValue<string>("Kafka:ConsumerGroupId");
            });

            services.AddCoreInfrastructure<OrderDbContext>(options =>
            {
                options.ConnectionString = config.GetConnectionString("OrderDb");
            }); 

            return services;
        }
    }
}
