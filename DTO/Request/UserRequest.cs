using System.ComponentModel.DataAnnotations;

namespace _2026_spark_backend.DTO.Requests;

public class UserRequest
{
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

    [MinLength(6)]
    public string? Password { get; set; }
}
