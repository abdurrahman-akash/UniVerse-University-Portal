using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class Faculty
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Designation { get; set; } = "Lecturer";

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime JoiningDate { get; set; } = DateTime.Today;

        public string FullName => $"{FirstName} {LastName}";
    }
}
