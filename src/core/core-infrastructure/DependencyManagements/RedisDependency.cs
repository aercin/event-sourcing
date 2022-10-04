using core_application.Abstractions;
using core_infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace core_infrastructure.DependencyManagements
{
    public static class RedisDependency
    {
        public static IServiceCollection AddStackExchangeRedisDependency(this IServiceCollection services, Action<Options> options, out string connStr)
        {
            var dependencyOptions = new Options();
            options(dependencyOptions);

            var redisConfigOption = ConfigurationOptions.Parse(dependencyOptions.Endpoints);
            redisConfigOption.Password = dependencyOptions.Password;
            redisConfigOption.DefaultDatabase = dependencyOptions.Database;

            connStr = redisConfigOption.ToString();

            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(redisConfigOption));
            services.AddScoped(typeof(IRedisRepository<>), typeof(RedisRepository<>));

            return services;
        }

        public sealed class Options
        {
            public string Endpoints { get; set; } //Host1:Port1,Host2:Port2 vb.
            public string Password { get; set; }
            public int Database { get; set; }
        }
    }
}
