namespace AuthServer.UseCases.Authentication;

public class AuthenticatorTokenResponse
{
    private string code;
    private TimeSpan expiresIn;
    private string token;
}