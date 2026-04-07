using System.ComponentModel.DataAnnotations;

namespace StudentOs.Blazor.Data.Models;

public class StudySession
{
    public int Id { get; set; }

    [Range(5, 600)]
    public int Duration { get; set; } = 45;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required]
    public int SubjectId { get; set; }

    public Subject Subject { get; set; } = default!;
}
