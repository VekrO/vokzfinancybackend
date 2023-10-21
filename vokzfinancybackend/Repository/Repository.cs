
using System.Linq.Expressions;
using Azure;
using Microsoft.EntityFrameworkCore;
using VokzFinancy.Data;
using vokzfinancybackend.Repository.Interfaces;

namespace VokzFinancy.Repository
{

    public class Repository<T> : IRepository<T> where T : class {

        private readonly BancoContext _context;
        public Repository(BancoContext context) {
            _context = context;
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate) {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate) {
            return await _context.Set<T>().SingleOrDefaultAsync(predicate);
        }
        public async Task<T> GetByIdIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {

            IQueryable<T> query = _context.Set<T>();

            IQueryable<T> result = includes.Aggregate(query, (current, include) => current.Include(include));

            if(predicate != null)
            {
                result = result.Where(predicate);
            }

            return await result.FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<T>> GetAllAsync() {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task Add(T entity) {
           _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }   

        public async Task Update(T entity) {
            _context.Set<T>().Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
        
        public async Task Delete(T entity) {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

    }

}