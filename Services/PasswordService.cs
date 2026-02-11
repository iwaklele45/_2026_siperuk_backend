using System.Security.Cryptography;
using System.Text;

namespace _2026_spark_backend.Services;

public interface IPasswordService
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

public class PasswordService : IPasswordService
{
    public string Hash(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    public bool Verify(string password, string hash)
    {
        var computed = Hash(password);
        return string.Equals(computed, hash, StringComparison.OrdinalIgnoreCase);
    }
}
