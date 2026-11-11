using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.ViewModels;

namespace SchoolManagementSystem.Services
{
    public interface IStudentService
    {
        //Task<List<Student>> GetAllAsync(string? search = null, int? departmentId = null);
        Task<PagedResult<Student>> GetPagedAsync(string? search = null, int? departmentId = null, int page = 1, int pageSize = 10);
        Task<Student?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Student student);
        Task<(bool Success, string Message)> UpdateAsync(Student student);
        Task DeleteAsync(int id);
    }
}
