using System.ComponentModel.DataAnnotations;

namespace BandirmaCepte.Models;

public class EmergencyContact
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }
}
