
using System.Linq.Expressions;
using Azure;
using Microsoft.EntityFrameworkCore;
using VokzFinancy.Data;
using VokzFinancy.Repository;

namespace VokzFinancy.Repository {

    public class Repository<T> : IRepository<T> where T : class {

        private readonly BancoContext _context;
        public Repository(BancoContext context) {
            _context = context;
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate) {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate) {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
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