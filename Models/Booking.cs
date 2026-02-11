using System.ComponentModel.DataAnnotations;

namespace _2026_spark_backend.Models;

public class Booking
{
    public int Id { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    [MaxLength(250)]
    public string Purpose { get; set; } = string.Empty;

    [Required]
    public int BookingStatusId { get; set; }

    public Room? Room { get; set; }

    public User? User { get; set; }

    public BookingStatus? BookingStatus { get; set; }

    public ICollection<BookingStatusHistory> StatusHistories { get; set; } = new List<BookingStatusHistory>();
}
