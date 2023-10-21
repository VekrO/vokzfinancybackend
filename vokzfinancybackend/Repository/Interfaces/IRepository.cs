using System.Linq.Expressions;

namespace vokzfinancybackend.Repository.Interfaces
{

    public interface IRepository<T>
    {

        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync();
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);

    }

}