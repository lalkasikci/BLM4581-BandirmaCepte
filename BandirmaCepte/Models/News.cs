using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandirmaCepte.Models;

public class News
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Haber başlığı zorunludur.")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Haber içeriği zorunludur.")]
    [MaxLength(5000, ErrorMessage = "Haber içeriği en fazla 5000 karakter olabilir.")]
    public string Content { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? Source { get; set; } // Kaynak: örn. "Bandırma Belediyesi", "BANÜ Basın"

    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

    public bool IsPublished { get; set; } = true;

    // İlişki: Haberi ekleyen/yöneten kullanıcı (User)
    public int? UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}
