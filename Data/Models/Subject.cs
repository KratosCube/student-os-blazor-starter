using System.ComponentModel.DataAnnotations;

namespace StudentOs.Blazor.Data.Models;

public class Subject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Název předmětu je povinný.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Název musí mít 2 až 50 znaků.")]
    public string Name { get; set; } = string.Empty;

    // Barva předmětu se používá v grafech, kartách a přehledech
    [Required]
    [RegularExpression("^#([A-Fa-f0-9]{6})$", ErrorMessage = "Barva musí být validní HEX kód.")]
    public string Color { get; set; } = "#6366f1";

    public List<Exam> Exams { get; set; } = new();
    public List<StudySession> Sessions { get; set; } = new();
}
