using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class FoodPaymentCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public decimal Amount { get; set; }

    }
}
