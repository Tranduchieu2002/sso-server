using System.Security.Claims;
using AuthServer.Helpers;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.UseCases;

public interface IJwtUseCases
{
    public string GenerateIdToken(
        string userId,
        string audience,
        string nonce,
        JsonWebKey jsonWebKey);

    public string GenerateAccessToken(
        string userId,
        string scope,
        string audience,
        string nonce);
}

public class JwtUseCases : IJwtUseCases
{
    private readonly JsonWebKey jsonWebKey;
    private readonly TokenIssuingOptions tokenIssuingOptions;

    public JwtUseCases(TokenIssuingOptions tokenIssuingOptions, JsonWebKey jsonWebKey)
    {
        this.tokenIssuingOptions = tokenIssuingOptions;
        this.jsonWebKey = jsonWebKey;
    }

    public string GenerateIdToken(string userId, string audience, string nonce, JsonWebKey jsonWebKey)
    {
        // https://openid.net/specs/openid-connect-core-1_0.html#IDToken
        // we can return some claims defined here: https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId)
        };

        var idToken = JwtGenerator.GenerateJwtToken(
            tokenIssuingOptions.IdTokenExpirySeconds,
            tokenIssuingOptions.Issuer,
            audience,
            nonce,
            claims,
            jsonWebKey
        );


        return idToken;
    }

    public string GenerateAccessToken(string userId, string scope, string audience, string nonce)
    {
        // access_token can be the same as id_token, but here we might have different values for expirySeconds so we use 2 different functions

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new("scope",
                scope) // Jeg vet ikke hvorfor JwtRegisteredClaimNames inneholder ikke "scope"??? Det har kun OIDC ting?  https://datatracker.ietf.org/doc/html/rfc8693#name-scope-scopes-claim
        };
        var idToken = JwtGenerator.GenerateJwtToken(
            tokenIssuingOptions.AccessTokenExpirySeconds,
            tokenIssuingOptions.Issuer,
            audience,
            nonce,
            claims,
            jsonWebKey
        );

        return idToken;
    }
}