namespace PRA_project.DTOs
{
    public class MoneyTransferCreateMailDto
    {
        public int? TransactionId { get; set; }

        public string UserRecieverEmail { get; set; }

        public string UserSenderEmail { get; set; }

        public int TransactionTypeId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
