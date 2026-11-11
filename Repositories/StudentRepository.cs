using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.ViewModels;

namespace SchoolManagementSystem.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _db;
        public StudentRepository(ApplicationDbContext db) => _db = db;

        //public async Task<List<Student>> GetAllAsync(string? search = null, int? departmentId = null)
        //{
        //    var query = _db.Students.Include(s => s.Department).AsQueryable();
        //    if (departmentId.HasValue)
        //        query = query.Where(s => s.DepartmentId == departmentId.Value);
        //    if (!string.IsNullOrWhiteSpace(search))
        //        query = query.Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search) || s.StudentId.Contains(search));
        //    return await query.OrderBy(s => s.StudentId).ToListAsync();
        //}

        public async Task<PagedResult<Student>> GetPagedAsync(string? search = null, int? departmentId = null, int page = 1, int pageSize = 10)
        {
            var query = _db.Students.Include(s => s.Department).AsQueryable();
            if (departmentId.HasValue)
                query = query.Where(s => s.DepartmentId == departmentId.Value);
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search) || s.StudentId.Contains(search));

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(s => s.StudentId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Student> { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize };
        }


        public async Task<Student?> GetByIdAsync(int id)
            => await _db.Students.Include(s => s.Department).FirstOrDefaultAsync(s => s.Id == id);

        public async Task<bool> StudentIdExistsAsync(string studentId, int excludeId = 0)
            => await _db.Students.AnyAsync(s => s.StudentId == studentId && s.Id != excludeId);

        public async Task AddAsync(Student student)
        { _db.Students.Add(student); await _db.SaveChangesAsync(); }

        public async Task UpdateAsync(Student student)
        { _db.Students.Update(student); await _db.SaveChangesAsync(); }

        public async Task DeleteAsync(int id)
        {
            var s = await _db.Students.FindAsync(id);
            if (s != null) { _db.Students.Remove(s); await _db.SaveChangesAsync(); }
        }
    }
}
