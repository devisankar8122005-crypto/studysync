using System.ComponentModel.DataAnnotations;

namespace StudySync.Models;

public class TimetableItem
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required]
    public string Day { get; set; } = "";

    [Required]
    public string Time { get; set; } = "";

    [Required]
    public string Activity { get; set; } = "";

    public User? User { get; set; }
}
