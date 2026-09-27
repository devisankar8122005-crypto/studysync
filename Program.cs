using Microsoft.EntityFrameworkCore;
using StudySync.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();


// ===============================
// Explicit Controller Routes
// ===============================

app.MapControllerRoute(
    name: "tasks",
    pattern: "Tasks/{action=Index}/{id?}",
    defaults: new { controller = "Tasks" });

app.MapControllerRoute(
    name: "subjects",
    pattern: "Subjects/{action=Index}/{id?}",
    defaults: new { controller = "Subjects" });

app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard/{action=Index}/{id?}",
    defaults: new { controller = "Dashboard" });

app.MapControllerRoute(
    name: "calendar",
    pattern: "Calendar/{action=Index}/{id?}",
    defaults: new { controller = "Calendar" });

app.MapControllerRoute(
    name: "notes",
    pattern: "Notes/{action=Index}/{id?}",
    defaults: new { controller = "Notes" });

app.MapControllerRoute(
    name: "progress",
    pattern: "Progress/{action=Index}/{id?}",
    defaults: new { controller = "Progress" });

app.MapControllerRoute(
    name: "studyplan",
    pattern: "StudyPlan/{action=Index}/{id?}",
    defaults: new { controller = "StudyPlan" });

    app.MapControllerRoute(
    name: "profile",
    pattern: "Profile/{action=Index}/{id?}",
    defaults: new { controller = "Profile" });

   app.MapControllerRoute(
    name: "settings",
    pattern: "Settings/{action=Index}/{id?}",
    defaults: new { controller = "Settings" }); 

// ===============================
// Default Route
// ===============================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();