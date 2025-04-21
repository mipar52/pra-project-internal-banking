using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class CarRegistrationUpdateDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public int IDCarRegistration {  get; set; }

        public string? CarBrand { get; set; }

        public string? CarModel { get; set; }

        public string RegistrationCountry { get; set; } = null!;

        public string RegistrationNumber { get; set; } = null!;
    }
}
