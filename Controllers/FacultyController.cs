using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Services;

namespace SchoolManagementSystem.Controllers
{
    // Faculty management - only Admin
    [Authorize(Roles = AppRoles.Admin)]
    public class FacultyController : Controller
    {
        private readonly IFacultyService _service;
        private readonly ApplicationDbContext _db;

        public FacultyController(IFacultyService service, ApplicationDbContext db)
        {
            _service = service;
            _db = db;
        }

        // GET: /Faculty
        //public async Task<IActionResult> Index(string? search, int? departmentId)
        //{
        //    ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name", departmentId);
        //    ViewBag.Search = search;
        //    return View(await _service.GetAllAsync(search, departmentId));
        //}

        // GET: /Faculty?page=2&search=Kamal&departmentId=1
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


        // GET: /Faculty/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var faculty = await _service.GetByIdAsync(id);
            return faculty == null ? NotFound() : View(faculty);
        }

        // GET: /Faculty/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name");
            return View(new Faculty());
        }

        // POST: /Faculty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Faculty faculty)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name", faculty.DepartmentId);
                return View(faculty);
            }
            var result = await _service.CreateAsync(faculty);
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: /Faculty/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var faculty = await _service.GetByIdAsync(id);
            if (faculty == null) return NotFound();
            ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name", faculty.DepartmentId);
            return View(faculty);
        }

        // POST: /Faculty/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Faculty faculty)
        {
            if (id != faculty.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name", faculty.DepartmentId);
                return View(faculty);
            }
            var result = await _service.UpdateAsync(faculty);
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: /Faculty/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var faculty = await _service.GetByIdAsync(id);
            return faculty == null ? NotFound() : View(faculty);
        }

        // POST: /Faculty/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Faculty deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
