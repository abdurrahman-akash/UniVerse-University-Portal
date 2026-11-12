using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    // Departments - only Admin
    [Authorize(Roles = AppRoles.Admin)]
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DepartmentsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Departments - real count from the database
        public async Task<IActionResult> Index()
        {
            // Include is the fix: without it d.Students is empty and Count is always 0
            var departments = await _db.Departments
                .Include(d => d.Students)
                .OrderBy(d => d.Name)
                .ToListAsync();
            return View(departments);
        }

        // GET: /Departments/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var department = await _db.Departments
                .Include(d => d.Students)
                .FirstOrDefaultAsync(d => d.Id == id);
            return department == null ? NotFound() : View(department);
        }

        // GET: /Departments/Create
        public IActionResult Create() => View(new Department());

        // POST: /Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid) return View(department);
            _db.Departments.Add(department);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Department added successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Departments/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _db.Departments.FindAsync(id);
            return department == null ? NotFound() : View(department);
        }

        // POST: /Departments/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id) return NotFound();
            if (!ModelState.IsValid) return View(department);
            _db.Departments.Update(department);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Department updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Departments/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _db.Departments
                .Include(d => d.Students)
                .FirstOrDefaultAsync(d => d.Id == id);
            return department == null ? NotFound() : View(department);
        }

        // POST: /Departments/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _db.Departments
                .Include(d => d.Students)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (department == null) return NotFound();
            if (department.Students.Any())
            {
                TempData["Error"] = "Cannot delete this department - it still has students. Move or delete the students first.";
                return RedirectToAction(nameof(Index));
            }
            _db.Departments.Remove(department);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Department deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
