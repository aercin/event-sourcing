using core_application.Abstractions;
using core_domain.Entitites;
using Dapper;

namespace MessageRelayService
{
    public class OrderWorker : BackgroundService
    {
        private readonly ILogger<OrderWorker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IEventDispatcher _eventDispatcher;
        public OrderWorker(ILogger<OrderWorker> logger,
                           IConfiguration configuration,
                           IEventDispatcher eventDispatcher,
                           IEnumerable<IDbConnectionFactory> dbConnectionFactories)
        {
            _logger = logger;
            _dbConnectionFactory = dbConnectionFactories.Single(x => x.Context == "order");
            _eventDispatcher = eventDispatcher;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(15));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Order Message Relay Service is running at: {time}", DateTime.Now);

                using (var connection = this._dbConnectionFactory.GetOpenConnection())
                {
                    string sql = $@"     SELECT
                                          ""Id"",
                                          ""Type"",
                                          ""Message"",
                                          ""CreatedOn""
                                     FROM public.""OutboxMessages"" 
                                ";

                    var messages = await connection.QueryAsync<OutboxMessage>(sql);

                    foreach (var relatedOutBoxMessage in messages)
                    {
                        try
                        { 
                            await this._eventDispatcher.DispatchEvent(_configuration.GetValue<string>("Kafka:PublishTopic:ToOrderService"), relatedOutBoxMessage.Message);

                            await connection.ExecuteAsync(@"DELETE FROM public.""OutboxMessages"" WHERE ""Id""=@Id", new { Id = relatedOutBoxMessage.Id });
                        }
                        catch (Exception ex)
                        {
                            this._logger.LogError(ex, $"{relatedOutBoxMessage.Id} idli mesaj event bus gönderiminde hata ile karþýlaþýldý");
                        }
                    }
                }
            }
        }
    }
}