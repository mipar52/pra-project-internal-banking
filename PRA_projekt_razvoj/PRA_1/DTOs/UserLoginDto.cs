using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string UserPassword { get; set; } = null!;

        [Required(ErrorMessage = "Required to choose between Email or Phone authentication")]
        public string CodeSenderOption { get; set; } = null!;



    }
}
