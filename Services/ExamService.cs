using Microsoft.EntityFrameworkCore;
using StudentOs.Blazor.Data;
using StudentOs.Blazor.Data.Models;

namespace StudentOs.Blazor.Services;

public class ExamService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public ExamService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Exam>> GetAllAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        return await db.Exams
            .Include(x => x.Subject)
            .OrderBy(x => x.Date)
            .ToListAsync();
    }

    public async Task<Exam?> GetByIdAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        return await db.Exams
            .Include(x => x.Subject)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Exam exam)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        db.Exams.Add(new Exam
        {
            SubjectId = exam.SubjectId,
            Date = exam.Date,
            Type = exam.Type,
            Duration = exam.Duration,
            IsDone = exam.IsDone,
            LegacyName = exam.LegacyName
        });

        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Exam exam)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var entity = await db.Exams.FirstOrDefaultAsync(x => x.Id == exam.Id);
        if (entity is null)
            return;

        entity.SubjectId = exam.SubjectId;
        entity.Date = exam.Date;
        entity.Type = exam.Type;
        entity.Duration = exam.Duration;
        entity.IsDone = exam.IsDone;
        entity.LegacyName = exam.LegacyName;

        await db.SaveChangesAsync();
    }

    public async Task ToggleAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var entity = await db.Exams.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
            return;

        entity.IsDone = !entity.IsDone;
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var entity = await db.Exams.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
            return;

        db.Exams.Remove(entity);
        await db.SaveChangesAsync();
    }
}