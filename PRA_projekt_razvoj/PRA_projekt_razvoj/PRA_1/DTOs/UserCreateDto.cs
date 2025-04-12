using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class UserCreateDto
    {
        public int IDUser {  get; set; }
      
        [Required(ErrorMessage = "Firstname is required!")]
        [StringLength(256)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Lastname is required!")]
        [StringLength(256)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(256, MinimumLength = 6, ErrorMessage = "Username needs to contain at least six charcaters.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 6, ErrorMessage = "Password needs to contain at least six charcaters.")]
        public string UserPassword { get; set; } = null!;

        public string? TestPassword { get; set; } = null;

        [Required(ErrorMessage = "Email address is required!")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;
    }
}
