using core_application.Abstractions;
using core_domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace core_infrastructure.Persistence
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : MongoBaseEntity
    {
        private readonly IMongoCollection<TDocument> _collection;
        public MongoRepository(IMongoClient client, IOptions<MongoDbSettings> settings)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            this._collection = database.GetCollection<TDocument>(settings.Value.CollectionName);
        }

        public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> predicate)
        {
            return (await this._collection.FindAsync(predicate)).FirstOrDefault();
        }

        public async Task<IEnumerable<TDocument>> FilterByAsyc(Expression<Func<TDocument, bool>> predicate)
        {
            return (await this._collection.FindAsync(predicate)).ToEnumerable();
        }

        public async Task InsertOneAsync(TDocument item)
        {
            await this._collection.InsertOneAsync(item);
        }

        public async Task ReplaceOneAsync(TDocument item)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, item.Id);
            await _collection.FindOneAndReplaceAsync(filter, item);
        }

        public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> predicate)
        {
            await _collection.FindOneAndDeleteAsync(predicate);
        }
    }
}
