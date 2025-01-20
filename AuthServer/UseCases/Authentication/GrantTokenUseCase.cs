using System.Net;
using AuthServer.Helpers;
using AuthServer.Models;
using AuthServer.UseCases.Authentication.Infrastructure;

namespace AuthServer.UseCases.Authentication;

public class GrantTokenUseCase(IJwtUseCases jwtUseCases) : IGrantTokenUseCase
{
    public async Task<TokenResponse> Handle(string code)
    {
        if (!AuthenticationInMemoryCache.PkceStore.TryGetValue(code, out var token))
        {
            throw new HttpRequestException("Invalid access token", new Exception(), HttpStatusCode.BadGateway);
        }

        if (token.CodeChallenge != PCKEGenerator.ComputeCodeChallenge(token.CodeVerifier))
        {
            throw new HttpRequestException("Invalid access token", new Exception(), HttpStatusCode.Unauthorized);
        }

        var jwtToken = jwtUseCases.GenerateAccessToken(Guid.NewGuid().ToString(), "", token.CodeVerifier, "");
        return
            new TokenResponse(jwtToken, 123, "Bearer");
    }
}