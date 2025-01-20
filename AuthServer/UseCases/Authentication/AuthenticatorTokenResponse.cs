namespace AuthServer.UseCases.Authentication;

public class AuthenticatorTokenResponse
{
    private string code;
    private string token;
    private TimeSpan expiresIn;
}