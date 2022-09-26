using core_application.Abstractions;
using core_infrastructure.DependencyManagements;
using core_infrastructure.Persistence;
using MessageRelayService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddAsyncIntegrationStyleDependency(options =>
        {
            options.MessageServiceType = IntegrationStyleDependency.MessageServiceType.Producer;
            options.BrokerAddress = hostContext.Configuration.GetValue<string>("Kafka:BootstrapServers");
        });
        services.AddSingleton<IDbConnectionFactory, PostgreDbConnectionFactory>((serviceProvider) =>
        {
            return new PostgreDbConnectionFactory("order", hostContext.Configuration.GetConnectionString("OrderDb"));
        });
        services.AddSingleton<IDbConnectionFactory, PostgreDbConnectionFactory>((serviceProvider) =>
        {
            return new PostgreDbConnectionFactory("stock", hostContext.Configuration.GetConnectionString("StockDb"));
        });
        services.AddSingleton<IDbConnectionFactory, PostgreDbConnectionFactory>((serviceProvider) =>
        {
            return new PostgreDbConnectionFactory("payment", hostContext.Configuration.GetConnectionString("PaymentDb"));
        });
        services.AddHostedService<OrderWorker>();
        services.AddHostedService<StockWorker>();
        services.AddHostedService<PaymentWorker>();
    })
    .Build();

await host.RunAsync();
