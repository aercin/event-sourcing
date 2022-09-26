using core_domain.Entities;
using System.Linq.Expressions;

namespace core_application.Abstractions
{
    public interface IMongoRepository<TDocument> where TDocument : MongoBaseEntity
    {
        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> predicate);
        Task<IEnumerable<TDocument>> FilterByAsyc(Expression<Func<TDocument, bool>> predicate);
        Task InsertOneAsync(TDocument item);
        Task ReplaceOneAsync(TDocument item);
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> predicate);
    }
}
