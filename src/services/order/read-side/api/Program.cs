using api.Projections;
using application;
using core_infrastructure.DependencyManagements;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddAsyncIntegrationStyleDependency(options =>
{
    options.MessageServiceType = IntegrationStyleDependency.MessageServiceType.Consumer;
    options.BrokerAddress = builder.Configuration.GetValue<string>("Kafka:BootstrapServers");
    options.ConsumerGroupId = builder.Configuration.GetValue<string>("Kafka:ConsumerGroupId");
});
builder.Services.AddStackExchangeRedisDependency(options =>
{
    options.Endpoints = builder.Configuration.GetValue<string>("Redis:Endpoints");
    options.Password = builder.Configuration.GetValue<string>("Redis:Password");
    options.Database = builder.Configuration.GetValue<int>("Redis:Database");
}, out string redisConnStr);
builder.Services.AddMongoDependency(options =>
{
    options.Host = builder.Configuration.GetValue<string>("Mongo:Host");
    options.Port = builder.Configuration.GetValue<string>("Mongo:Port");
    options.UserName = builder.Configuration.GetValue<string>("Mongo:UserName");
    options.Password = builder.Configuration.GetValue<string>("Mongo:Password");
    options.DatabaseName = builder.Configuration.GetValue<string>("Mongo:DatabaseName");
    options.CollectionName = builder.Configuration.GetValue<string>("Mongo:CollectionName");
}, out string mongoConnStr);
builder.Services.AddHostedService<ProjectionWorker>();

builder.Services.AddHealthChecks()
                .AddRedis(redisConnStr, name: "redis")
                .AddMongoDb(mongoConnStr, name: "mongo")
                .AddKafka(setup =>
                {
                    setup.BootstrapServers = builder.Configuration.GetValue<string>("Kafka:BootstrapServers");
                    setup.MessageTimeoutMs = 5000;
                }, name: "kafka");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = reg => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
