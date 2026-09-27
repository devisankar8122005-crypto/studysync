using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudySync.Data;
using StudySync.Models;

namespace StudySync.Controllers;

public class TasksController : Controller
{
    private readonly ApplicationDbContext _db;

    public TasksController(ApplicationDbContext db) => _db = db;

    private int? UserId => HttpContext.Session.GetInt32("UserId");


    // =========================
    // TASK LIST
    // =========================
    public async Task<IActionResult> Index(string? search, string? status)
    {
        if (UserId == null)
            return RedirectToAction("Login", "Account");

        var q = _db.Tasks
            .Include(x => x.Subject)
            .Where(x => x.UserId == UserId);

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(x => x.TaskName.Contains(search));

        if (status == "completed")
            q = q.Where(x => x.IsCompleted);

        if (status == "pending")
            q = q.Where(x => !x.IsCompleted);

        ViewBag.Search = search;
        ViewBag.Status = status;

        return View(
            await q
                .OrderBy(x => x.IsCompleted)
                .ThenBy(x => x.DueDate)
                .ToListAsync()
        );
    }


    // =========================
    // CREATE - GET
    // =========================
    public async Task<IActionResult> Create()
    {
        if (UserId == null)
            return RedirectToAction("Login", "Account");

        ViewBag.Subjects = await _db.Subjects
            .Where(x => x.UserId == UserId)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return View(new TaskItem
        {
            StartDate = DateTime.Today,
            DueDate = DateTime.Today
        });
    }


    // =========================
    // CREATE - POST
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskItem model)
    {
        if (UserId == null)
            return RedirectToAction("Login", "Account");

        // Start Date should not be after Due Date
        if (model.StartDate > model.DueDate)
        {
            ModelState.AddModelError(
                "StartDate",
                "Start Date cannot be after Due Date."
            );
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Subjects = await _db.Subjects
                .Where(x => x.UserId == UserId)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return View(model);
        }

        model.UserId = UserId.Value;

        _db.Tasks.Add(model);

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // =========================
    // EDIT - GET
    // =========================
    public async Task<IActionResult> Edit(int id)
    {
        if (UserId == null)
            return RedirectToAction("Login", "Account");

        var item = await _db.Tasks
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == UserId);

        if (item == null)
            return NotFound();

        ViewBag.Subjects = await _db.Subjects
            .Where(x => x.UserId == UserId)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return View(item);
    }


    // =========================
    // EDIT - POST
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TaskItem model)
    {
        if (UserId == null)
            return RedirectToAction("Login", "Account");

        // Start Date should not be after Due Date
        if (model.StartDate > model.DueDate)
        {
            ModelState.AddModelError(
                "StartDate",
                "Start Date cannot be after Due Date."
            );
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Subjects = await _db.Subjects
                .Where(x => x.UserId == UserId)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return View(model);
        }

        var item = await _db.Tasks
            .FirstOrDefaultAsync(x =>
                x.Id == model.Id &&
                x.UserId == UserId);

        if (item == null)
            return NotFound();

        item.SubjectId = model.SubjectId;
        item.TaskName = model.TaskName;
        item.Description = model.Description;

        // Start Date + Due Date
        item.StartDate = model.StartDate;
        item.DueDate = model.DueDate;

        item.Priority = model.Priority;
        item.Reminder = model.Reminder;
        item.IsCompleted = model.IsCompleted;

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // =========================
    // TOGGLE COMPLETE
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int id)
    {
        if (UserId == null)
            return RedirectToAction("Login", "Account");

        var item = await _db.Tasks
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == UserId);

        if (item != null)
        {
            item.IsCompleted = !item.IsCompleted;

            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }


    // =========================
    // DELETE
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (UserId == null)
            return RedirectToAction("Login", "Account");

        var item = await _db.Tasks
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == UserId);

        if (item != null)
        {
            _db.Tasks.Remove(item);

            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}