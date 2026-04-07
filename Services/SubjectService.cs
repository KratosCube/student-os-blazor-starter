using Microsoft.EntityFrameworkCore;
using StudentOs.Blazor.Data;
using StudentOs.Blazor.Data.Models;

namespace StudentOs.Blazor.Services;

public class SubjectService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public SubjectService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Subject>> GetAllAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Subjects
            .Include(x => x.Exams.OrderBy(e => e.Date))
            .Include(x => x.Sessions.OrderByDescending(s => s.CreatedAt))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Subject subject)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Subjects.Add(subject);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subject subject)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Subjects.Update(subject);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var subject = await db.Subjects.FindAsync(id);
        if (subject is null) return;
        db.Subjects.Remove(subject);
        await db.SaveChangesAsync();
    }
}
