namespace PRA_project.DTOs
{
    public class FriendGetDto
    {
        public string FirstName { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? ProfilePictureUrl { get; set; }

        public string LastName { get; set; } = null!;
    }
}
