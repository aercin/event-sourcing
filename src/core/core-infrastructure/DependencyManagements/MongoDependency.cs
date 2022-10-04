using core_application.Abstractions;
using core_infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace core_infrastructure.DependencyManagements
{
    public static class MongoDependency
    {
        public static IServiceCollection AddMongoDependency(this IServiceCollection services, Action<Options> options, out string connStr)
        {
            var dependencyOptions = new Options();
            options(dependencyOptions);

            string mongoDbConnStr = string.Empty;
            if (!string.IsNullOrEmpty(dependencyOptions.UserName) && !string.IsNullOrEmpty(dependencyOptions.Password))
            {
                mongoDbConnStr = $"mongodb://{dependencyOptions.UserName}:{dependencyOptions.Password}@{dependencyOptions.Host}:{dependencyOptions.Port}";
            }
            else
            {
                mongoDbConnStr = $"mongodb://{dependencyOptions.Host}:{dependencyOptions.Port}";
            }

            connStr = mongoDbConnStr;

            services.AddSingleton<IMongoClient>(client => new MongoClient(mongoDbConnStr));
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.Configure<MongoDbSettings>(x =>
            {
                x.DatabaseName = dependencyOptions.DatabaseName;
                x.CollectionName = dependencyOptions.CollectionName;
            });

            return services;
        }

        public sealed class Options
        {
            public string Host { get; set; }
            public string Port { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string DatabaseName { get; set; }
            public string CollectionName { get; set; }
        }
    }
}
