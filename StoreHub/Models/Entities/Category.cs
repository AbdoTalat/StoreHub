using System.ComponentModel.DataAnnotations;

namespace StoreHub.Models.Entities
{
    public class Category
    {
        public int ID { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(4)]
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
