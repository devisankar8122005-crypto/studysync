using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudySync.Data;

namespace StudySync.Controllers;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _db;
    public ProfileController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");
        var user = await _db.Users.Include(x => x.Tasks).FirstAsync(x => x.Id == userId);
        ViewBag.Completed = user.Tasks.Count(x => x.IsCompleted);
        ViewBag.Total = user.Tasks.Count;
        return View(user);
    }
}
