using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class CanteenPaymentCreateDto
    {
        [Required]
        public int IdCanteenPayment { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public decimal Amount { get; set; }

    }
}
