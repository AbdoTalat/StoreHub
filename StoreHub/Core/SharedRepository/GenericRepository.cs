using Microsoft.EntityFrameworkCore;
using StoreHub.Data.DbContext;
using System.Linq.Expressions;

namespace StoreHub.Core.SharedRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly StoreHubContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(StoreHubContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAllAsQueryable(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
                query = query.Where(filter);

            return query;
        }
        public async Task<IEnumerable<T>> GetAllAsEnumerableAsync()
            => await _dbSet.ToListAsync();
        public IQueryable<T> GetAllWithIncludeAsQueryable(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }
            if (filter != null)
                query = query.Where(filter);
            return query.AsQueryable();
        }
        public async Task<IEnumerable<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetSingleWithIncludeAsync( Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>> includeProperties)
        {
            IQueryable<T> query = _dbSet;

            query = includeProperties(query);

            return await query.FirstOrDefaultAsync(predicate);
        }
        public async Task<T?> GetByIdAsync(int? Id)
            => await _dbSet.FindAsync(Id);


        public async Task<T?> AddNewAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            var entity = await GetByIdAsync(Id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
