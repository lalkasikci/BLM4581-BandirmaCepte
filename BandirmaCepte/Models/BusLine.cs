using System.ComponentModel.DataAnnotations;

namespace BandirmaCepte.Models;

public class BusLine
{
    public int Id { get; set; }

    [Required, MaxLength(10)]
    public string LineNumber { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Route { get; set; } = string.Empty;

    // CSV of departure times, e.g., "08:00,09:30,11:00"
    public string ScheduleCsv { get; set; } = string.Empty;
}
