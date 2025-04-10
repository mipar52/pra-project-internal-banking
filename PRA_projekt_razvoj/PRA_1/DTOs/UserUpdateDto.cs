using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class UserUpdateDto
    {
        
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [StringLength(256, MinimumLength = 6, ErrorMessage = "Username needs to contain at least six characters")]
        public string? UserName { get; set; }

        [StringLength(256, MinimumLength = 6, ErrorMessage = "Password needs to contain at least six characters")]
        public string? Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
