using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class Notice
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.Now;
        public string Audience { get; set; } = "All";
    }
}
