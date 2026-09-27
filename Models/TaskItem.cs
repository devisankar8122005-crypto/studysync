using System.ComponentModel.DataAnnotations;

namespace StudySync.Models;

public class TaskItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? SubjectId { get; set; }

    [Required, StringLength(150)]
    public string TaskName { get; set; } = "";

    [StringLength(500)]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; } = DateTime.Today;

    [Required]
    public string Priority { get; set; } = "Medium";

    public bool Reminder { get; set; }
    public bool IsCompleted { get; set; }

    public User? User { get; set; }
    public Subject? Subject { get; set; }
}