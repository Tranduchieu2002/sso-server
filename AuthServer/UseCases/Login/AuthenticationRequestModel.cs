using Microsoft.AspNetCore.Mvc;

namespace AuthServer.UseCases.Login;

public enum ResponseType
{
    Code, // Authorization code flow
    Token, // Implicit flow
    IdToken // ID token only flow
}

public sealed class AuthenticationRequestModel
{
    [BindProperty(Name = "client_id", SupportsGet = true)]
    public string ClientId { get; set; } = string.Empty;

    [BindProperty(Name = "redirect_uri", SupportsGet = true)]
    public string RedirectUri { get; set; } = string.Empty;

    [BindProperty(Name = "response_type", SupportsGet = true)]
    public ResponseType ResponseType { get; set; } = ResponseType.Code;

    [BindProperty(Name = "scope", SupportsGet = true)]
    public string? Scope { get; set; } = string.Empty;

    [BindProperty(Name = "code_challenge", SupportsGet = true)]
    public string CodeChallenge { get; set; } = string.Empty;

    /**
     * This code will be generated from client
     */
    [BindProperty(Name = "code_challenge_method", SupportsGet = true)]
    public string CodeChallengeMethod { get; set; } = string.Empty;

    [BindProperty(Name = "nonce", SupportsGet = true)]
    public string Nonce { get; set; } = string.Empty;

    [BindProperty(Name = "state", SupportsGet = true)]
    public string State { get; set; } = string.Empty;

    [BindProperty(Name = "action", SupportsGet = true)]
    public string? Action { get; set; }
}

public sealed class TokenRequest
{
    public string GrantType { get; set; } = "authorization_code";
    public string Code { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string CodeVerifier { get; set; } = string.Empty;
}

public sealed class UserInfo
{
    public string Sub { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}