using core_application.Abstractions;
using core_domain.Abstractions;
using StackExchange.Redis;
using System.Text.Json;

namespace core_infrastructure.Persistence
{
    public class RedisRepository<T> : IRedisRepository<T> where T : class, IAggregateRoot
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            this._connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await this._connectionMultiplexer.GetDatabase().StringGetAsync(key);

            if (value.HasValue)
            {
                if (typeof(T).IsValueType)
                {
                    return (T)Convert.ChangeType(value.ToString(), typeof(T));
                }
                else
                {
                    return JsonSerializer.Deserialize<T>(value.ToString());
                }
            }

            return default(T);
        }

        public async Task SetAsync<T>(string key, T item)
        {
            string itemStringRepresentation;

            if (typeof(T).IsValueType)
            {
                itemStringRepresentation = item.ToString();
            }
            else
            {
                itemStringRepresentation = JsonSerializer.Serialize(item);
            }

            await this._connectionMultiplexer.GetDatabase().StringSetAsync(key, itemStringRepresentation);
        }

        public async Task RemoveAsync(string key)
        {
            await this._connectionMultiplexer.GetDatabase().KeyDeleteAsync(key);
        }

        public async Task<bool> IsKeyExistAsync(string key)
        {
            return await this._connectionMultiplexer.GetDatabase().KeyExistsAsync(key);
        }
    }
}
