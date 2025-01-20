using AuthServer.Helpers;
using AuthServer.UseCases.Authentication.Infrastructure;

namespace AuthServer.UseCases.Login;

public class LoginRequestHandler(IJwtUseCases jwtUseCases, ILogger<LoginRequestHandler> logger)
{
    // Simulated user database
    private List<User> users =
    [
        new User { Username = "admin", Password = "admin" },
        new User { Username = "user", Password = "password" }
    ];

    // Simulate a data store for PKCE tokens (use database/Redis in production)
    //  return redirectUrl
    public async Task<string> Handle(HttpContext context, AuthenticationRequestModel request)
    {
        // Authenticate user
        var authenticatedUser = users.Find(u => u.Password == request.State);
        if (authenticatedUser == null)
        {
            logger.LogWarning("LoginRequestHandler.Handle - Invalid username or password.");
            return "Invalid username or password.";
        }
        // Saving State
        // AuthenticationInMemoryCache.PkceStore[authenticatedUser.Username] = St;
        //
        // logger.LogCritical(
        //     $"LoginRequestHandler.Handle - PKCE saved for user '{authenticatedUser.Username}': {pkce.CodeChallenge}");

        // hh success response (could also return a token or other info as needed)
        return $"{""}#{authenticatedUser.Password}";
    }


    internal PkceToken? FetchPkceToken(string username)
    {
        // Retrieve PKCE token for the user
        if (!AuthenticationInMemoryCache.PkceStore.TryGetValue(username, out var pkceToken))
            return null; // Return null if no valid token exists
        if (pkceToken.Expiration > DateTime.UtcNow)
        {
            return pkceToken; // Return if still valid
        }

        // Remove expired token
        AuthenticationInMemoryCache.PkceStore.TryRemove(username, out _);

        return null; // Return null if no valid token exists
    }

    // Simulate user data
    private record User
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}