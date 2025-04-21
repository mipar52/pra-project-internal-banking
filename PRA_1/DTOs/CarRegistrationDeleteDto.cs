using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class CarRegistrationDeleteDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public int IDCarRegistration { get; set; }
    }
}
