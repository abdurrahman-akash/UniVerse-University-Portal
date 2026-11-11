using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using System.Reflection.Emit;


namespace SchoolManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Notice> Notices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ekjon student er StudentId ekbar e thakbe, duplicate hobe na
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.StudentId)
                .IsUnique();

            // Demo Department data
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Computer Science and Engineering", Code = "CSE" },
                new Department { Id = 2, Name = "Business Administration", Code = "BBA" },
                new Department { Id = 3, Name = "Electrical and Electronic Engineering", Code = "EEE" }
            );

            // Demo Student data
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, FirstName = "Abdur", LastName = "Rahman", StudentId = "CSE-2024-001", DepartmentId = 1, Program = "BSc in CSE", Semester = 1, Batch = "2024", Gender = "Male", AdmissionDate = new DateTime(2024, 1, 1), IsActive = true },
                new Student { Id = 2, FirstName = "Fatema", LastName = "Khatun", StudentId = "BBA-2024-001", DepartmentId = 2, Program = "BBA", Semester = 1, Batch = "2024", Gender = "Female", AdmissionDate = new DateTime(2024, 1, 1), IsActive = true }
            );

            // Seed faculty - real data for the dashboard count
            modelBuilder.Entity<Faculty>().HasData(
                new Faculty { Id = 1, FirstName = "Kamal", LastName = "Hossain", Designation = "Professor", DepartmentId = 1, Email = "kamal@universe.edu" },
                new Faculty { Id = 2, FirstName = "Nasrin", LastName = "Akter", Designation = "Associate Professor", DepartmentId = 2, Email = "nasrin@universe.edu" },
                new Faculty { Id = 3, FirstName = "Rafiq", LastName = "Islam", Designation = "Professor", DepartmentId = 3, Email = "rafiq@universe.edu" }
            );

            // Seed notices - real data for the notice board
            modelBuilder.Entity<Notice>().HasData(
                new Notice { Id = 1, Title = "Mid-Term Exam Routine Published", Description = "Mid-term exam will start from 15th March.", PublishDate = new DateTime(2026, 3, 1), Audience = "Students" },
                new Notice { Id = 2, Title = "New Semester Registration Open", Description = "Registration for Spring 2026 is now open.", PublishDate = new DateTime(2026, 2, 20), Audience = "All" }
            );

        }
    }
}