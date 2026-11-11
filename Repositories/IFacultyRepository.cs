using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.ViewModels;

namespace SchoolManagementSystem.Repositories
{
    public interface IFacultyRepository
    {
        //Task<List<Faculty>> GetAllAsync(string? search = null, int? departmentId = null);
        Task<PagedResult<Faculty>> GetPagedAsync(string? search = null, int? departmentId = null, int page = 1, int pageSize = 10);
        Task<Faculty?> GetByIdAsync(int id);
        Task AddAsync(Faculty faculty);
        Task UpdateAsync(Faculty faculty);
        Task DeleteAsync(int id);
    }
}
