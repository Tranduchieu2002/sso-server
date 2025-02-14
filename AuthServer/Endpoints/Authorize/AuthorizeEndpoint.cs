using System.Security.Claims;
using AuthServer.Enpoints;
using AuthServer.Models;
using AuthServer.UseCases.Authentication;
using AuthServer.UseCases.Authentication.AuthorizationCode;
using AuthServer.UseCases.Client;
using AuthServer.UseCases.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace AuthServer.Endpoints.Authorize;

public class GrantTokenRequestModal
{
    public GrantTokenRequestModal()
    {
        code = !string.IsNullOrWhiteSpace(code) ? code : throw new ArgumentNullException(nameof(code));
    }

    public string code { get; set; }
}

public class AuthorizeEndpoint(
    LoginRequestHandler loginRequestHandler,
    IGrantTokenUseCase grantTokenUseCase,
    ISignInManager signInManager,
    ILogger<AuthorizeEndpoint> logger,
    IClientService clientService,
    IAuthorizationCodeUseCase authorizationCodeService)
    : IEndpoint
{
    private readonly IAuthorizationCodeUseCase _authorizationCodeService =
        authorizationCodeService ?? throw new ArgumentNullException(nameof(authorizationCodeService));

    private readonly IClientService _clientService = clientService;

    private readonly IGrantTokenUseCase _grantTokenUseCase =
        grantTokenUseCase ?? throw new ArgumentNullException(nameof(grantTokenUseCase));

    private readonly ILogger<AuthorizeEndpoint> _logger = logger;

    private readonly LoginRequestHandler _loginRequestHandler =
        loginRequestHandler ?? throw new ArgumentNullException(nameof(loginRequestHandler));

    private readonly ISignInManager _signInUseCase =
        signInManager ?? throw new ArgumentNullException(nameof(signInManager));

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        const string ApiVersion = "v1"; // Define API version here.
        // only allow www-url-application 
        app.MapPost($"oauth2/{ApiVersion}/authorize",
            async (HttpContext context, [FromForm] AuthenticationRequestModel request) =>
            {
                // Ensure the user is authenticated
                if ((bool)(!context.User.Identity?.IsAuthenticated)!)
                {
                    return Results.Unauthorized();
                }

                var user = context.User; // Current authenticated user
                var userId = user.FindFirstValue(ClaimTypes.Name); // Extract user ID (or any relevant claim)

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.BadRequest("User information is missing.");
                }

                try
                {
                    // Validate the client application
                    var client = await _clientService.GetClientAsync(request.ClientId);
                    if (client == null)
                    {
                        return Results.BadRequest("Invalid client ID.");
                    }

                    // Verify redirect URI
                    if (!client.RedirectUris.Contains(request.RedirectUri))
                    {
                        return Results.BadRequest("Redirect URI mismatch.");
                    }

                    if (!new[] { ResponseType.Code, ResponseType.Token, ResponseType.IdToken }.Contains(
                            request.ResponseType))
                    {
                        return Results.BadRequest("Unsupported response type.");
                    }

                    // Validate scopes
                    var requestedScopes = request.Scope?.Split(',') ?? [];
                    string[] validScopes = ["openid", "profile", "email"];
                    if (requestedScopes.Any(scope => !validScopes.Contains(scope)))
                    {
                        return Results.BadRequest("Invalid or unsupported scope(s) requested.");
                    }

                    // Generate tokens
                    string authorizationCode;
                    string? idToken = null;
                    string? tokenId = null;

                    switch (request.ResponseType)
                    {
                        case ResponseType.Code:
                            // Generate the authorization code
                            authorizationCode =
                                await _authorizationCodeService.CreateAuthorizationCodeAsync(userId, request.ClientId,
                                    requestedScopes);
                            break;
                        default: return Results.BadRequest($"{request.ResponseType} is not supported.");
                        // case ResponseType.Token:
                        //     idToken = _tokenService.GenerateIdToken(user, client, request.Nonce);
                        //
                        //     tokenId = await _tokenService.CreateTokenIdAsync(idToken, userId, request.ClientId);
                        //     break;
                    }

                    var redirectUrlParams = new Dictionary<string, string>
                    {
                        { "state", request.State }
                    };

                    if (!string.IsNullOrEmpty(authorizationCode))
                    {
                        redirectUrlParams["code"] = authorizationCode;
                    }

                    if (request.ResponseType == ResponseType.Token && !string.IsNullOrEmpty(idToken))
                    {
                        redirectUrlParams["id_token"] = idToken;
                    }

                    if (!string.IsNullOrEmpty(tokenId))
                    {
                        redirectUrlParams["token_id"] = tokenId;
                    }

                    var redirectUrl = QueryHelpers.AddQueryString(request.RedirectUri, redirectUrlParams!);

                    return Results.Redirect(redirectUrl);
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes
                    _logger.LogError(ex, "Error handling the authorization request.");
                    return Results.Problem(statusCode: StatusCodes.Status500InternalServerError,
                        detail: "An error occurred while processing the request.");
                }
            });
        // Token issuance route
        app.MapPost($"api/{ApiVersion}/token", async (HttpContext context, [FromBody] GrantTokenRequestModel request) =>
        {
            if (string.IsNullOrEmpty(request.Code))
            {
                context.Response.StatusCode = 400;
                return Results.Text("Invalid code.", statusCode: 400);
            }

            var tokenResponse = await _grantTokenUseCase.Handle(request.Code);

            return Results.Json(tokenResponse);
        });

        app.MapPost($"api/{ApiVersion}/signin", async (HttpContext context, [FromBody] SignInRequestModel model) =>
        {
            var res = await _signInUseCase.Handle(context, model);

            if (res.IsSuccess)
            {
                return Results.Ok(res);
            }

            // Return the Razor Page with the error message
            context.Items["ErrorMessage"] = res.ErrorMessage;
            return Results.BadRequest(new { ErrorMessage = res.ErrorMessage }); // Optional: Handle as API response
        });
    }
}