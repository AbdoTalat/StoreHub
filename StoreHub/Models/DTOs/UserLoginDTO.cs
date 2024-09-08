using System.ComponentModel.DataAnnotations;

namespace StoreHub.Models.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
