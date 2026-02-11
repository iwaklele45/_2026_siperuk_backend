using System.ComponentModel.DataAnnotations;

namespace _2026_spark_backend.Models;

public class Room
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Location { get; set; } = string.Empty;

    [Range(1, 500)]
    public int Capacity { get; set; }

    [MaxLength(300)]
    public string? Description { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
