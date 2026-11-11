using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;

        public string? HeadOfDepartment { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
