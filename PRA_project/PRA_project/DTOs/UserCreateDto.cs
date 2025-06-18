using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class UserCreateDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public int? StudyProgramId { get; set; }

        public string Password { get; set; } = null!;

        public int? RoleId { get; set; }

        public string? ProfilePictureUrl { get; set; }
    }
}
