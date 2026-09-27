using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudySync.Data;
using StudySync.Models;

namespace StudySync.Controllers;

public class NotesController : Controller
{
    private readonly ApplicationDbContext _db;
    public NotesController(ApplicationDbContext db) => _db = db;
    private int? UserId => HttpContext.Session.GetInt32("UserId");

    public async Task<IActionResult> Index()
    {
        if (UserId == null) return RedirectToAction("Login", "Account");
        ViewBag.Subjects = await _db.Subjects.Where(x => x.UserId == UserId).OrderBy(x => x.Name).ToListAsync();
        return View(await _db.Notes.Include(x => x.Subject).Where(x => x.UserId == UserId).OrderByDescending(x => x.CreatedAt).ToListAsync());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Note model)
    {
        if (UserId == null) return RedirectToAction("Login", "Account");
        if (!string.IsNullOrWhiteSpace(model.Title) && !string.IsNullOrWhiteSpace(model.Content))
        {
            model.UserId = UserId.Value; model.CreatedAt = DateTime.Now;
            _db.Notes.Add(model); await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (UserId == null) return RedirectToAction("Login", "Account");
        var n = await _db.Notes.FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId);
        if (n != null) { _db.Notes.Remove(n); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}
