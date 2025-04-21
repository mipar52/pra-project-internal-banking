using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class SendRequestMoneyCreateDto
    {
        [Required]
        public int UserSenderId { get; set; }

        [Required]
        public int UserRecieverId { get; set; }

        [Required]
        public int TransactionTypeId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
