using Microsoft.EntityFrameworkCore;
using _2026_spark_backend.Models;
using _2026_spark_backend.Services;

namespace _2026_spark_backend.Seeders;

public static class ModelBuilderSeedUsers
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        var passwordService = new PasswordService();

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FullName = "Administrator Kampus",
                Email = "admin@spark.test",
                Role = "Admin",
                PasswordHash = passwordService.Hash("admin123")
            },
            new User
            {
                Id = 2,
                FullName = "Staff Akademik",
                Email = "staff@spark.test",
                Role = "Staff",
                PasswordHash = passwordService.Hash("staff123")
            }
        );
    }
}
