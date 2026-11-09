using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandirmaCepte.Models;

public class EmergencyContact
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Kurum / Hizmet adı zorunludur.")]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon numarası zorunludur.")]
    [MaxLength(50)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Category { get; set; } // örn: "Acil Çağrı", "Sağlık", "Güvenlik", "Belediye", "Altyapı & Arıza"

    [MaxLength(250)]
    public string? Description { get; set; }

    // İlişki: Numarayı ekleyen/düzenleyen yönetici (User)
    public int? UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}
