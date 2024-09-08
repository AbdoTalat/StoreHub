using StoreHub.Core.SharedRepository;
using StoreHub.Core.Repository.Interfaces;
using StoreHub.Data.DbContext;
using StoreHub.Models.Entities;

namespace StoreHub.Core.Repository.Implementations
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly StoreHubContext context;

        public PaymentRepository(StoreHubContext context) : base(context)
        {
            this.context = context;
        }
        public async Task AddPaymentAsync(Payment payment)
        {
            try
            {
                await context.Payments.AddAsync(payment);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
