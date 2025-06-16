using PRA_project.Controllers;
using PRA_project.Models;

namespace PRA_project.DTOs
{
    public class TransactionGetDto
    {
        public string TypeName { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public int TransactionTypeId { get; set; }
    }
}
