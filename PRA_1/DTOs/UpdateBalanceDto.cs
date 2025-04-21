using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class UpdateBalanceDto
    {
        [Required]
        public int IdUser { get; set; }

        [Required]
        public string CreditCardFirstName { get; set; }

        [Required]
        public string CreditCardLastName { get; set; }

        [Required]
        public DateTime CreditCardExpiryDate { get; set; }

        [Required]
        public string CreditCardCvv { get; set; }

        [Required]
        public string CreditCardNumber { get; set; }

        [Required]
        public decimal Balance { get; set; }

    }
}
