namespace PRA_project.DTOs
{
    public class CreditCardCreateMailDto
    {
        public string UserMail { get; set; }
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string CreditCardNumber { get; set; } = null!;

        public DateTime ExpiryDate { get; set; }

        public string Cvv { get; set; } = null!;
    }
}
