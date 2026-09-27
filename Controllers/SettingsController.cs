using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudySync.Data;

namespace StudySync.Controllers;

public class SettingsController : Controller
{
    private readonly ApplicationDbContext _db;

    public SettingsController(ApplicationDbContext db)
    {
        _db = db;
    }

    // =========================
    // SETTINGS HOME
    // =========================
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToAction("Login", "Account");

        return View();
    }


    // =========================
    // CHANGE PASSWORD
    // =========================
    [HttpGet]
    public IActionResult ChangePassword()
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToAction("Login", "Account");

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> ChangePassword(
        string CurrentPassword,
        string NewPassword,
        string ConfirmPassword)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        if (string.IsNullOrWhiteSpace(CurrentPassword) ||
            string.IsNullOrWhiteSpace(NewPassword) ||
            string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ViewBag.Error = "Please fill all password fields.";
            return View();
        }

        if (NewPassword != ConfirmPassword)
        {
            ViewBag.Error = "New password and confirm password do not match.";
            return View();
        }

        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId.Value);

        if (user == null)
            return RedirectToAction("Login", "Account");

        if (user.PasswordHash != CurrentPassword)
        {
            ViewBag.Error = "Current password is incorrect.";
            return View();
        }

        user.PasswordHash = NewPassword;

        await _db.SaveChangesAsync();

        ViewBag.Success = "Password changed successfully.";

        return View();
    }


    // =========================
    // EDIT PROFILE
    // =========================
    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId.Value);

        if (user == null)
            return RedirectToAction("Login", "Account");

        return View(user);
    }


    [HttpPost]
    public async Task<IActionResult> EditProfile(string FullName, string Email)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId.Value);

        if (user == null)
            return RedirectToAction("Login", "Account");

        if (string.IsNullOrWhiteSpace(FullName) ||
            string.IsNullOrWhiteSpace(Email))
        {
            ViewBag.Error = "Please fill all fields.";
            return View(user);
        }

        user.FullName = FullName;
        user.Email = Email;

        await _db.SaveChangesAsync();

        HttpContext.Session.SetString("UserName", FullName);

        ViewBag.Success = "Profile updated successfully.";

        return View(user);
    }


    // =========================
    // REMINDER SETTINGS
    // =========================
    [HttpGet]
    public IActionResult Reminders()
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToAction("Login", "Account");

        ViewBag.RemindersEnabled =
            HttpContext.Session.GetString("RemindersEnabled") != "false";

        ViewBag.ReminderTime =
            HttpContext.Session.GetString("ReminderTime") ?? "18:00";

        return View();
    }


    [HttpPost]
    public IActionResult Reminders(bool RemindersEnabled, string ReminderTime)
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToAction("Login", "Account");

        HttpContext.Session.SetString(
            "RemindersEnabled",
            RemindersEnabled ? "true" : "false"
        );

        HttpContext.Session.SetString(
            "ReminderTime",
            ReminderTime ?? "18:00"
        );

        ViewBag.Success = "Reminder settings saved successfully.";

        ViewBag.RemindersEnabled = RemindersEnabled;
        ViewBag.ReminderTime = ReminderTime ?? "18:00";

        return View();
    }


    // =========================
    // DARK MODE
    // =========================
    [HttpGet]
    public IActionResult DarkMode()
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToAction("Login", "Account");

        ViewBag.DarkMode =
            HttpContext.Session.GetString("DarkMode") == "true";

        return View();
    }


    [HttpPost]
    public IActionResult DarkMode(bool Enabled)
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToAction("Login", "Account");

        HttpContext.Session.SetString(
            "DarkMode",
            Enabled ? "true" : "false"
        );

        ViewBag.DarkMode = Enabled;
        ViewBag.Success = "Dark mode setting saved successfully.";

        return View();
    }


    // =========================
    // GENERAL PREFERENCES
    // =========================
    [HttpGet]
    public IActionResult Preferences()
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToAction("Login", "Account");

        ViewBag.StartPage =
            HttpContext.Session.GetString("StartPage") ?? "Dashboard";

        ViewBag.TaskView =
            HttpContext.Session.GetString("TaskView") ?? "List";

        return View();
    }


    [HttpPost]
    public IActionResult Preferences(string StartPage, string TaskView)
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToAction("Login", "Account");

        HttpContext.Session.SetString(
            "StartPage",
            StartPage ?? "Dashboard"
        );

        HttpContext.Session.SetString(
            "TaskView",
            TaskView ?? "List"
        );

        ViewBag.StartPage = StartPage ?? "Dashboard";
        ViewBag.TaskView = TaskView ?? "List";

        ViewBag.Success = "General preferences saved successfully.";

        return View();
    }
}