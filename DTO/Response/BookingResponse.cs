namespace _2026_spark_backend.DTO.Responses;

public class BookingResponse
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public string RoomName { get; set; } = string.Empty;

    public int UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string Purpose { get; set; } = string.Empty;

    public int BookingStatusId { get; set; }

    public string BookingStatusName { get; set; } = string.Empty;
}
