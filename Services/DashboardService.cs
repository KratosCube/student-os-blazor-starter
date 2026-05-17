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

        // Načtení dat s vazbami
        var subjects = await db
            .Subjects.Include(x => x.Exams.Where(e => !e.IsDone).OrderBy(e => e.Date))
            .Include(x => x.Sessions)
            .OrderBy(x => x.Name)
            .ToListAsync();

        var exams = await db.Exams.Include(x => x.Subject).OrderBy(x => x.Date).ToListAsync();

        var now = DateTime.Now;
        var today = now.Date;
        var sevenDaysAgo = today.AddDays(-6);

        // Základní statistiky
        var todayTotalMinutes = subjects
            .SelectMany(x => x.Sessions)
            .Where(x => x.CreatedAt.Date == today)
            .Sum(x => x.Duration);

        var weekTotalMinutes = subjects
            .SelectMany(x => x.Sessions)
            .Where(x => x.CreatedAt.Date >= sevenDaysAgo)
            .Sum(x => x.Duration);

        var lifetimeMinutes = subjects.SelectMany(x => x.Sessions).Sum(x => x.Duration);

        var totalLifetimeCredits = lifetimeMinutes / 45;

        var activeExams = exams.Where(x => !x.IsDone).ToList();

        var todayExams = activeExams.Where(x => x.Date.Date == today).ToList();

        // LOGIKA PRO SKLÁDANÝ TÝDENNÍ GRAF (Stacked Bar Chart)
        var weeklySeries = Enumerable
            .Range(0, 7)
            .Select(index =>
            {
                var day = sevenDaysAgo.AddDays(index);

                // Pro každý den najdeme všechna sezení a seskupíme je podle předmětu
                var segments = subjects
                    .SelectMany(s =>
                        s.Sessions.Where(sess => sess.CreatedAt.Date == day.Date)
                            .Select(sess => new
                            {
                                s.Name,
                                s.Color,
                                sess.Duration,
                            })
                    )
                    .GroupBy(x => new { x.Name, x.Color })
                    .Select(g => new ChartSegmentVm(
                        g.Key.Name,
                        g.Sum(x => x.Duration),
                        g.Key.Color
                    ))
                    .ToList();

                return new ChartItemVm(day.ToString("dd.MM"), segments);
            })
            .ToList();

        // LOGIKA PRO GRAF PODLE PŘEDMĚTŮ
        var subjectSeries = subjects
            .Select(subject =>
            {
                var totalMinutes = subject.Sessions.Sum(s => s.Duration);
                var segments = new List<ChartSegmentVm>();

                if (totalMinutes > 0)
                {
                    segments.Add(new ChartSegmentVm(subject.Name, totalMinutes, subject.Color));
                }

                return new ChartItemVm(subject.Name, segments);
            })
            .Where(x => x.TotalValue > 0)
            .OrderByDescending(x => x.TotalValue)
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

// POMOCNÉ TŘÍDY PRO GRAFY

public class ChartItemVm
{
    public string Label { get; set; }
    public List<ChartSegmentVm> Segments { get; set; }
    public int TotalValue => Segments.Sum(s => s.Value);

    public ChartItemVm(string label, List<ChartSegmentVm> segments)
    {
        Label = label;
        Segments = segments;
    }
}

public record ChartSegmentVm(string Label, int Value, string Color);

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

