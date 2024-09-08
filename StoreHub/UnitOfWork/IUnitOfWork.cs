using StoreHub.Core.SharedRepository;

namespace StoreHub.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        //public Task<IDbContextTransaction> BeginTransactionAsync();
        //public Task CommitTransactionAsync();
        //public Task RollBackTransactionAsync();
    }
}
