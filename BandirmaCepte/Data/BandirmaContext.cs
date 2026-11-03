using Microsoft.EntityFrameworkCore;
using BandirmaCepte.Models;

namespace BandirmaCepte.Data;

public class BandirmaContext : DbContext
{
    public BandirmaContext(DbContextOptions<BandirmaContext> options) : base(options) { }

    public DbSet<News> News { get; set; }
    public DbSet<BusLine> BusLines { get; set; }
    public DbSet<Pharmacy> Pharmacies { get; set; }
    public DbSet<Earthquake> Earthquakes { get; set; }
    public DbSet<EmergencyContact> EmergencyContacts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" }
        );
    }
}
