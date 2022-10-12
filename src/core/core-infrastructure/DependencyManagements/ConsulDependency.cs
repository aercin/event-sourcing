using Consul;
using core_infrastructure.Models;
using core_infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace core_infrastructure.DependencyManagements
{
    public static class ConsulDependency
    {
        public static IServiceCollection AddConsulDependency(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ConsulConfig>(config.GetSection("Consul"));
            services.AddHostedService<ConsulRegisterService>();
            services.AddSingleton<IConsulClient, ConsulClient>(provider => new ConsulClient(option =>
            {
                option.Address = new Uri(config.GetValue<string>("Consul:Address"));
            }));
            return services;
        }
    }
}
