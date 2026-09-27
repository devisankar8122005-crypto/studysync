using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudySync.Data;

namespace StudySync.Controllers;

public class CalendarController : Controller
{
    private readonly ApplicationDbContext _db;
    public CalendarController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(DateTime? date)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");
        var selected = (date ?? DateTime.Today).Date;
        ViewBag.SelectedDate = selected;
        ViewBag.Tasks = await _db.Tasks.Include(x => x.Subject)
            .Where(x => x.UserId == userId && x.DueDate.Date == selected)
            .OrderBy(x => x.DueDate).ToListAsync();
        return View();
    }
}
