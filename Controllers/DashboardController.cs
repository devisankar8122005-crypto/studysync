using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudySync.Data;
using StudySync.Models;

namespace StudySync.Controllers;

public class DashboardController : Controller
{
    private readonly ApplicationDbContext _db;
    public DashboardController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var tasks = await _db.Tasks.Include(x => x.Subject).Where(x => x.UserId == userId).ToListAsync();
        var total = tasks.Count;
        var completed = tasks.Count(x => x.IsCompleted);

        var vm = new DashboardViewModel
        {
            UserName = HttpContext.Session.GetString("UserName") ?? "Student",
            TotalTasks = total,
            CompletedTasks = completed,
            PendingTasks = total - completed,
            Progress = total == 0 ? 0 : (int)Math.Round(completed * 100.0 / total),
            TodayTasks = tasks.Where(x => x.DueDate.Date == DateTime.Today).OrderBy(x => x.IsCompleted).ThenBy(x => x.DueDate).ToList(),
            UpcomingTasks = tasks.Where(x => !x.IsCompleted && x.DueDate.Date >= DateTime.Today).OrderBy(x => x.DueDate).Take(5).ToList(),
            SubjectProgress = tasks.GroupBy(x => x.Subject?.Name ?? "No Subject").Select(g => new SubjectProgress
            {
                SubjectName = g.Key, Total = g.Count(), Completed = g.Count(x => x.IsCompleted)
            }).OrderBy(x => x.SubjectName).ToList()
        };
        return View(vm);
    }
}
