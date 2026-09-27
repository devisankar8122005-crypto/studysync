using System.ComponentModel.DataAnnotations;

namespace StudySync.Models;

public class User
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string FullName { get; set; } = "";

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = "";

    [Required]
    public string PasswordHash { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public ICollection<Note> Notes { get; set; } = new List<Note>();
    public ICollection<TimetableItem> TimetableItems { get; set; } = new List<TimetableItem>();
}
