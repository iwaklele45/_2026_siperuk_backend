using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _2026_spark_backend.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = "User";

    [Required]
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
