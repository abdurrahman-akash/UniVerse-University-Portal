using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.ViewModels;

namespace SchoolManagementSystem.Services
{
    public interface IFacultyService
    {
        //Task<List<Faculty>> GetAllAsync(string? search = null, int? departmentId = null);
        Task<PagedResult<Faculty>> GetPagedAsync(string? search = null, int? departmentId = null, int page = 1, int pageSize = 10);

        Task<Faculty?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Faculty faculty);
        Task<(bool Success, string Message)> UpdateAsync(Faculty faculty);
        Task DeleteAsync(int id);
    }
}
