namespace PRA_project.DTOs
{
    public class RequestTransferMailDto
    {
        public string UserReceiverEmail { get; set; }

        public string UserSenderEmail { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}
