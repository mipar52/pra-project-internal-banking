using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string UserPassword { get; set; } = null!;
    }
}
