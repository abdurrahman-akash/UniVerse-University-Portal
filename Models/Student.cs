using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string StudentId { get; set; } = string.Empty;

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        [Required]
        public string Program { get; set; } = "BSc in CSE";

        public int Semester { get; set; } = 1;
        public string? Batch { get; set; }

        public string Gender { get; set; } = "Male";
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        public string FullName => $"{FirstName} {LastName}";
    }
}
