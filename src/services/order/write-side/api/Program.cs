using api.Consumers;
using application;
using HealthChecks.UI.Client;
using infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<ConsumerWorker>();
builder.Services.AddHealthChecks()
                 .AddNpgSql(builder.Configuration.GetConnectionString("OrderDb"), name: "postgre")
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
