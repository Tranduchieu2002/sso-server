namespace AuthServer.UseCases.Authentication.AuthorizationCode;

public class AuthorizationCodeUseCase : IAuthorizationCodeUseCase
{
    // In-memory storage for authorization codes (for demo purposes)
    private readonly Dictionary<string, AuthorizationCodeInfo> _authorizationCodes = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="clientId"></param>
    /// <param name="scopes"></param>
    /// <returns></returns>
    public async Task<string> CreateAuthorizationCodeAsync(string userId, string clientId, string[] scopes)
    {
        // Generate a unique authorization code (e.g., a GUID or random string)
        var authorizationCode = Guid.NewGuid().ToString("N");

        // Store the authorization code with associated information
        var codeInfo =
            new AuthorizationCodeInfo(clientId, userId, authorizationCode, scopes, DateTime.UtcNow.AddMinutes(10));

        // Store in-memory (can be replaced with DB in production)
        _authorizationCodes[authorizationCode] = codeInfo;

        await Task.CompletedTask;
        return authorizationCode;
    }

    public Task<bool> VerifyAuthorizationCodeAsync(string code)
    {
        _authorizationCodes.TryGetValue(code, out var authorizationCode);
        return Task.FromResult(authorizationCode != null);
    }
}