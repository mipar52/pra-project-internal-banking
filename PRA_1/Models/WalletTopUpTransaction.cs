namespace PRA_1.Models
{
    public class WalletTopUpTransaction : Transaction
    {
        public int CreditCardId { get; set; }
        public virtual CreditCard CreditCard { get; set; } = null; 
    }
}
