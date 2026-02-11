// not yet
using Microsoft.EntityFrameworkCore;
using _2026_spark_backend.Models;
using _2026_spark_backend.Services;

namespace _2026_spark_backend.Seeders;

public static class ModelBuilderSeedStatus
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingStatus>().HasData(
            new BookingStatus
            {
                Id = 1,
                Name = "Waiting",
                Description = "Booking sedang menunggu antrian"
            },
            new BookingStatus
            {
                Id = 2,
                Name = "Approved",
                Description = "Booking sudah disetujui"
            },
            new BookingStatus
            {
                Id = 3,
                Name = "Rejected",
                Description = "Booking ditolah"
            }, new BookingStatus
            {
                Id = 4,
                Name = "Finish",
                Description = "Booking telah selesai"
            }
        );
    }
}