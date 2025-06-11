using PRA_project.Controllers;

namespace PRA_project.DTOs
{
    public class ParkingTransactionCreateDto
    {
        public int UserId { get; set; }

        public int TransactionTypeId { get; set; }

        public DateTime Date { get; set; }

        public string RegistrationNumber { get; set; } = null!;

        public string RegistrationCountryCode { get; set; } = null!;

        public int DurationHours { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

    }
}