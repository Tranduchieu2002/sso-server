namespace AuthServer.Models;

public record TokenResponse(string AccessToken, long ExpiresIn, string TokenType)
{
}