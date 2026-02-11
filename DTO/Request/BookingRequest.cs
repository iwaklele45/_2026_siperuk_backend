using System.ComponentModel.DataAnnotations;

namespace _2026_spark_backend.DTO.Requests;

public class BookingRequest
{
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

    public int? BookingStatusId { get; set; }
}
