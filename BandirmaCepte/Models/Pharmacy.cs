using System.ComponentModel.DataAnnotations;

namespace BandirmaCepte.Models;

public class Pharmacy
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(250)]
    public string Address { get; set; } = string.Empty;

    public bool IsOnCall { get; set; }

    public string? Phone { get; set; }
}
