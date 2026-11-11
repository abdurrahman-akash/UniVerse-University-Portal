using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data
{
    // Creates the Admin, Faculty and Student roles and one Admin user on first run
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();

            foreach (var role in new[] { AppRoles.Admin, AppRoles.Faculty, AppRoles.Student })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            const string adminEmail = "admin@universe.edu";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, "Admin123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }
            else if (!await userManager.IsInRoleAsync(admin, AppRoles.Admin))
            {
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }
        }
    }
}
