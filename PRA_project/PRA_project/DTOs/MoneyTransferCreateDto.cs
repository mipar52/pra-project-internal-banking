namespace PRA_project.DTOs
{
    public class MoneyTransferCreateDto
    {

        public int? TransactionId { get; set; }

        public int UserRecieverId { get; set; }

        public int UserSenderId { get; set; }

        public int TransactionTypeId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
