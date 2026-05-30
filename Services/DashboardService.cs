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

        var subjects = await db
            .Subjects.Include(x => x.Exams.Where(e => !e.IsDone).OrderBy(e => e.Date))
            .Include(x => x.Sessions)
            .OrderBy(x => x.Name)
            .ToListAsync();

        var exams = await db.Exams.Include(x => x.Subject).OrderBy(x => x.Date).ToListAsync();

        var now = DateTime.Now;
        var today = now.Date;

        var sevenDaysAgo = today.AddDays(-6);
        var thirtyDaysAgo = today.AddDays(-29);
        var upcomingLimit = today.AddDays(14);

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

        var activeExams = exams.Where(x => !x.IsDone).OrderBy(x => x.Date).ToList();

        var todayExams = activeExams.Where(x => x.Date.Date == today).OrderBy(x => x.Date).ToList();

        var upcomingExams = activeExams
            .Where(x => x.Date.Date > today && x.Date.Date <= upcomingLimit)
            .OrderBy(x => x.Date)
            .ToList();

        var weeklySeries = BuildDailySeries(subjects, sevenDaysAgo, today);
        var last30DaysSeries = BuildDailySeries(subjects, thirtyDaysAgo, today);
        var allTimeSeries = BuildAllTimeSeries(subjects);
        var subjectSeries = BuildSubjectSeries(subjects);

        return new DashboardVm(
            subjects,
            exams,
            activeExams,
            todayExams,
            upcomingExams,
            todayTotalMinutes,
            weekTotalMinutes,
            lifetimeMinutes,
            totalLifetimeCredits,
            weeklySeries,
            last30DaysSeries,
            allTimeSeries,
            subjectSeries
        );
    }

    private static List<ChartItemVm> BuildDailySeries(
        List<Subject> subjects,
        DateTime startDate,
        DateTime endDate
    )
    {
        var days = Math.Max(1, (endDate.Date - startDate.Date).Days + 1);

        return Enumerable
            .Range(0, days)
            .Select(index =>
            {
                var day = startDate.Date.AddDays(index);

                var segments = subjects
                    .SelectMany(subject =>
                        subject
                            .Sessions.Where(session => session.CreatedAt.Date == day)
                            .Select(session => new
                            {
                                subject.Name,
                                subject.Color,
                                session.Duration,
                            })
                    )
                    .GroupBy(x => new { x.Name, x.Color })
                    .Select(group => new ChartSegmentVm(
                        group.Key.Name,
                        group.Sum(x => x.Duration),
                        group.Key.Color
                    ))
                    .ToList();

                return new ChartItemVm(day.ToString("dd.MM"), segments);
            })
            .ToList();
    }

    private static List<ChartItemVm> BuildAllTimeSeries(List<Subject> subjects)
    {
        var sessions = subjects
            .SelectMany(subject =>
                subject.Sessions.Select(session => new
                {
                    subject.Name,
                    subject.Color,
                    session.Duration,
                    Month = new DateTime(session.CreatedAt.Year, session.CreatedAt.Month, 1),
                })
            )
            .ToList();

        if (sessions.Count == 0)
            return new List<ChartItemVm>();

        return sessions
            .GroupBy(x => x.Month)
            .OrderBy(group => group.Key)
            .Select(monthGroup =>
            {
                var segments = monthGroup
                    .GroupBy(x => new { x.Name, x.Color })
                    .Select(subjectGroup => new ChartSegmentVm(
                        subjectGroup.Key.Name,
                        subjectGroup.Sum(x => x.Duration),
                        subjectGroup.Key.Color
                    ))
                    .ToList();

                return new ChartItemVm(monthGroup.Key.ToString("MM.yyyy"), segments);
            })
            .ToList();
    }

    private static List<ChartItemVm> BuildSubjectSeries(List<Subject> subjects)
    {
        return subjects
            .Select(subject =>
            {
                var totalMinutes = subject.Sessions.Sum(session => session.Duration);
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
    }
}

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
    List<Exam> UpcomingExams,
    int TodayTotalMinutes,
    int WeekTotalMinutes,
    int LifetimeMinutes,
    int TotalLifetimeCredits,
    List<ChartItemVm> WeeklySeries,
    List<ChartItemVm> Last30DaysSeries,
    List<ChartItemVm> AllTimeSeries,
    List<ChartItemVm> SubjectSeries
);
