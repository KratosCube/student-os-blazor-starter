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

    // Načte aktivní předměty včetně jejich termínů a studijních session
    public async Task<List<Subject>> GetAllAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db
            .Subjects.Where(x => !x.IsArchived)
            .Include(x => x.Exams.OrderBy(e => e.Date))
            .Include(x => x.Sessions.OrderByDescending(s => s.CreatedAt))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    // Přidá nový předmět do databáze
    public async Task AddAsync(Subject subject)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        subject.IsArchived = false;
        db.Subjects.Add(subject);
        await db.SaveChangesAsync();
    }

    // Upraví existující aktivní předmět
    public async Task UpdateAsync(Subject subject)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = await db.Subjects.FirstOrDefaultAsync(x => x.Id == subject.Id && !x.IsArchived);
        if (entity is null)
            return;

        entity.Name = subject.Name;
        entity.Color = subject.Color;
        await db.SaveChangesAsync();
    }

    // Archivuje předmět podle jeho Id, aby jeho historie zůstala zachovaná
    public async Task DeleteAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var subject = await db.Subjects.FindAsync(id);
        if (subject is null)
            return;

        subject.IsArchived = true;
        await db.SaveChangesAsync();
    }
}
