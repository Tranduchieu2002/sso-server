using System.ComponentModel.DataAnnotations;
using AuthServer.Helpers;
using Microsoft.Extensions.Options;

namespace AuthServer;

public static class OIDCConfigurationService
{
    private static void AddOidcConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind OidcOptions to the "OidcOptions" section in appsettings.json
        services.AddOptions<OidcOptions>()
            .BindConfiguration("OidcOptions")
            .ValidateDataAnnotations() // Automatically validate DataAnnotations attributes
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<OidcOptions>, OidcOptionsValidator>();


        // Validate the options immediately during application startup
        var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<OidcOptions>>().Value;

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(options);

        if (!Validator.TryValidateObject(options, validationContext, validationResults, true))
        {
            foreach (var validationResult in validationResults)
            {
                if (validationResult.ErrorMessage != null)
                    throw new OptionsValidationException("OidcOptions", typeof(OidcOptions),
                        new[] { validationResult.ErrorMessage });
            }
        }
    }

    public static IServiceCollection AddOIDC(this IServiceCollection services, IConfiguration configuration)
    {
        // Add OIDC configuration and validation
        AddOidcConfiguration(services, configuration);

        // Retrieve the validated OidcOptions from configuration
        var oidcOptions = configuration.GetSection("OidcOptions").Get<OidcOptions>();

        // Add authentication services
        // services.AddAuthentication(options =>
        //     {
        //         options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //         options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //         options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        //     })
        //     .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        //     .AddOpenIdConnect("oidc", options =>
        //     {
        //         options.Authority = oidcOptions!.Authority; // From OidcOptions
        //         options.ClientId = oidcOptions.ClientId; // From OidcOptions
        //         options.ClientSecret = oidcOptions.ClientSecret; // From OidcOptions
        //         options.ResponseType = OpenIdConnectResponseType.Code; // Authorization Code flow
        //         options.RequireHttpsMetadata = false;
        //         options.SaveTokens = true; // Stores tokens in the auth cookie
        //
        //         // Add Scopes
        //         options.Scope.Add("openid");
        //         options.Scope.Add("profile");
        //         options.Scope.Add("email");
        //
        //         // Map claims from the provider to the application's claims
        //         options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        //         options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        //
        //         options.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             NameClaimType = "name",
        //             RoleClaimType = "roles"
        //         };
        //
        //         options.Events = new OpenIdConnectEvents
        //         {
        //             OnTokenValidated = async context =>
        //             {
        //                 // Custom logic after token validation
        //             },
        //             OnAuthenticationFailed = context =>
        //             {
        //                 context.HandleResponse();
        //                 context.Response.Redirect("/Error");
        //                 return Task.CompletedTask;
        //             }
        //         };
        //     });

        services.AddControllersWithViews();
        return services;
    }
}