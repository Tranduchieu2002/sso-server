namespace AuthServer.UseCases.Authentication;

public record AuthorizationCodeInfo(
    string ClientId,
    string UserId,
    string AuthorizationCode,
    string[] Scopes,
    DateTime Expiration)
{
}