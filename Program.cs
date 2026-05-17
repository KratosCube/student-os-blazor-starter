using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using StudentOs.Blazor.Components;
using StudentOs.Blazor.Data;
using StudentOs.Blazor.Data.Models;
using StudentOs.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite("Data Source=student-os.db")
);

builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<ExamService>();
builder.Services.AddScoped<StudySessionService>();
builder.Services.AddScoped<BrowserStorageService>();
builder.Services.AddScoped<ThemeService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    await using var db = await factory.CreateDbContextAsync();
    await db.Database.EnsureCreatedAsync();

    if (!db.Subjects.Any())
    {
        var dma = new Subject { Name = "DMA", Color = "#f59e0b" };
        var uur = new Subject { Name = "UUR", Color = "#6366f1" };
        var alg = new Subject { Name = "ALG", Color = "#10b981" };
        var osy = new Subject { Name = "OSY", Color = "#f43f5e" };

        db.Subjects.AddRange(dma, uur, alg, osy);
        await db.SaveChangesAsync();

        var now = DateTime.Now;

        db.Exams.AddRange(
            new Exam
            {
                SubjectId = dma.Id,
                Date = now.Date.AddHours(9),
                Type = "confirmed",
                IsDone = false,
            },
            new Exam
            {
                SubjectId = uur.Id,
                Date = now.Date.AddDays(1).AddHours(13),
                Type = "potential",
                Duration = 120,
                IsDone = false,
            },
            new Exam
            {
                SubjectId = alg.Id,
                Date = now.Date.AddDays(3).AddHours(8),
                Type = "confirmed",
                Duration = 60,
                IsDone = false,
            },
            new Exam
            {
                SubjectId = osy.Id,
                Date = now.Date.AddDays(-2).AddHours(10),
                Type = "confirmed",
                Duration = 75,
                IsDone = true,
            }
        );

        db.StudySessions.AddRange(
            new StudySession
            {
                SubjectId = dma.Id,
                Duration = 45,
                CreatedAt = now.Date.AddDays(-6).AddHours(18),
            },
            new StudySession
            {
                SubjectId = dma.Id,
                Duration = 60,
                CreatedAt = now.Date.AddDays(-4).AddHours(19),
            },
            new StudySession
            {
                SubjectId = uur.Id,
                Duration = 30,
                CreatedAt = now.Date.AddDays(-5).AddHours(16),
            },
            new StudySession
            {
                SubjectId = uur.Id,
                Duration = 90,
                CreatedAt = now.Date.AddDays(-2).AddHours(20),
            },
            new StudySession
            {
                SubjectId = alg.Id,
                Duration = 120,
                CreatedAt = now.Date.AddDays(-1).AddHours(17),
            },
            new StudySession
            {
                SubjectId = alg.Id,
                Duration = 50,
                CreatedAt = now.Date.AddHours(-3),
            },
            new StudySession
            {
                SubjectId = osy.Id,
                Duration = 80,
                CreatedAt = now.Date.AddDays(-3).AddHours(14),
            },
            new StudySession
            {
                SubjectId = osy.Id,
                Duration = 40,
                CreatedAt = now.Date.AddHours(-1),
            }
        );

        await db.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapStaticAssets();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
