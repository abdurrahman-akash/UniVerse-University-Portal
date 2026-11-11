using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace SchoolManagementSystem.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IStudentService _service;
        private readonly ApplicationDbContext _db;

        public StudentsController(IStudentService service, ApplicationDbContext db)
        {
            _service = service;
            _db = db;
        }

        // GET: /Students
        //public async Task<IActionResult> Index(string? search, int? departmentId)
        //{
        //    ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name", departmentId);
        //    ViewBag.Search = search;
        //    return View(await _service.GetAllAsync(search, departmentId));
        //}

        // GET: /Students?page=2&search=Abdur&departmentId=1
        public async Task<IActionResult> Index(string? search, int? departmentId, int page = 1)
        {
            if (page < 1) page = 1;
            const int pageSize = 10;

            ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name", departmentId);
            ViewBag.Search = search;
            ViewBag.DepartmentId = departmentId;

            var result = await _service.GetPagedAsync(search, departmentId, page, pageSize);
            return View(result);
        }


        // GET: /Students/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var student = await _service.GetByIdAsync(id);
            return student == null ? NotFound() : View(student);
        }

        // GET: /Students/Create
        // Show the Create form with the next StudentId already filled in
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name");

            var student = new Student { DepartmentId = 1, Batch = "2024" };
            student.StudentId = await GenerateStudentIdAsync(student.DepartmentId, student.Batch);

            return View(student);
        }


        // POST: /Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            // Auto-generate StudentId, it does not come from the form
            if (student.DepartmentId > 0)
            {
                student.StudentId = await GenerateStudentIdAsync(student.DepartmentId, student.Batch);
                ModelState.Remove("StudentId");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name", student.DepartmentId);
                return View(student);
            }
            var result = await _service.CreateAsync(student);
            if (!result.Success)
            {
                ModelState.AddModelError("StudentId", result.Message);
                ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name", student.DepartmentId);
                return View(student);
            }
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // Returns the next StudentId for the selected department and batch
        [HttpGet]
        public async Task<IActionResult> GetNextStudentId(int departmentId, string batch)
        {
            if (departmentId <= 0 || string.IsNullOrWhiteSpace(batch))
                return Json(new { studentId = "" });

            var studentId = await GenerateStudentIdAsync(departmentId, batch);
            return Json(new { studentId });
        }


        private async Task<string> GenerateStudentIdAsync(int departmentId, string batch)
        {
            var dept = await _db.Departments.FindAsync(departmentId);
            var code = dept?.Code ?? "GEN";

            var year = string.IsNullOrWhiteSpace(batch)
                ? DateTime.Now.Year.ToString()
                : batch;

            var prefix = $"{code}-{year}-";

            // Generates a StudentId based on department code and batch
            var last = await _db.Students
                .Where(s => s.StudentId.StartsWith(prefix))
                .OrderByDescending(s => s.StudentId)
                .FirstOrDefaultAsync();

            int next = 1;
            if (last != null)
            {
                var parts = last.StudentId.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int n))
                    next = n + 1;
            }

            return $"{prefix}{next:000}";
        }

        // GET: /Students/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _service.GetByIdAsync(id);
            if (student == null) return NotFound();
            ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name", student.DepartmentId);
            return View(student);
        }

        // POST: /Students/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name", student.DepartmentId);
                return View(student);
            }
            var result = await _service.UpdateAsync(student);
            if (!result.Success)
            {
                ModelState.AddModelError("StudentId", result.Message);
                ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name", student.DepartmentId);
                return View(student);
            }
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: /Students/Delete/1
        // Show the delete confirmation page
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _service.GetByIdAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // Delete the student after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Student deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
