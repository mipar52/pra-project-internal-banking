namespace PRA_project.DTOs
{
    public class RequestTransferDto
    {
        public int UserRecieverId { get; set; }

        public int UserSenderId { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}
