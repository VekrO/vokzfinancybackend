using System.Linq.Expressions;

namespace VokzFinancy.Repository {

    public interface IRepository<T> {

        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);

    }

}