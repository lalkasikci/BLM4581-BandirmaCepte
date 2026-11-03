using System.ComponentModel.DataAnnotations;

namespace BandirmaCepte.Models;

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty; // BCrypt hash

    public int RoleId { get; set; }
    public Role? Role { get; set; }
}
