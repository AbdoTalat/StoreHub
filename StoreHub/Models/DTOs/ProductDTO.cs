using System.ComponentModel.DataAnnotations;

namespace StoreHub.Models.DTOs
{
    public class ProductDTO
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(4)]
        public string Description { get; set; }

        [Required]
        [Range(10, int.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public IFormFile? Image { get; set; } = null;

        [Required]
        public int CategoryId { get; set; }
    }
}
