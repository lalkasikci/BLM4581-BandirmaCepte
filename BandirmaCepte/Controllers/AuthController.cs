using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BandirmaCepte.Data;
using BandirmaCepte.Models;

namespace BandirmaCepte.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly BandirmaContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(BandirmaContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public record RegisterDto(
        [Required, StringLength(100, MinimumLength = 3)] string Username,
        [Required, MinLength(8)] string Password);
    public record LoginDto(string Username, string Password);
    public record AuthResponseDto(string Token, string Username, string Role);

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var username = dto.Username.Trim();
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest(new { message = "Kullanıcı adı ve şifre zorunludur." });
        }

        if (username.Length < 3 || username.Length > 100)
        {
            return BadRequest(new { message = "Kullanıcı adı 3 ile 100 karakter arasında olmalıdır." });
        }

        if (dto.Password.Length < 8)
        {
            return BadRequest(new { message = "Şifre en az 8 karakter olmalıdır." });
        }

        if (await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
        {
            return BadRequest(new { message = "Bu kullanıcı adı zaten kullanılıyor." });
        }

        var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        int roleId = userRole?.Id ?? 2;

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RoleId = roleId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Kayıt başarılı. Giriş yapabilirsiniz." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username.ToLower() == dto.Username.ToLower());

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre." });
        }

        var roleName = user.Role?.Name ?? "User";
        var token = GenerateJwtToken(user, roleName);

        return Ok(new AuthResponseDto(token, user.Username, roleName));
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var username = User.Identity?.Name;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new { username, role });
    }

    private string GenerateJwtToken(User user, string roleName)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, roleName)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
