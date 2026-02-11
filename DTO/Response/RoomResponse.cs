namespace _2026_spark_backend.DTO.Responses;

public class RoomResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public string? Description { get; set; }
}
