using Microsoft.AspNetCore.Http;

namespace SchoolManagementSystem.Services
{
    public interface IFileService
    {
        /// <summary>
        /// Saves a student's photo into wwwroot/uploads/student-photos/ with a readable, distinct filename.
        /// Returns the relative URL path to save in the database.
        /// </summary>
        Task<string?> SaveStudentPhotoAsync(IFormFile? file, string studentId, string studentName);

        /// <summary>
        /// Safely deletes a file from wwwroot if it exists.
        /// </summary>
        bool DeleteFile(string? relativeFilePath);
    }
}

