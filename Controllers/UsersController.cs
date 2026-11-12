using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.ViewModels;

namespace SchoolManagementSystem.Controllers
{
    // User Management - only Admin can open this page
    [Authorize(Roles = AppRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Users - list all users with their roles
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var list = new List<(AppUser User, string Roles)>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                list.Add((user, string.Join(", ", roles)));
            }
            return View(list);
        }

        // GET: /Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Users/Create - Admin creates a user by role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, model.Role);
            TempData["Success"] = $"User {model.Email} created as {model.Role}.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Users/Delete - Admin cannot delete his own account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction(nameof(Index));
            }

            await _userManager.DeleteAsync(user);
            TempData["Success"] = $"User {user.Email} deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
