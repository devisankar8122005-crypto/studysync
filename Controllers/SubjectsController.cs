using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudySync.Data;
using StudySync.Models;

namespace StudySync.Controllers;

public class SubjectsController : Controller
{
    private readonly ApplicationDbContext _db;

    public SubjectsController(ApplicationDbContext db)
    {
        _db = db;
    }

    private int? UserId => HttpContext.Session.GetInt32("UserId");

    public async Task<IActionResult> Index()
    {
        if (UserId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var subjects = await _db.Subjects
            .Where(x => x.UserId == UserId.Value)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return View(subjects);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string name, IFormFile? file)
    {
        if (UserId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            string? fileName = null;
            string? filePath = null;

            if (file != null && file.Length > 0)
            {
                string uploadFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "subjects"
                );

                Directory.CreateDirectory(uploadFolder);

                fileName = Path.GetFileName(file.FileName);

                string uniqueFileName =
                    Guid.NewGuid().ToString() + "_" + fileName;

                string fullPath = Path.Combine(
                    uploadFolder,
                    uniqueFileName
                );

                using (var stream = new FileStream(
                    fullPath,
                    FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                filePath = "/uploads/subjects/" + uniqueFileName;
            }

            var subject = new Subject
            {
                UserId = UserId.Value,
                Name = name.Trim(),
                FileName = fileName,
                FilePath = filePath
            };

            _db.Subjects.Add(subject);

            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, string name)
    {
        if (UserId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var subject = await _db.Subjects
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == UserId.Value);

        if (subject != null &&
            !string.IsNullOrWhiteSpace(name))
        {
            subject.Name = name.Trim();

            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (UserId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var subject = await _db.Subjects
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == UserId.Value);

        if (subject != null)
        {
            _db.Subjects.Remove(subject);

            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}