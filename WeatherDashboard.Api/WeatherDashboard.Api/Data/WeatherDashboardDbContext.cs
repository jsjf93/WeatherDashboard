using Microsoft.EntityFrameworkCore;
using WeatherDashboard.Api.Models.Entities;

namespace WeatherDashboard.Api.Data;

public class WeatherDashboardDbContext(DbContextOptions<WeatherDashboardDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserFavourite> UserFavourites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<UserFavourite>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.City }).IsUnique();
            entity.Property(e => e.City).IsRequired().HasMaxLength(256);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany(u => u.Favourites)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
