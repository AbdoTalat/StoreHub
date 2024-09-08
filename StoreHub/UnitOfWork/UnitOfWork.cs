using Microsoft.EntityFrameworkCore.Storage;
using StoreHub.Core.SharedRepository;
using StoreHub.Data.DbContext;

namespace StoreHub.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreHubContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        //private IDbContextTransaction _transaction;
        //private bool IsDispose = false;
        public UnitOfWork(StoreHubContext context)
        {
            _context = context;
        }
        public void Dispose() { }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if(_repositories.ContainsKey(typeof(T)))
            {
                return _repositories[typeof(T)] as IGenericRepository<T>;
            }
            var repository = new GenericRepository<T>(_context);
            _repositories[typeof (T)] = repository;
            return repository;
        }

        //public async Task<IDbContextTransaction> BeginTransactionAsync()
        //{
        //    _transaction = await _context.Database.BeginTransactionAsync();
        //    return _transaction;
        //}

        //public async Task CommitTransactionAsync()
        //{
        //    await _transaction.CommitAsync();
        //}

        //public async Task RollBackTransactionAsync()
        //{
        //    await _transaction.RollbackAsync();
        //}
    }
}
