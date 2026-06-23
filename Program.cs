using System.Data;
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
    // Při startu aplikace se vytvoří SQLite databáze, pokud ještě neexistuje.
    await db.Database.EnsureCreatedAsync();
    // Tato kontrola umožní spustit aplikaci i nad starší lokální databází bez sloupce Note.
    await EnsureExamNoteColumnAsync(db);
    // Tato kontrola umožní spustit aplikaci i nad starší lokální databází bez sloupce IsArchived.
    await EnsureSubjectArchiveColumnAsync(db);
    // Pokud je databáze prázdná, vloží se ukázková data.
    if (!db.Subjects.Any())
    {
        var dma = new Subject { Name = "DMA", Color = "#f59e0b" };
        var uur = new Subject { Name = "UUR", Color = "#6366f1" };
        var alg = new Subject { Name = "ALG", Color = "#10b981" };
        var osy = new Subject { Name = "OSY", Color = "#f43f5e" };
        var pos = new Subject { Name = "POS", Color = "#0ea5e9" };
        var web = new Subject { Name = "WEB", Color = "#8b5cf6" };

        db.Subjects.AddRange(dma, uur, alg, osy, pos, web);
        await db.SaveChangesAsync();

        var now = DateTime.Now;

        db.Exams.AddRange(
            new Exam
            {
                SubjectId = uur.Id,
                Date = now.Date.AddHours(13),
                Type = "confirmed",
                Duration = 90,
                Note = "Kontrola semestrální práce a druhá prezentace projektu.",
                IsDone = false,
            },
            new Exam
            {
                SubjectId = dma.Id,
                Date = now.Date.AddDays(1).AddHours(9),
                Type = "confirmed",
                Duration = 120,
                Note = "Zápočet z diskrétní matematiky. Témata: relace, grafy, kombinatorika.",
                IsDone = false,
            },
            new Exam
            {
                SubjectId = alg.Id,
                Date = now.Date.AddDays(3).AddHours(10),
                Type = "potential",
                Duration = 90,
                Note = "Možný termín testu. Procvičit složitost algoritmů a rekurzi.",
                IsDone = false,
            },
            new Exam
            {
                SubjectId = osy.Id,
                Date = now.Date.AddDays(6).AddHours(8),
                Type = "confirmed",
                Duration = 75,
                Note = "Zkouška z operačních systémů. Procesy, vlákna, plánování, synchronizace.",
                IsDone = false,
            },
            new Exam
            {
                SubjectId = pos.Id,
                Date = now.Date.AddDays(9).AddHours(14),
                Type = "potential",
                Duration = 60,
                Note = "Odevzdání části projektu a konzultace návrhu.",
                IsDone = false,
            },
            new Exam
            {
                SubjectId = web.Id,
                Date = now.Date.AddDays(13).AddHours(11),
                Type = "confirmed",
                Duration = 90,
                Note = "Prezentace webové aplikace. Připravit screenshoty a krátký popis funkcí.",
                IsDone = false,
            },
            new Exam
            {
                SubjectId = dma.Id,
                Date = now.Date.AddDays(-5).AddHours(12),
                Type = "confirmed",
                Duration = 60,
                Note = "První průběžný test.",
                IsDone = true,
            },
            new Exam
            {
                SubjectId = uur.Id,
                Date = now.Date.AddDays(-2).AddHours(15),
                Type = "confirmed",
                Duration = 45,
                Note = "Kontrola návrhu GUI.",
                IsDone = true,
            }
        );

        db.StudySessions.AddRange(
            new StudySession
            {
                SubjectId = uur.Id,
                Duration = 45,
                CreatedAt = now.Date.AddDays(-27).AddHours(18),
            },
            new StudySession
            {
                SubjectId = dma.Id,
                Duration = 60,
                CreatedAt = now.Date.AddDays(-26).AddHours(17),
            },
            new StudySession
            {
                SubjectId = alg.Id,
                Duration = 90,
                CreatedAt = now.Date.AddDays(-25).AddHours(19),
            },
            new StudySession
            {
                SubjectId = osy.Id,
                Duration = 50,
                CreatedAt = now.Date.AddDays(-24).AddHours(16),
            },
            new StudySession
            {
                SubjectId = web.Id,
                Duration = 80,
                CreatedAt = now.Date.AddDays(-20).AddHours(18),
            },
            new StudySession
            {
                SubjectId = pos.Id,
                Duration = 40,
                CreatedAt = now.Date.AddDays(-19).AddHours(20),
            },
            new StudySession
            {
                SubjectId = dma.Id,
                Duration = 70,
                CreatedAt = now.Date.AddDays(-18).AddHours(17),
            },
            new StudySession
            {
                SubjectId = uur.Id,
                Duration = 55,
                CreatedAt = now.Date.AddDays(-17).AddHours(19),
            },
            new StudySession
            {
                SubjectId = alg.Id,
                Duration = 120,
                CreatedAt = now.Date.AddDays(-13).AddHours(18),
            },
            new StudySession
            {
                SubjectId = osy.Id,
                Duration = 90,
                CreatedAt = now.Date.AddDays(-12).AddHours(16),
            },
            new StudySession
            {
                SubjectId = dma.Id,
                Duration = 45,
                CreatedAt = now.Date.AddDays(-11).AddHours(20),
            },
            new StudySession
            {
                SubjectId = web.Id,
                Duration = 75,
                CreatedAt = now.Date.AddDays(-10).AddHours(18),
            },
            new StudySession
            {
                SubjectId = uur.Id,
                Duration = 60,
                CreatedAt = now.Date.AddDays(-6).AddHours(17),
            },
            new StudySession
            {
                SubjectId = alg.Id,
                Duration = 90,
                CreatedAt = now.Date.AddDays(-5).AddHours(18),
            },
            new StudySession
            {
                SubjectId = dma.Id,
                Duration = 45,
                CreatedAt = now.Date.AddDays(-4).AddHours(19),
            },
            new StudySession
            {
                SubjectId = osy.Id,
                Duration = 100,
                CreatedAt = now.Date.AddDays(-3).AddHours(16),
            },
            new StudySession
            {
                SubjectId = pos.Id,
                Duration = 50,
                CreatedAt = now.Date.AddDays(-2).AddHours(20),
            },
            new StudySession
            {
                SubjectId = web.Id,
                Duration = 80,
                CreatedAt = now.Date.AddDays(-1).AddHours(18),
            },
            new StudySession
            {
                SubjectId = uur.Id,
                Duration = 45,
                CreatedAt = now.Date.AddHours(-5),
            },
            new StudySession
            {
                SubjectId = dma.Id,
                Duration = 60,
                CreatedAt = now.Date.AddHours(-3),
            },
            new StudySession
            {
                SubjectId = alg.Id,
                Duration = 30,
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

// Kontroluje existenci sloupce Note v tabulce Exams a případně ho doplní.
static async Task EnsureExamNoteColumnAsync(AppDbContext db)
{
    var connection = db.Database.GetDbConnection();

    if (connection.State != ConnectionState.Open)
        await connection.OpenAsync();

    var columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    await using (var command = connection.CreateCommand())
    {
        command.CommandText = "PRAGMA table_info('Exams')";

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            columns.Add(reader.GetString(1));
        }
    }

    if (!columns.Contains("Note"))
    {
        await db.Database.ExecuteSqlRawAsync("ALTER TABLE Exams ADD COLUMN Note TEXT");
    }
}

// Kontroluje existenci sloupce IsArchived v tabulce Subjects a případně ho doplní.
static async Task EnsureSubjectArchiveColumnAsync(AppDbContext db)
{
    var connection = db.Database.GetDbConnection();

    if (connection.State != ConnectionState.Open)
        await connection.OpenAsync();

    var columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    await using (var command = connection.CreateCommand())
    {
        command.CommandText = "PRAGMA table_info('Subjects')";

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            columns.Add(reader.GetString(1));
        }
    }

    if (!columns.Contains("IsArchived"))
    {
        await db.Database.ExecuteSqlRawAsync(
            "ALTER TABLE Subjects ADD COLUMN IsArchived INTEGER NOT NULL DEFAULT 0"
        );
    }
}

app.Run();
