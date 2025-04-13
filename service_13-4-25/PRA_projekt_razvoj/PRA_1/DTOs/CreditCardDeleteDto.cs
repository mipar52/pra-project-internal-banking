using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class CreditCardDeleteDto
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(16)]
        public string CardNumber { get; set; } = null!;
    }
}
