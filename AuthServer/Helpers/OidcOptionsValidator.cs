using Microsoft.Extensions.Options;

namespace AuthServer.Helpers;

public class OidcOptionsValidator : IValidateOptions<OidcOptions>
{
    public ValidateOptionsResult Validate(string? name, OidcOptions? options)
    {
        if (options == null)
        {
            return ValidateOptionsResult.Fail("OidcOptions cannot be null.");
        }

        Console.WriteLine($"OIDC Authority: {options.Authority}");
        if (!options.RequireHttps && options.Authority.StartsWith("https://"))
        {
            return ValidateOptionsResult.Fail(
                "HTTPS is required for Authority unless RequireHttps is explicitly set to false.");
        }

        return ValidateOptionsResult.Success;
    }
}