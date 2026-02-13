using Microsoft.EntityFrameworkCore;
using _2026_spark_backend.Models;
using _2026_spark_backend.Seeders;

namespace _2026_spark_backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<Booking> Bookings => Set<Booking>();

    public DbSet<BookingStatus> BookingStatuses => Set<BookingStatus>();

    public DbSet<BookingStatusHistory> BookingStatusHistories => Set<BookingStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.BookingStatus)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.BookingStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookingStatusHistory>()
            .HasOne(h => h.Booking)
            .WithMany(b => b.StatusHistories)
            .HasForeignKey(h => h.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookingStatusHistory>()
            .HasOne(h => h.BookingStatus)
            .WithMany(s => s.BookingStatusHistories)
            .HasForeignKey(h => h.BookingStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed initial users and booking statuses
        ModelBuilderSeedUsers.SeedData(modelBuilder);
        ModelBuilderSeedStatus.SeedData(modelBuilder);

    }
}
