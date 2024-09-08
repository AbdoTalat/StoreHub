using System.ComponentModel.DataAnnotations;

namespace StoreHub.Models.DTOs
{
    public class UserRegistrationDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
