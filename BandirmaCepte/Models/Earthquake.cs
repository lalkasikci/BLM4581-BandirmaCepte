using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandirmaCepte.Models;

public class Earthquake
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Büyüklük değeri zorunludur.")]
    public double Magnitude { get; set; }

    [Required(ErrorMessage = "Konum bilgisi zorunludur.")]
    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Derinlik bilgisi zorunludur.")]
    public double Depth { get; set; }

    public DateTime OccurrenceTime { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? Source { get; set; } = "Kandilli Rasathanesi";

    
    public int? UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}
