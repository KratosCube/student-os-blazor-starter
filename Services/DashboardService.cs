using Microsoft.EntityFrameworkCore;
using StudentOs.Blazor.Data;
using StudentOs.Blazor.Data.Models;

namespace StudentOs.Blazor.Services;

public class DashboardService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public DashboardService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<DashboardVm> GetDashboardAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var subjects = await db.Subjects
            .Include(x => x.Exams.OrderBy(e => e.Date))
            .Include(x => x.Sessions.OrderByDescending(s => s.CreatedAt))
            .OrderBy(x => x.Name)
            .ToListAsync();

        var exams = await db.Exams
            .Include(x => x.Subject)
            .OrderBy(x => x.Date)
            .ToListAsync();

        var now = DateTime.Now;
        var today = now.Date;
        var sevenDaysAgo = today.AddDays(-6);

        var todayTotalMinutes = subjects
            .SelectMany(x => x.Sessions)
            .Where(x => x.CreatedAt.Date == today)
            .Sum(x => x.Duration);

        var weekTotalMinutes = subjects
            .SelectMany(x => x.Sessions)
            .Where(x => x.CreatedAt.Date >= sevenDaysAgo)
            .Sum(x => x.Duration);

        var lifetimeMinutes = subjects
            .SelectMany(x => x.Sessions)
            .Sum(x => x.Duration);

        var totalLifetimeCredits = lifetimeMinutes / 45;

        var activeExams = exams
            .Where(x => !x.IsDone)
            .ToList();

        var todayExams = activeExams
            .Where(x => x.Date.Date == today)
            .ToList();

        var weeklySeries = Enumerable.Range(0, 7)
            .Select(index =>
            {
                var day = sevenDaysAgo.AddDays(index);

                var minutes = subjects
                    .SelectMany(x => x.Sessions)
                    .Where(x => x.CreatedAt.Date == day)
                    .Sum(x => x.Duration);

                return new ChartItemVm(
                    day.ToString("dd.MM"),
                    minutes,
                    null
                );
            })
            .ToList();

        var subjectSeries = subjects
            .Select(subject => new ChartItemVm(
                subject.Name,
                subject.Sessions.Sum(session => session.Duration),
                subject.Color))
            .OrderByDescending(x => x.Value)
            .ToList();

        return new DashboardVm(
            subjects,
            exams,
            activeExams,
            todayExams,
            todayTotalMinutes,
            weekTotalMinutes,
            lifetimeMinutes,
            totalLifetimeCredits,
            weeklySeries,
            subjectSeries
        );
    }
}

public record ChartItemVm(string Label, int Value, string? Color);

public record DashboardVm(
    List<Subject> Subjects,
    List<Exam> Exams,
    List<Exam> ActiveExams,
    List<Exam> TodayExams,
    int TodayTotalMinutes,
    int WeekTotalMinutes,
    int LifetimeMinutes,
    int TotalLifetimeCredits,
    List<ChartItemVm> WeeklySeries,
    List<ChartItemVm> SubjectSeries
);