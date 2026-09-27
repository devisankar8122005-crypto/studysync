using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudySync.Data;

namespace StudySync.Controllers;

public class ProgressController : Controller
{
    private readonly ApplicationDbContext _db;
    public ProgressController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");
        var tasks = await _db.Tasks.Include(x => x.Subject).Where(x => x.UserId == userId).ToListAsync();
        ViewBag.Total = tasks.Count;
        ViewBag.Completed = tasks.Count(x => x.IsCompleted);
        ViewBag.Pending = tasks.Count(x => !x.IsCompleted);
        ViewBag.Progress = tasks.Count == 0 ? 0 : (int)Math.Round(tasks.Count(x => x.IsCompleted) * 100.0 / tasks.Count);
        ViewBag.Subjects = tasks.GroupBy(x => x.Subject?.Name ?? "No Subject").Select(g => new { Name = g.Key, Total = g.Count(), Completed = g.Count(x => x.IsCompleted) }).ToList();
        return View();
    }
}
