using System.ComponentModel.DataAnnotations;

namespace StudySync.Models;

public class Note
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? SubjectId { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = "";

    [Required]
    public string Content { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public User? User { get; set; }
    public Subject? Subject { get; set; }
}
