using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class UserFriendCreateDto
    {
        [Required]
        public int IdUser { get; set; }

        [Required]
        public string FriendEmail { get; set; }

    }
}
