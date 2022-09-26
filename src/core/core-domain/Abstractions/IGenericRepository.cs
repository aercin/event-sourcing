using System.Linq.Expressions;

namespace core_domain.Abstractions
{
    public interface IGenericRepository<T> where T : class, IAggregateRoot
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void Remove(T entity);  
    }
}
