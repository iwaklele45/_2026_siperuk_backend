using System.ComponentModel.DataAnnotations;

namespace _2026_spark_backend.Models;

public class BookingStatusHistory
{
    public int Id { get; set; }

    [Required]
    public int BookingId { get; set; }

    [Required]
    public int BookingStatusId { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(250)]
    public string? Notes { get; set; }

    public Booking? Booking { get; set; }

    public BookingStatus? BookingStatus { get; set; }
}
