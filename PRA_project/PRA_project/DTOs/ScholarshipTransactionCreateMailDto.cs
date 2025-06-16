namespace PRA_project.DTOs
{
    public class ScholarshipTransactionCreateMailDto
    {
        public string UserEmail { get; set; }

        public int TransactionTypeId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
