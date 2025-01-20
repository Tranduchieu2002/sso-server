namespace AuthServer.UseCases.Authentication.AuthorizationCode;

public interface IAuthorizationCodeUseCase
{
    Task<string> CreateAuthorizationCodeAsync(string userId, string clientId, string[] scopes);
    Task<bool> VerifyAuthorizationCodeAsync(string code);

}