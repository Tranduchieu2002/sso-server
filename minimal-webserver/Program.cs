using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();
app.UseCookiePolicy();

// Endpoint to generate and serve the authorization URL with CSRF protection
app.MapGet("/generate-auth-url", async (HttpContext context) =>
{
    // Generate CSRF state
    var csrfState = Guid.NewGuid().ToString("N");

    // Set CSRF state in a secure cookie
    context.Response.Cookies.Append("csrfState", csrfState, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        MaxAge = TimeSpan.FromMinutes(10) // Expires in 10 minutes
    });

    // Construct the authorization URL
    var clientKey = "1234";
    var redirectUri = Uri.EscapeDataString(context.Request.PathBase + "/oauth2/callback");
    var scope = "openid,profile,email";
    var responseType = "code";
    var authorizationUrl = $"https://localhost:7064/oauth2/authorize?" +
                           $"client_id={clientKey}&" +
                           $"scope={scope}&" +
                           $"response_type={responseType}&" +
                           $"redirect_uri={redirectUri}&" +
                           $"state={csrfState}";

    // Return the URL as a response
    context.Response.Redirect(authorizationUrl);
});

// Endpoint to handle authorization callback and validate CSRF state
app.MapGet("/oauth2/callback", async (HttpContext context) =>
{
    var query = context.Request.Query;

    // Extract `state` from the query string
    if (!query.TryGetValue("state", out var receivedState) || string.IsNullOrEmpty(receivedState))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Missing or invalid state parameter.");
        return;
    }

    // Retrieve stored CSRF state from cookies
    if (!context.Request.Cookies.TryGetValue("csrfState", out var storedState) || string.IsNullOrEmpty(storedState))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Missing or invalid CSRF cookie.");
        return;
    }

    // Validate CSRF state
    if (!string.Equals(storedState, receivedState, StringComparison.Ordinal))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("CSRF validation failed.");
        return;
    }

    // CSRF validation passed
    await context.Response.WriteAsync("CSRF validation successful. Authorization complete.");
});

app.Run();
