using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.ViewModels;
using SchoolManagementSystem.Repositories;

namespace SchoolManagementSystem.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _repo;
        public FacultyService(IFacultyRepository repo) => _repo = repo;

        //public Task<List<Faculty>> GetAllAsync(string? search = null, int? departmentId = null)
        //    => _repo.GetAllAsync(search, departmentId);
        public Task<PagedResult<Faculty>> GetPagedAsync(string? search = null, int? departmentId = null, int page = 1, int pageSize = 10)
    => _repo.GetPagedAsync(search, departmentId, page, pageSize);


        public Task<Faculty?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<(bool Success, string Message)> CreateAsync(Faculty faculty)
        {
            await _repo.AddAsync(faculty);
            return (true, "Faculty added successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Faculty faculty)
        {
            await _repo.UpdateAsync(faculty);
            return (true, "Faculty updated successfully.");
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
