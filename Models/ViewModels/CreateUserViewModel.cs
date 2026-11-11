using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models.ViewModels
{
    // Admin creates a new user and assigns a role - no public registration
    public class CreateUserViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = AppRoles.Student;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
