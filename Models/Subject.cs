using System.ComponentModel.DataAnnotations;

namespace StudySync.Models;

public class Subject
{
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = "";

    public string? Color { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public User? User { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    public ICollection<Note> Notes { get; set; } = new List<Note>();
}