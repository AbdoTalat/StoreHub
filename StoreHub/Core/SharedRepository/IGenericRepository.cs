using System.Linq.Expressions;

namespace StoreHub.Core.SharedRepository
{
    public interface IGenericRepository<T> where T : class
    {
        public IQueryable<T> GetAllAsQueryable(Expression<Func<T, bool>>? filter = null);
        public Task<IEnumerable<T>> GetAllAsEnumerableAsync();
        public Task<IEnumerable<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties);
        public IQueryable<T> GetAllWithIncludeAsQueryable(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includeProperties);
        public Task<T?> GetSingleWihtIncludeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        public Task<T?> GetByIdAsync(int? Id);
        public Task<T?> AddNewAsync(T entity);
        public Task<bool> UpdateAsync(T entity);
        public Task<bool> DeleteAsync(int Id);
    }
}
