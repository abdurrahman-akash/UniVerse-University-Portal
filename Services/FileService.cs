using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SchoolManagementSystem.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string?> SaveStudentPhotoAsync(IFormFile? file, string studentId, string studentName)
        {
            if (file == null || file.Length == 0)
                return null;

            if (file.Length > MaxFileSizeInBytes)
                throw new InvalidOperationException("File size exceeds the 5MB limit.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Only .jpg, .jpeg, .png, and .webp images are allowed.");

            // Target directory: wwwroot/uploads/student-photos/
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "student-photos");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Create a clean, human-readable file name: [StudentID]_[StudentName]_[Timestamp].[ext]
            var cleanStudentId = Regex.Replace(studentId ?? "student", @"[^a-zA-Z0-9_-]", "_");
            var cleanName = Regex.Replace(studentName ?? "photo", @"[^a-zA-Z0-9_-]", "_");
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            var fileName = $"{cleanStudentId}_{cleanName}_{timestamp}{extension}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return browser-accessible relative path
            return $"/uploads/student-photos/{fileName}";
        }

        public bool DeleteFile(string? relativeFilePath)
        {
            if (string.IsNullOrWhiteSpace(relativeFilePath))
                return false;

            try
            {
                // Trim leading slash to combine correctly with WebRootPath
                var trimmedPath = relativeFilePath.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
                var fullPath = Path.Combine(_environment.WebRootPath, trimmedPath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
            }
            catch
            {
                // File deletion failed gracefully (e.g. file lock or permissions)
            }

            return false;
        }
    }
}

