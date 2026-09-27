using Microsoft.AspNetCore.Mvc;

namespace StudySync.Controllers;

public class StudyPlanController : Controller
{
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("UserId") == null) return RedirectToAction("Login", "Account");
        return View();
    }
}
