using System.Security.Cryptography;
using System.Text;

namespace AuthServer.Helpers;

public record PkceToken
{
    public string CodeVerifier { get; init; } = default!;
    public string CodeChallenge { get; init; } = default!;
    public string CodeChallengeMethod { get; init; } = "S256"; // Default PKCE method
    public DateTime Expiration { get; init; }
}

/// <summary>
/// Provides a randomly generating PKCE code verifier and it's corresponding code challenge with expiration in 10mins
/// </summary>
internal static class PCKEGenerator
{
    public static PkceToken Generate(int size = 32)
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[size];
        rng.GetBytes(randomBytes);
        var verifier = Base64UrlEncode(randomBytes);

        var buffer = Encoding.UTF8.GetBytes(verifier);
        var hash = SHA256.Create().ComputeHash(buffer);
        var challenge = Base64UrlEncode(hash);
        var pkceToken = new PkceToken
        {
            CodeVerifier = verifier,
            CodeChallenge = challenge,
            CodeChallengeMethod = "S256",
            Expiration = DateTime.UtcNow.AddMinutes(10) // Expiry for PKCE token
        };

        return pkceToken;
    }

    private static string Base64UrlEncode(byte[] data) =>
        Convert.ToBase64String(data)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');

    public static string ComputeCodeChallenge(string codeVerifier)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
        return Base64UrlEncode(bytes);
    }

    public static bool VerifyCodeChallenge(string codeVerifier, string codeChallenge)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));

        return Base64UrlEncode(bytes) == codeChallenge;
    }
}