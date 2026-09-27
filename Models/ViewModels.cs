using System.ComponentModel.DataAnnotations;

namespace StudySync.Models;

public class RegisterViewModel
{
    [Required, StringLength(50)]
    public string FullName { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, MinLength(6)]
    public string Password { get; set; } = "";

    [Required, Compare("Password")]
    public string ConfirmPassword { get; set; } = "";
}

public class LoginViewModel
{
    [Required, Display(Name = "Username / Email")]
    public string Email { get; set; } = "";

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = "";
}

public class DashboardViewModel
{
    public string UserName { get; set; } = "";
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int PendingTasks { get; set; }
    public int Progress { get; set; }
    public List<TaskItem> TodayTasks { get; set; } = new();
    public List<TaskItem> UpcomingTasks { get; set; } = new();
    public List<SubjectProgress> SubjectProgress { get; set; } = new();
}

public class SubjectProgress
{
    public string SubjectName { get; set; } = "";
    public int Total { get; set; }
    public int Completed { get; set; }
    public int Percentage => Total == 0 ? 0 : (int)Math.Round(Completed * 100.0 / Total);
}
