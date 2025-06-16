namespace PRA_project.DTOs
{
    public class UserDto
    {
            public string FirstName { get; set; } = null!;

            public string LastName { get; set; } = null!;

            public string EmailAddress { get; set; } = null!;

            public string PhoneNumber { get; set; } = null!;

            public string? StudyProgramme { get; set; }

            public string? Role { get; set; }

            public string? ProfilePictureUrl { get; set; }
        }
}
