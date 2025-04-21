using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class ParkingPaymentCreateDto
    {
        [Required]
        public int IdUser{get; set; }

        [Required]
        public string RegistrationCountryCode { get; set; } = null!;

        [Required]
        public string RegistrationNumber { get; set; } = null!;

        [Required]
        public int DurationHours { get; set; }

        public DateTime StartTime { get; set; }
    }
}
