using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class Verify2FADto
    {
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Authentication code is required")]
        public string AuthCode { get; set; } = null!;

        public DateTime AuthCodeTime { get; set; }
    }
}
