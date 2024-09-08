using Microsoft.AspNetCore.Identity;

namespace StoreHub.Models.Entities
{

    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }

}
