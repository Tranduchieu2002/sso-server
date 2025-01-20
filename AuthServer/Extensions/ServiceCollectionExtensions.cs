using AuthServer.Helpers;
using AuthServer.UseCases;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Load configuration
        var tokenIssuer = configuration.GetSection("TokenIssuer").Get<TokenIssuingOptions>() ?? new TokenIssuingOptions();

        // Swagger configuration
        services.AddSwaggerGen(options =>
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });

        // Add custom services
        services.AddSingleton(EstablishJwtConfiguration.LoadFromDefault());
        services.AddOIDC(configuration);
        services.AddUseCases();
        services.AddEndpointsApiExplorer();

        // Add distributed memory cache and session
        services.AddDistributedMemoryCache();
        services.AddSession();

        // Razor Pages with custom route
        services.AddRazorPages(options =>
        {
            options.Conventions.AddPageRoute("/Authorize/Index", "oauth2/authorize");
        });
        services.AddAntiforgery();           // Enable CSRF protection

        // Authentication and authorization
        services.AddSingleton<TokenIssuingOptions>(tokenIssuer);
        services.Configure<OidcOptions>(configuration.GetSection("OidcOptions"));
        services.AddSingleton<IValidateOptions<OidcOptions>, OidcOptionsValidator>();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/SignIn"; // Redirect here for unauthorized access
                options.AccessDeniedPath = "/AccessDenied"; // Redirect if access is denied
            });

        // Suppress API behavior inference
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressInferBindingSourcesForParameters = true;
        });

        // Map endpoints
        services.AddEndpoints(typeof(Program).Assembly);
    }
}