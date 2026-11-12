using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data
{
    // Seeds Bangladeshi demo data - 100 students, 12 faculty, 3 departments
    // Idempotent: if 100 students already exist, it does nothing
    public static class UniversitySeedData
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var db = services.GetRequiredService<ApplicationDbContext>();

            // Departments - use the existing 3, do not create duplicates
            var departments = await db.Departments.ToListAsync();
            if (departments.Count == 0)
            {
                departments = new List<Department>
                {
                    new Department { Name = "Computer Science and Engineering", Code = "CSE", HeadOfDepartment = "Prof. Dr. Kamal Hossain" },
                    new Department { Name = "Business Administration", Code = "BBA", HeadOfDepartment = "Dr. Nasrin Akter" },
                    new Department { Name = "Electrical and Electronic Engineering", Code = "EEE", HeadOfDepartment = "Prof. Dr. Rafiq Islam" }
                };
                db.Departments.AddRange(departments);
                await db.SaveChangesAsync();
            }

            // Faculty - 12 total, 4 per department
            if (await db.Faculties.CountAsync() < 12)
            {
                var facultyNames = new (string First, string Last, string Designation)[]
                {
                    ("Kamal", "Hossain", "Professor"), ("Sadia", "Rahman", "Assistant Professor"),
                    ("Tanvir", "Hasan", "Lecturer"), ("Mitu", "Chowdhury", "Lecturer"),
                    ("Nasrin", "Akter", "Associate Professor"), ("Arif", "Billah", "Assistant Professor"),
                    ("Shakila", "Khan", "Lecturer"), ("Rashed", "Mahmud", "Lecturer"),
                    ("Rafiq", "Islam", "Professor"), ("Nusrat", "Jahan", "Assistant Professor"),
                    ("Hasan", "Ali", "Lecturer"), ("Farzana", "Ahmed", "Lecturer")
                };
                for (var i = 0; i < facultyNames.Length; i++)
                {
                    var (first, last, designation) = facultyNames[i];
                    var email = $"{first.ToLower()}.{last.ToLower()}@universe.edu";
                    if (await db.Faculties.AnyAsync(f => f.Email == email)) continue;
                    db.Faculties.Add(new Faculty
                    {
                        FirstName = first,
                        LastName = last,
                        Designation = designation,
                        DepartmentId = departments[i % 3].Id,
                        Email = email,
                        Phone = $"017{10000000 + i * 12345}",
                        JoiningDate = new DateTime(2015 + i % 9, 1, 1)
                    });
                }
                await db.SaveChangesAsync();
            }

            // Students - 100 total (2 seeded already exist, so add 98 more)
            if (await db.Students.CountAsync() >= 100) return;

            var firstNames = new[] { "Abdur", "Fatema", "Tanvir", "Nusrat", "Rahim", "Ayesha", "Sakib", "Mehedi", "Riya", "Hasan", "Shakib", "Tania", "Imran", "Priya", "Arif", "Lima", "Rashed", "Mitu", "Siam", "Nabila", "Rakib", "Farhana", "Jahid", "Sumaiya", "Anik" };
            var lastNames = new[] { "Rahman", "Khatun", "Ahmed", "Hossain", "Islam", "Khan", "Ali", "Begum", "Chowdhury", "Akter", "Uddin", "Sultana", "Miah", "Hasan", "Jahan" };
            var random = new Random(42);
            var batches = new[] { "2023", "2024", "2025" };

            // Next serial per department+batch, starting after the existing StudentIds
            var nextSerial = new Dictionary<string, int>();
            var existingIds = await db.Students.Select(s => s.StudentId).ToListAsync();
            foreach (var id in existingIds)
            {
                var parts = id.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out var n))
                {
                    var prefix = $"{parts[0]}-{parts[1]}-";
                    if (!nextSerial.ContainsKey(prefix) || nextSerial[prefix] <= n)
                        nextSerial[prefix] = n + 1;
                }
            }

            var toAdd = 100 - await db.Students.CountAsync();
            var students = new List<Student>();
            for (var i = 0; i < toAdd; i++)
            {
                var dept = departments[i % 3];
                var batch = batches[i % 3];
                var prefix = $"{dept.Code}-{batch}-";
                if (!nextSerial.ContainsKey(prefix)) nextSerial[prefix] = 1;
                var serial = nextSerial[prefix]++;

                var first = firstNames[random.Next(firstNames.Length)];
                var last = lastNames[random.Next(lastNames.Length)];
                var gender = random.Next(2) == 0 ? "Male" : "Female";
                var program = dept.Code switch
                {
                    "CSE" => "BSc in CSE",
                    "BBA" => "BBA",
                    _ => "BSc in EEE"
                };

                students.Add(new Student
                {
                    FirstName = first,
                    LastName = last,
                    StudentId = $"{prefix}{serial:000}",
                    DepartmentId = dept.Id,
                    Program = program,
                    Semester = batch == "2025" ? 1 : batch == "2024" ? 3 : 5,
                    Batch = batch,
                    Gender = gender,
                    DateOfBirth = new DateTime(2003 + random.Next(4), random.Next(1, 13), random.Next(1, 28)),
                    Phone = $"01{random.Next(300000000, 999999999)}",
                    Email = $"{first.ToLower()}.{last.ToLower()}{i}@universe.edu",
                    Address = "Dhaka, Bangladesh",
                    GuardianName = $"Md. {last}",
                    GuardianPhone = $"01{random.Next(300000000, 999999999)}",
                    AdmissionDate = new DateTime(int.Parse(batch), 1, 1),
                    IsActive = true
                });
            }

            db.Students.AddRange(students);
            await db.SaveChangesAsync();
        }
    }
}
