using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class CreditCardCreateDto
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string CardNumber { get; set; } = null!;

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public string CvvHash { get; set; } = null!;

        public string CvvSalt { get; set; } = null!;
    }
}
