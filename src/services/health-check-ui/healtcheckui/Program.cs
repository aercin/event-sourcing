var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecksUI(setup =>
{
    setup.AddWebhookNotification("webhook1",
    uri: "http://localhost:5242/api/notification",
    payload: "{\r\n  \"message\": \"Webhook report for [[LIVENESS]]: [[FAILURE]] - Description: [[DESCRIPTIONS]]\"\r\n}",
    restorePayload: "{\r\n  \"message\": \"[[LIVENESS]] is back to life\"\r\n}");
    setup.SetMinimumSecondsBetweenFailureNotifications(10);
}).AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecksUI(setup =>
{
    setup.AddCustomStylesheet("healthcheck.css");
});

app.Run();
