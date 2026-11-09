using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandirmaCepte.Models;

public class BusLine
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Hat kodu/numarası zorunludur.")]
    [MaxLength(20)]
    public string LineNumber { get; set; } = string.Empty; // örn: "H1", "10K", "17M"

    [Required(ErrorMessage = "Güzergâh bilgisi zorunludur.")]
    [MaxLength(200)]
    public string Route { get; set; } = string.Empty; // örn: "Meydan - Paşabayır - 600 Evler"

    [MaxLength(250)]
    public string? Description { get; set; } // örn: "Şehir içi ana ring hattı"

    // Hızlı erişim ve geriye dönük uyumluluk için virgülle ayrılmış saatler (örn: "07:30,08:00,08:30")
    public string ScheduleCsv { get; set; } = string.Empty;

    // 1-to-many ilişki: Bir hatta ait birden çok sefer/hareket saati
    public ICollection<BusSchedule> Schedules { get; set; } = new List<BusSchedule>();
}
