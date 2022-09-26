using api.Projections;
using application;
using core_infrastructure.DependencyManagements;

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
});
builder.Services.AddMongoDependency(options =>
{
    options.Host = builder.Configuration.GetValue<string>("Mongo:Host");
    options.Port = builder.Configuration.GetValue<string>("Mongo:Port");
    options.UserName = builder.Configuration.GetValue<string>("Mongo:UserName");
    options.Password = builder.Configuration.GetValue<string>("Mongo:Password");
    options.DatabaseName = builder.Configuration.GetValue<string>("Mongo:DatabaseName");
    options.CollectionName = builder.Configuration.GetValue<string>("Mongo:CollectionName");
});
builder.Services.AddHostedService<ProjectionWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
