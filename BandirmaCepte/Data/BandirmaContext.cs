using Microsoft.EntityFrameworkCore;
using BandirmaCepte.Models;

namespace BandirmaCepte.Data;

public class BandirmaContext : DbContext
{
    public BandirmaContext(DbContextOptions<BandirmaContext> options) : base(options) { }

    public DbSet<News> News { get; set; }
    public DbSet<BusLine> BusLines { get; set; }
    public DbSet<BusSchedule> BusSchedules { get; set; }
    public DbSet<Pharmacy> Pharmacies { get; set; }
    public DbSet<Earthquake> Earthquakes { get; set; }
    public DbSet<EmergencyContact> EmergencyContacts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Role Tohum Verisi (Seed Data)
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" }
        );

        // 2. User -> Role İlişkisi (Çoka-Bir)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // 3. BusLine -> BusSchedule İlişkisi (Bire-Çok: Bir hattın birden fazla sefer saati olur)
        modelBuilder.Entity<BusSchedule>()
            .HasOne(s => s.BusLine)
            .WithMany(l => l.Schedules)
            .HasForeignKey(s => s.BusLineId)
            .OnDelete(DeleteBehavior.Cascade);

        // 4. User -> News İlişkisi (Bire-Çok: Haberi ekleyen yönetici)
        modelBuilder.Entity<News>()
            .HasOne(n => n.User)
            .WithMany(u => u.News)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // 5. User -> Earthquake İlişkisi (Bire-Çok: Manuel deprem kaydını giren yönetici)
        modelBuilder.Entity<Earthquake>()
            .HasOne(e => e.User)
            .WithMany(u => u.Earthquakes)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // 6. User -> EmergencyContact İlişkisi (Bire-Çok: Acil numarayı ekleyen yönetici)
        modelBuilder.Entity<EmergencyContact>()
            .HasOne(c => c.User)
            .WithMany(u => u.EmergencyContacts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
