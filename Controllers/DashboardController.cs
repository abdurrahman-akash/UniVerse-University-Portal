using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models.ViewModels;
using static SchoolManagementSystem.Models.ViewModels.DashboardViewModel;

namespace SchoolManagementSystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashboardViewModel
            {
                // Real counts from the database
                TotalStudents = await _db.Students.CountAsync(),
                TotalFaculty = await _db.Faculties.CountAsync(),
                TotalDepartments = await _db.Departments.CountAsync(),

                // No Course or Fee tables yet - these stay 0 until those modules are built
                TotalCourses = 0,
                FeesCollected = 0,
                FeesDue = 0,

                // Real recent students - newest admissions first
                RecentStudents = await _db.Students
                    .Include(s => s.Department)
                    .OrderByDescending(s => s.AdmissionDate)
                    .Take(5)
                    .ToListAsync(),

                // Real notices - newest first
                RecentNotices = await _db.Notices
                    .OrderByDescending(n => n.PublishDate)
                    .Take(5)
                    .ToListAsync(),

                // Real students-per-department data for the chart
                StudentsByDepartment = await _db.Students
                    .GroupBy(s => s.Department!.Code)
                    .Select(g => new DepartmentStat { Code = g.Key, Count = g.Count() })
                    .ToListAsync()
            };

            return View(vm);
        }
    }
}
