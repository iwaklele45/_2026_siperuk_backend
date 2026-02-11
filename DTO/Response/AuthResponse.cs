using System;

namespace _2026_spark_backend.DTO.Responses;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public UserResponse User { get; set; } = new();
}
