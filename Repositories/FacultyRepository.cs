using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.ViewModels;

namespace SchoolManagementSystem.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly ApplicationDbContext _db;
        public FacultyRepository(ApplicationDbContext db) => _db = db;

        //public async Task<List<Faculty>> GetAllAsync(string? search = null, int? departmentId = null)
        //{
        //    var query = _db.Faculties.Include(f => f.Department).AsQueryable();
        //    if (departmentId.HasValue)
        //        query = query.Where(f => f.DepartmentId == departmentId.Value);
        //    if (!string.IsNullOrWhiteSpace(search))
        //        query = query.Where(f => f.FirstName.Contains(search) || f.LastName.Contains(search) || f.Email!.Contains(search));
        //    return await query.OrderBy(f => f.FirstName).ToListAsync();
        //}

        public async Task<PagedResult<Faculty>> GetPagedAsync(string? search = null, int? departmentId = null, int page = 1, int pageSize = 10)
        {
            var query = _db.Faculties.Include(f => f.Department).AsQueryable();
            if (departmentId.HasValue)
                query = query.Where(f => f.DepartmentId == departmentId.Value);
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(f => f.FirstName.Contains(search) || f.LastName.Contains(search) || f.Email!.Contains(search));

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(f => f.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Faculty> { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize };
        }


        public async Task<Faculty?> GetByIdAsync(int id)
            => await _db.Faculties.Include(f => f.Department).FirstOrDefaultAsync(f => f.Id == id);

        public async Task AddAsync(Faculty faculty)
        { _db.Faculties.Add(faculty); await _db.SaveChangesAsync(); }

        public async Task UpdateAsync(Faculty faculty)
        { _db.Faculties.Update(faculty); await _db.SaveChangesAsync(); }

        public async Task DeleteAsync(int id)
        {
            var f = await _db.Faculties.FindAsync(id);
            if (f != null) { _db.Faculties.Remove(f); await _db.SaveChangesAsync(); }
        }
    }
}
