namespace PRA_project.DTOs
{
    public class CreditCardGetDto
    {
        public string FirstName { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string CreditCardNumber { get; set; } = null!;

        public DateTime ExpiryDate { get; set; }
    }
}
