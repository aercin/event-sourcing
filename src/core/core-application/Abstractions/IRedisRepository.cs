using core_domain.Abstractions;

namespace core_application.Abstractions
{
    public interface IRedisRepository<T> where T : class, IAggregateRoot
    {
        public Task<T> GetAsync<T>(string key);
        public Task SetAsync<T>(string key, T item); 
        public Task RemoveAsync(string key);
        public Task<bool> IsKeyExistAsync(string key);
    }
}
