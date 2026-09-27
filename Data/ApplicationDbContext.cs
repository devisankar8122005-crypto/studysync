using Microsoft.EntityFrameworkCore;
using StudySync.Models;

namespace StudySync.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<TimetableItem> TimetableItems => Set<TimetableItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Subject>().HasOne(x => x.User).WithMany(x => x.Subjects).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<TaskItem>().HasOne(x => x.User).WithMany(x => x.Tasks).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<TaskItem>().HasOne(x => x.Subject).WithMany(x => x.Tasks).HasForeignKey(x => x.SubjectId).OnDelete(DeleteBehavior.SetNull);
       modelBuilder.Entity<Note>().HasOne(x => x.User).WithMany(x => x.Notes).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Note>().HasOne(x => x.Subject).WithMany(x => x.Notes).HasForeignKey(x => x.SubjectId).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<TimetableItem>().HasOne(x => x.User).WithMany(x => x.TimetableItems).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
