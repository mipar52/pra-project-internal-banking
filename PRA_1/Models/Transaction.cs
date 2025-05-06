namespace PRA_1.Models
{
    public abstract class Transaction
    {
        public int IdTransaction { get; set; }
        public int UserId { get; set; } //FK na usera
        public int TransactionTypeId{ get; set; } //FK na TransType
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        
        public virtual User User { get; set; }
        public virtual TransactionType TransactionType { get; set; }
}
