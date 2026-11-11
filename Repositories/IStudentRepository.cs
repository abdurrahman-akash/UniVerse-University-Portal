using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.ViewModels;

namespace SchoolManagementSystem.Repositories
{
    public interface IStudentRepository
    {
        //Task<List<Student>> GetAllAsync(string? search = null, int? departmentId = null);
        Task<PagedResult<Student>> GetPagedAsync(string? search = null, int? departmentId = null, int page = 1, int pageSize = 10);
        Task<Student?> GetByIdAsync(int id);
        Task<bool> StudentIdExistsAsync(string studentId, int excludeId = 0);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(int id);

    }
}
