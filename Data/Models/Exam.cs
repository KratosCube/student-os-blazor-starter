using System.ComponentModel.DataAnnotations;

namespace StudentOs.Blazor.Data.Models;

public class Exam
{
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.Now.AddDays(1);

    [Required]
    public string Type { get; set; } = "confirmed";

    [Range(15, 480)]
    public int Duration { get; set; } = 90;

    public bool IsDone { get; set; }

    public int? SubjectId { get; set; }
    public Subject? Subject { get; set; }

    public string? LegacyName { get; set; }
}
