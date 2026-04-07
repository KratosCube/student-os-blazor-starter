using Microsoft.EntityFrameworkCore;
using StudentOs.Blazor.Data.Models;

namespace StudentOs.Blazor.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<StudySession> StudySessions => Set<StudySession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subject>()
            .Property(x => x.Color)
            .HasDefaultValue("#6366f1");

        modelBuilder.Entity<Subject>()
            .Property(x => x.Name)
            .HasMaxLength(50);

        modelBuilder.Entity<Exam>()
            .HasOne(x => x.Subject)
            .WithMany(x => x.Exams)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudySession>()
            .HasOne(x => x.Subject)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
