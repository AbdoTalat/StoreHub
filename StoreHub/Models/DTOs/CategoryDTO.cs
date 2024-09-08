using System.ComponentModel.DataAnnotations;

namespace StoreHub.Models.DTOs
{
    public class CategoryDTO
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(4)]
        public string Description { get; set; }
    }
}
