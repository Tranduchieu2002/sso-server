namespace AuthServer.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseAppMiddleware(this WebApplication app)
    {
        app.MapEndpoints(); // Map endpoints
        app.UseHttpsRedirection(); // Enforce HTTPS
        app.UseSession(); // Enable session
        app.MapRazorPages(); // Map Razor Pages
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseAuthentication(); // Enable authentication
        app.UseAntiforgery();
        app.UseAuthorization(); // Enable authorization
    }
}
