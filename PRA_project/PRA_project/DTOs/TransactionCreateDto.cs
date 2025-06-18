using PRA_project.Controllers;

namespace PRA_project.DTOs
{
    public class TransactionCreateDto
    {
        public int UserId { get; set; }

        public string UserEmail { get; set; }
        public int TransactionTypeId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
