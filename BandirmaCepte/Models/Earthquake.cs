using System.ComponentModel.DataAnnotations;

namespace BandirmaCepte.Models;

public class Earthquake
{
    public int Id { get; set; }

    [Required]
    public double Magnitude { get; set; }

    [Required, MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    public double Depth { get; set; }

    public DateTime OccurrenceTime { get; set; } = DateTime.UtcNow;
}
