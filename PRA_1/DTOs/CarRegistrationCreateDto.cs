using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class CarRegistrationCreateDto
    {
        [Required]
        public string Email { get; set; }
        
        public string? CarBrand { get; set; }

        public string? CarModel { get; set; }

        [Required]
        public string RegistrationCountry { get; set; } = null!;

        [Required]
        public string RegistrationNumber { get; set; } = null!;

    }
}
