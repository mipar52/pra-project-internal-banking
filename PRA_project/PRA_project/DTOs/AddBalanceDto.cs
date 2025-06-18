namespace PRA_project.DTOs
{
    public class AddBalanceDto
    {
        public int IdUser { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public int TransactionTypeId { get; set; }
    }
}
