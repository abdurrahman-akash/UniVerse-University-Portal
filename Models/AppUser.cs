using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Models
{
    // Application user - extends IdentityUser with name fields
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";
    }
}
