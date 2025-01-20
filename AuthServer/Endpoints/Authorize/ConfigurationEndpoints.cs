using AuthServer.Enpoints;

namespace AuthServer.Endpoints.Authorize;

public class ConfigurationEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(".well-known/openid-configuration", (HttpContext context) =>
        {
            // Define the path to your .well-known directory
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "oidc-assets", ".well-known",
                "openid-configuration.json");

            if (!File.Exists(filePath))
            {
                context.Response.StatusCode = 404;
                return Results.Text("Configuration file not found.", statusCode: 404);
            }

            // Serve the static file with correct content type
            return Results.File(filePath, "application/json");
        });

        // Serve the JWKs files
        app.MapGet(".well-known/jwks", (HttpContext context) =>
        {
            var jwkFilePath = Path.Combine(Directory.GetCurrentDirectory(), "oidc-assets", ".private", "jwk.json");

            if (!File.Exists(jwkFilePath))
            {
                context.Response.StatusCode = 404;
                return Results.Text("JWK file not found.", statusCode: 404);
            }

            return Results.File(jwkFilePath, "application/json");
        });

        app.MapGet(".well-known/public-jwks", (HttpContext context) =>
        {
            var publicJwkFilePath =
                Path.Combine(Directory.GetCurrentDirectory(), "oidc-assets", ".public", "public-jwk.json");

            if (!File.Exists(publicJwkFilePath))
            {
                context.Response.StatusCode = 404;
                return Results.Text("Public JWK file not found.", statusCode: 404);
            }

            return Results.File(publicJwkFilePath, "application/json");
        });
    }
}