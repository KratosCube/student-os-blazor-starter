using Microsoft.EntityFrameworkCore;
using StudentOs.Blazor.Data;
using StudentOs.Blazor.Data.Models;

namespace StudentOs.Blazor.Services;

public class StudySessionService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public StudySessionService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    // Uloží už připravenou studijní session do databáze.
    public async Task AddAsync(StudySession session)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.StudySessions.Add(session);
        await db.SaveChangesAsync();
    }

    // Vytvoří a uloží novou studijní session podle předmětu a počtu minut
    public async Task LogSessionAsync(int subjectId, int minutes)
    {
        if (subjectId <= 0 || minutes <= 0)
            return;

        await using var db = await _dbFactory.CreateDbContextAsync();
        db.StudySessions.Add(
            new StudySession
            {
                SubjectId = subjectId,
                Duration = minutes,
                CreatedAt = DateTime.Now,
            }
        );
        await db.SaveChangesAsync();
    }
}
