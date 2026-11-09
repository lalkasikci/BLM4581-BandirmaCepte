using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandirmaCepte.Models;

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty; // BCrypt hash

    public int RoleId { get; set; }
    public Role? Role { get; set; }

    // İlişkiler: Kullanıcının (özellikle yöneticinin) eklediği içerikler
    [JsonIgnore]
    public ICollection<News> News { get; set; } = new List<News>();

    [JsonIgnore]
    public ICollection<Earthquake> Earthquakes { get; set; } = new List<Earthquake>();

    [JsonIgnore]
    public ICollection<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();
}
