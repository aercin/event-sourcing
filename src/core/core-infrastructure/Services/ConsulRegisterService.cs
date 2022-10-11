using Consul;
using core_infrastructure.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace core_infrastructure.Services
{
    public class ConsulRegisterService : IHostedService
    {
        private readonly IConsulClient _consulClient;
        private readonly ConsulConfig _consulConfig;

        public ConsulRegisterService(IConsulClient consulClient, IOptions<ConsulConfig> consulConfig)
        {
            this._consulClient = consulClient;
            this._consulConfig = consulConfig.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this._consulClient.Agent.ServiceDeregister(this._consulConfig.ServiceId, cancellationToken);

            var serviceAddressUri = new Uri(this._consulConfig.ServiceAddress);
            var serviceRegistration = new AgentServiceRegistration
            {
                ID = this._consulConfig.ServiceId,
                Name = this._consulConfig.ServiceName,
                Address = serviceAddressUri.Host,
                Port = serviceAddressUri.Port,
                Check = new AgentServiceCheck
                {
                    Name = $"{this._consulConfig.ServiceName}-check",
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Status = HealthStatus.Passing,
                    HTTP = $"http://host.docker.internal:{serviceAddressUri.Port}/health",
                    Method = "GET",
                    Interval = TimeSpan.FromSeconds(10),
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            await this._consulClient.Agent.ServiceRegister(serviceRegistration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await this._consulClient.Agent.ServiceDeregister(this._consulConfig.ServiceId, cancellationToken);
        }
    }
}
