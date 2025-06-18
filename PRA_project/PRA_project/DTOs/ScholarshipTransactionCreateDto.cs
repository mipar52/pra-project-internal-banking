using PRA_project.Controllers;

namespace PRA_project.DTOs
{
    public class ScholarshipTransactionCreateDto
    {
        public int UserId { get; set; }

        public int TransactionTypeId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

    }
}
