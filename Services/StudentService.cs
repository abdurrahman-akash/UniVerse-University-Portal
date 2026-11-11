using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.ViewModels;
using SchoolManagementSystem.Repositories;

namespace SchoolManagementSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;
        public StudentService(IStudentRepository repo) => _repo = repo;

        //public Task<List<Student>> GetAllAsync(string? search = null, int? departmentId = null)
        //    => _repo.GetAllAsync(search, departmentId);
        public Task<PagedResult<Student>> GetPagedAsync(string? search = null, int? departmentId = null, int page = 1, int pageSize = 10)
    => _repo.GetPagedAsync(search, departmentId, page, pageSize);


        public Task<Student?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<(bool Success, string Message)> CreateAsync(Student student)
        {
            if (await _repo.StudentIdExistsAsync(student.StudentId))
                return (false, $"Student ID {student.StudentId} already exists.");
            await _repo.AddAsync(student);
            return (true, "Student added successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Student student)
        {
            if (await _repo.StudentIdExistsAsync(student.StudentId, student.Id))
                return (false, $"Student ID {student.StudentId} already exists.");
            await _repo.UpdateAsync(student);
            return (true, "Student updated successfully.");
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
