using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandirmaCepte.Models;

public class BusSchedule
{
    public int Id { get; set; }

    [Required]
    public int BusLineId { get; set; }

    [JsonIgnore]
    public BusLine? BusLine { get; set; }

    [Required(ErrorMessage = "Kalkış saati zorunludur.")]
    [MaxLength(10)]
    public string DepartureTime { get; set; } = string.Empty; 

    [MaxLength(100)]
    public string DeparturePoint { get; set; } = string.Empty; 
    [MaxLength(100)]
    public string ArrivalPoint { get; set; } = string.Empty; 
    [MaxLength(50)]
    public string DayType { get; set; } = "Hafta İçi"; 
}
