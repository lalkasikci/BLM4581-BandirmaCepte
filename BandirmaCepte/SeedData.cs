using Microsoft.EntityFrameworkCore;
using BandirmaCepte.Data;
using BandirmaCepte.Models;
using BCrypt.Net;

namespace BandirmaCepte;

public static class SeedData
{
    public static async Task EnsureCreatedAsync(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<BandirmaContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BandirmaContext>>();

        try
        {
            // Veritabanı tablolarını otomatik oluştur (migration yoksa EnsureCreated kullan)
            await ctx.Database.EnsureCreatedAsync();

            // Seed roles
            if (!await ctx.Roles.AnyAsync())
            {
                ctx.Roles.AddRange(
                    new Role { Id = 1, Name = "Admin" },
                    new Role { Id = 2, Name = "User" }
                );
                await ctx.SaveChangesAsync();
            }

            // Seed default admin and user
            if (!await ctx.Users.AnyAsync(u => u.Username == "admin"))
            {
                ctx.Users.Add(new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    RoleId = 1 // Admin
                });
            }

            if (!await ctx.Users.AnyAsync(u => u.Username == "user"))
            {
                ctx.Users.Add(new User
                {
                    Username = "user",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                    RoleId = 2 // User
                });
            }

            // Örnek Haber verileri (3. Hafta)
            if (!await ctx.News.AnyAsync())
            {
                ctx.News.AddRange(
                    new News
                    {
                        Title = "Bandırma Sahil Yenileme Projesi Başladı",
                        Content = "Bandırma sahil bandında çevre düzenlemesi ve yürüyüş yolları yenileme çalışmaları hız kazandı.",
                        PublishedAt = DateTime.UtcNow
                    },
                    new News
                    {
                        Title = "Bandırma Onyedi Eylül Üniversitesi Bahar Şenlikleri",
                        Content = "Üniversite kampüsünde düzenlenecek etkinlik ve konser programı açıklandı.",
                        PublishedAt = DateTime.UtcNow
                    }
                );
            }

            // Örnek Otobüs Hatları (3. Hafta)
            if (!await ctx.BusLines.AnyAsync())
            {
                ctx.BusLines.AddRange(
                    new BusLine
                    {
                        LineNumber = "H1",
                        Route = "Meydan - Liman - Paşabayır",
                        ScheduleCsv = "07:30,08:00,08:30,09:00,10:00,12:00,14:00,17:00,18:30"
                    },
                    new BusLine
                    {
                        LineNumber = "H2",
                        Route = "Meydan - Devlet Hastanesi - TOKİ",
                        ScheduleCsv = "07:15,07:45,08:15,09:00,11:00,13:00,15:00,17:30,19:00"
                    }
                );
            }

            // Örnek Acil Numaralar (3. Hafta)
            if (!await ctx.EmergencyContacts.AnyAsync())
            {
                ctx.EmergencyContacts.AddRange(
                    new EmergencyContact { Name = "Acil Çağrı Merkezi", PhoneNumber = "112", Description = "Tüm acil durumlar (Sağlık, Polis, İtfaiye)" },
                    new EmergencyContact { Name = "Bandırma Devlet Hastanesi", PhoneNumber = "0266 738 00 22", Description = "Santral / Randevu ve Bilgi" },
                    new EmergencyContact { Name = "Bandırma Belediyesi", PhoneNumber = "0266 711 11 11", Description = "Halk Masası / Çağrı Merkezi" }
                );
            }

            // Örnek Deprem Verileri (3. Hafta)
            if (!await ctx.Earthquakes.AnyAsync())
            {
                ctx.Earthquakes.AddRange(
                    new Earthquake { Magnitude = 3.2, Location = "Marmara Denizi - Bandırma Açıkları", Depth = 7.5, OccurrenceTime = DateTime.UtcNow.AddHours(-3) },
                    new Earthquake { Magnitude = 2.8, Location = "Erdek Körfezi", Depth = 10.2, OccurrenceTime = DateTime.UtcNow.AddHours(-12) },
                    new Earthquake { Magnitude = 3.9, Location = "Manyas Gölü Civarı", Depth = 8.1, OccurrenceTime = DateTime.UtcNow.AddDays(-1) }
                );
            }

            // Örnek Nöbetçi Eczane
            if (!await ctx.Pharmacies.AnyAsync())
            {
                ctx.Pharmacies.AddRange(
                    new Pharmacy { Name = "Liman Eczanesi", Address = "İnönü Cad. No:14/A Bandırma", IsOnCall = true, Phone = "0266 714 12 34" },
                    new Pharmacy { Name = "Şifa Eczanesi", Address = "Atatürk Cad. No:52 Bandırma", IsOnCall = false, Phone = "0266 718 56 78" }
                );
            }

            await ctx.SaveChangesAsync();
            logger.LogInformation("Veritabanı başlatıldı ve örnek veriler başarıyla yüklendi.");
        }
        catch (Exception ex)
        {
            logger.LogWarning("Veritabanı bağlantısı sırasında bir uyarı/hata oluştu (PostgreSQL henüz çalışmıyor olabilir): {Message}", ex.Message);
        }
    }
}
