using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalFaculty { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalCourses { get; set; }
        public decimal FeesCollected { get; set; }
        public decimal FeesDue { get; set; }
        public List<Student> RecentStudents { get; set; } = new();
        public List<Notice> RecentNotices { get; set; } = new();

        // Real students-per-department data for the doughnut chart
        public List<DepartmentStat> StudentsByDepartment { get; set; } = new();

        public class DepartmentStat
        {
            public string Code { get; set; } = string.Empty;
            public int Count { get; set; }
        }

    }
}
