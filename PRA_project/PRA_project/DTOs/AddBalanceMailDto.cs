namespace PRA_project.DTOs
{
    public class AddBalanceMailDto
    {
        public string EmailUser { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public int TransactionTypeId { get; set; }
    }
}
