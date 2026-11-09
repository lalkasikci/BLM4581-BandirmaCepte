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
            // Şemanın güncel olup olmadığını kontrol et
            bool isSchemaValid = false;
            try
            {
                // Yeni tabloların ve kolonların varlığını sorgula
                _ = await ctx.BusSchedules.FirstOrDefaultAsync();
                _ = await ctx.News.Select(n => n.Source).FirstOrDefaultAsync();
                isSchemaValid = true;
            }
            catch
            {
                isSchemaValid = false;
            }

            if (!isSchemaValid)
            {
                logger.LogInformation("Veritabanı şeması güncelleniyor (3. Hafta modelleri oluşturuluyor)...");
                await ctx.Database.EnsureDeletedAsync();
                await ctx.Database.EnsureCreatedAsync();
            }
            else
            {
                await ctx.Database.EnsureCreatedAsync();
            }

            // 1. Rolleri Tohumla (Seed Roles)
            if (!await ctx.Roles.AnyAsync())
            {
                ctx.Roles.AddRange(
                    new Role { Id = 1, Name = "Admin" },
                    new Role { Id = 2, Name = "User" }
                );
                await ctx.SaveChangesAsync();
            }

            // 2. Kullanıcıları Tohumla (Admin ve Normal Kullanıcı)
            User? adminUser = await ctx.Users.FirstOrDefaultAsync(u => u.Username == "admin");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    RoleId = 1 // Admin
                };
                ctx.Users.Add(adminUser);
                await ctx.SaveChangesAsync();
            }

            if (!await ctx.Users.AnyAsync(u => u.Username == "user"))
            {
                ctx.Users.Add(new User
                {
                    Username = "user",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                    RoleId = 2 // User
                });
                await ctx.SaveChangesAsync();
            }

            // 3. Örnek Haber Verileri (Week 3 - Admin ile ilişkili)
            if (!await ctx.News.AnyAsync())
            {
                ctx.News.AddRange(
                    new News
                    {
                        Title = "Bandırma Sahil Kordonu Yenileme Projesi Başladı",
                        Content = "Bandırma sahil bandında çevre düzenlemesi, bisiklet yolları ve yürüyüş parkurlarının yenilenmesi için çalışmalar aralıksız sürüyor. Proje tamamlandığında ilçe sakinlerine modern bir sahil alanı sunulacak.",
                        Source = "Bandırma Belediyesi",
                        PublishedAt = DateTime.UtcNow.AddDays(-1),
                        IsPublished = true,
                        UserId = adminUser.Id
                    },
                    new News
                    {
                        Title = "Bandırma Onyedi Eylül Üniversitesi Bahar Şenlikleri Programı Açıklandı",
                        Content = "BANÜ merkez kampüsünde 15-18 Mayıs tarihleri arasında gerçekleştirilecek bahar şenlikleri kapsamında spor turnuvaları, atölyeler ve ünlü sanatçıların katılımıyla konserler düzenlenecek.",
                        Source = "BANÜ Rektörlüğü",
                        PublishedAt = DateTime.UtcNow.AddDays(-2),
                        IsPublished = true,
                        UserId = adminUser.Id
                    },
                    new News
                    {
                        Title = "Bandırma Kuşcenneti Milli Parkı Ziyaretçi Rekoruna Koşuyor",
                        Content = "Bahar göç döneminde yüzlerce farklı kuş türüne ev sahipliği yapan Bandırma Kuşcenneti Milli Parkı, hafta sonu yerli ve yabancı doğaseverlerin yoğun ilgisiyle karşılaştı.",
                        Source = "Doğa Koruma ve Milli Parklar",
                        PublishedAt = DateTime.UtcNow.AddDays(-3),
                        IsPublished = true,
                        UserId = adminUser.Id
                    },
                    new News
                    {
                        Title = "Bandırmaspor TFF 1. Lig Maçı Biletleri Satışa Sunuldu",
                        Content = "Bu hafta sonu 17 Eylül Stadyumu'nda oynanacak kritik lig mücadelesi öncesinde Passolig ve stadyum gişelerinden bilet satışları başladı. Taraftarların yoğun ilgi göstermesi bekleniyor.",
                        Source = "Bandırmaspor Kulübü",
                        PublishedAt = DateTime.UtcNow.AddDays(-4),
                        IsPublished = true,
                        UserId = adminUser.Id
                    }
                );
                await ctx.SaveChangesAsync();
            }

            // 4. Örnek Otobüs Hatları ve İlişkili Sefer Saatleri (Week 3 - 1:N İlişki)
            if (!await ctx.BusLines.AnyAsync())
            {
                var line1 = new BusLine
                {
                    LineNumber = "H1",
                    Route = "Meydan - Liman - Paşabayır - 600 Evler",
                    Description = "Şehir içi ana ring hattı; ana duraklar ve sahil boyu",
                    ScheduleCsv = "07:30, 08:00, 08:30, 09:15, 10:30, 12:00, 14:00, 16:30, 17:15, 18:00, 19:30, 21:00",
                    Schedules = new List<BusSchedule>
                    {
                        new() { DepartureTime = "07:30", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Hafta İçi" },
                        new() { DepartureTime = "08:00", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Hafta İçi" },
                        new() { DepartureTime = "08:30", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Hafta İçi" },
                        new() { DepartureTime = "09:15", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Her Gün" },
                        new() { DepartureTime = "10:30", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Her Gün" },
                        new() { DepartureTime = "12:00", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Her Gün" },
                        new() { DepartureTime = "14:00", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Her Gün" },
                        new() { DepartureTime = "16:30", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Hafta İçi" },
                        new() { DepartureTime = "17:15", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Hafta İçi" },
                        new() { DepartureTime = "18:00", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Hafta İçi" },
                        new() { DepartureTime = "19:30", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Her Gün" },
                        new() { DepartureTime = "21:00", DeparturePoint = "Meydan", ArrivalPoint = "600 Evler", DayType = "Her Gün" }
                    }
                };

                var line2 = new BusLine
                {
                    LineNumber = "H2",
                    Route = "Meydan - Devlet Hastanesi - TOKİ Konutları",
                    Description = "Sağlık ve toplu konut ekspres hattı",
                    ScheduleCsv = "07:15, 07:45, 08:15, 09:00, 11:00, 13:00, 15:00, 16:45, 17:30, 18:45, 20:00",
                    Schedules = new List<BusSchedule>
                    {
                        new() { DepartureTime = "07:15", DeparturePoint = "Meydan", ArrivalPoint = "TOKİ", DayType = "Hafta İçi" },
                        new() { DepartureTime = "07:45", DeparturePoint = "Meydan", ArrivalPoint = "TOKİ", DayType = "Hafta İçi" },
                        new() { DepartureTime = "08:15", DeparturePoint = "Meydan", ArrivalPoint = "TOKİ", DayType = "Hafta İçi" },
                        new() { DepartureTime = "09:00", DeparturePoint = "Meydan", ArrivalPoint = "TOKİ", DayType = "Her Gün" },
                        new() { DepartureTime = "11:00", DeparturePoint = "Meydan", ArrivalPoint = "TOKİ", DayType = "Her Gün" },
                        new() { DepartureTime = "13:00", DeparturePoint = "Meydan", ArrivalPoint = "TOKİ", DayType = "Her Gün" },
                        new() { DepartureTime = "15:00", DeparturePoint = "Meydan", ArrivalPoint = "TOKİ", DayType = "Her Gün" },
                        new() { DepartureTime = "16:45", DeparturePoint = "Meydan", ArrivalPoint = "TOKİ", DayType = "Hafta İçi" },
                        new() { DepartureTime = "17:30", DeparturePoint = "Meydan", ArrivalPoint = "TOKİ", DayType = "Hafta İçi" },
                        new() { DepartureTime = "18:45", DeparturePoint = "Meydan", ArrivalPoint = "TOKİ", DayType = "Her Gün" },
                        new() { DepartureTime = "20:00", DeparturePoint = "Meydan", ArrivalPoint = "TOKİ", DayType = "Her Gün" }
                    }
                };

                var line3 = new BusLine
                {
                    LineNumber = "H3",
                    Route = "İDO İskelesi - Otogar - BANÜ Merkez Kampüs",
                    Description = "Üniversite ve şehirlerarası ulaşım transfer hattı",
                    ScheduleCsv = "07:45, 08:15, 08:45, 09:30, 11:15, 13:30, 15:15, 16:30, 17:15, 18:15, 20:15",
                    Schedules = new List<BusSchedule>
                    {
                        new() { DepartureTime = "07:45", DeparturePoint = "İDO İskelesi", ArrivalPoint = "Kampüs", DayType = "Hafta İçi" },
                        new() { DepartureTime = "08:15", DeparturePoint = "İDO İskelesi", ArrivalPoint = "Kampüs", DayType = "Hafta İçi" },
                        new() { DepartureTime = "08:45", DeparturePoint = "İDO İskelesi", ArrivalPoint = "Kampüs", DayType = "Hafta İçi" },
                        new() { DepartureTime = "09:30", DeparturePoint = "İDO İskelesi", ArrivalPoint = "Kampüs", DayType = "Her Gün" },
                        new() { DepartureTime = "11:15", DeparturePoint = "İDO İskelesi", ArrivalPoint = "Kampüs", DayType = "Her Gün" },
                        new() { DepartureTime = "13:30", DeparturePoint = "İDO İskelesi", ArrivalPoint = "Kampüs", DayType = "Her Gün" },
                        new() { DepartureTime = "15:15", DeparturePoint = "İDO İskelesi", ArrivalPoint = "Kampüs", DayType = "Her Gün" },
                        new() { DepartureTime = "16:30", DeparturePoint = "İDO İskelesi", ArrivalPoint = "Kampüs", DayType = "Hafta İçi" },
                        new() { DepartureTime = "17:15", DeparturePoint = "İDO İskelesi", ArrivalPoint = "Kampüs", DayType = "Hafta İçi" },
                        new() { DepartureTime = "18:15", DeparturePoint = "İDO İskelesi", ArrivalPoint = "Kampüs", DayType = "Her Gün" },
                        new() { DepartureTime = "20:15", DeparturePoint = "İDO İskelesi", ArrivalPoint = "Kampüs", DayType = "Her Gün" }
                    }
                };

                ctx.BusLines.AddRange(line1, line2, line3);
                await ctx.SaveChangesAsync();
            }

            // 5. Örnek Acil Numaralar (Week 3 - Admin ile ilişkili)
            if (!await ctx.EmergencyContacts.AnyAsync())
            {
                ctx.EmergencyContacts.AddRange(
                    new EmergencyContact
                    {
                        Name = "112 Acil Çağrı Merkezi",
                        PhoneNumber = "112",
                        Category = "Acil Durum",
                        Description = "Tüm acil durumlar (Sağlık, Polis, Jandarma, İtfaiye, Orman Yangın)",
                        UserId = adminUser.Id
                    },
                    new EmergencyContact
                    {
                        Name = "Bandırma Eğitim ve Araştırma Hastanesi",
                        PhoneNumber = "0266 738 00 22",
                        Category = "Sağlık",
                        Description = "Santral, Randevu ve Acil Servis Bilgilendirme",
                        UserId = adminUser.Id
                    },
                    new EmergencyContact
                    {
                        Name = "Bandırma İlçe Emniyet Müdürlüğü",
                        PhoneNumber = "0266 714 11 00",
                        Category = "Güvenlik",
                        Description = "Polis Merkezi Amirliği ve Güvenlik Hizmetleri",
                        UserId = adminUser.Id
                    },
                    new EmergencyContact
                    {
                        Name = "Bandırma Belediyesi Çağrı Merkezi",
                        PhoneNumber = "0266 711 11 11",
                        Category = "Belediye",
                        Description = "Halk Masası, İstek, Şikayet ve Zabıta Hizmetleri",
                        UserId = adminUser.Id
                    },
                    new EmergencyContact
                    {
                        Name = "Bandırma İtfaiye Grup Amirliği",
                        PhoneNumber = "0266 718 11 10",
                        Category = "İtfaiye",
                        Description = "Yangın, Kaza ve Arama-Kurtarma Hizmetleri",
                        UserId = adminUser.Id
                    },
                    new EmergencyContact
                    {
                        Name = "BASKİ Su Arıza ve Şikayet",
                        PhoneNumber = "185",
                        Category = "Altyapı",
                        Description = "Balıkesir Su ve Kanalizasyon İdaresi Bandırma Şubesi",
                        UserId = adminUser.Id
                    },
                    new EmergencyContact
                    {
                        Name = "UEDAŞ Elektrik Arıza",
                        PhoneNumber = "186",
                        Category = "Altyapı",
                        Description = "Uludağ Elektrik Dağıtım A.Ş. Arıza İhbar Hattı",
                        UserId = adminUser.Id
                    },
                    new EmergencyContact
                    {
                        Name = "Aksa Doğalgaz Acil",
                        PhoneNumber = "187",
                        Category = "Altyapı",
                        Description = "7/24 Doğalgaz Acil Müdahale ve İhbar Hattı",
                        UserId = adminUser.Id
                    }
                );
                await ctx.SaveChangesAsync();
            }

            // 6. Örnek Deprem Verileri (Week 3 - Admin ile ilişkili)
            if (!await ctx.Earthquakes.AnyAsync())
            {
                ctx.Earthquakes.AddRange(
                    new Earthquake
                    {
                        Magnitude = 3.4,
                        Location = "Marmara Denizi - Bandırma Açıkları (Balıkesir)",
                        Depth = 8.2,
                        OccurrenceTime = DateTime.UtcNow.AddHours(-3),
                        Source = "Kandilli Rasathanesi",
                        UserId = adminUser.Id
                    },
                    new Earthquake
                    {
                        Magnitude = 2.9,
                        Location = "Erdek Körfezi (Balıkesir)",
                        Depth = 10.5,
                        OccurrenceTime = DateTime.UtcNow.AddHours(-11),
                        Source = "Kandilli Rasathanesi",
                        UserId = adminUser.Id
                    },
                    new Earthquake
                    {
                        Magnitude = 3.8,
                        Location = "Manyas Gölü Civarı (Balıkesir)",
                        Depth = 7.1,
                        OccurrenceTime = DateTime.UtcNow.AddDays(-1).AddHours(-4),
                        Source = "AFAD",
                        UserId = adminUser.Id
                    },
                    new Earthquake
                    {
                        Magnitude = 2.2,
                        Location = "Bandırma - Edincik Açıkları (Balıkesir)",
                        Depth = 9.0,
                        OccurrenceTime = DateTime.UtcNow.AddDays(-2),
                        Source = "Kandilli Rasathanesi",
                        UserId = adminUser.Id
                    },
                    new Earthquake
                    {
                        Magnitude = 2.6,
                        Location = "Kapıdağ Yarımadası - Marmara Denizi",
                        Depth = 11.4,
                        OccurrenceTime = DateTime.UtcNow.AddDays(-3),
                        Source = "AFAD",
                        UserId = adminUser.Id
                    }
                );
                await ctx.SaveChangesAsync();
            }

            // 7. Örnek Nöbetçi Eczaneler
            if (!await ctx.Pharmacies.AnyAsync())
            {
                ctx.Pharmacies.AddRange(
                    new Pharmacy
                    {
                        Name = "Liman Eczanesi",
                        Address = "İnönü Cad. No:14/A Bandırma (Liman Karşısı)",
                        IsOnCall = true,
                        Phone = "0266 714 12 34"
                    },
                    new Pharmacy
                    {
                        Name = "Merkez Kent Eczanesi",
                        Address = "Kaşif Acar Cad. No:28 Bandırma",
                        IsOnCall = true,
                        Phone = "0266 715 90 12"
                    },
                    new Pharmacy
                    {
                        Name = "Şifa Eczanesi",
                        Address = "Atatürk Cad. No:52 Bandırma",
                        IsOnCall = false,
                        Phone = "0266 718 56 78"
                    }
                );
                await ctx.SaveChangesAsync();
            }

            logger.LogInformation("Veritabanı başlatıldı ve 3. Hafta modellerine ait örnek veriler başarıyla yüklendi.");
        }
        catch (Exception ex)
        {
            logger.LogError("Veritabanı başlatılırken hata oluştu: {Message}", ex.Message);
        }
    }
}
