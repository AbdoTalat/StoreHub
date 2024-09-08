using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreHub.Models.Entities;

namespace StoreHub.Data.DbContext
{
    public class StoreHubContext : IdentityDbContext<ApplicationUser>
    {
        public StoreHubContext() { }
        public StoreHubContext(DbContextOptions options) : base(options) { }


        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        //public virtual DbSet<Cart> Carts { get; set; }
        //public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
