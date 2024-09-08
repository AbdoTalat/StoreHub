using StoreHub.Core.SharedRepository;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Repository.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        public Task AddPaymentAsync(Payment payment);
    }
}
