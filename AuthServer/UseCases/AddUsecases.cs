using AuthServer.UseCases.Authentication;
using AuthServer.UseCases.Authentication.AuthorizationCode;
using AuthServer.UseCases.Client;
using AuthServer.UseCases.Login;

namespace AuthServer.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IJwtUseCases, JwtUseCases>();
        services.AddScoped<LoginRequestHandler>();
        services.AddTransient<IGrantTokenUseCase, GrantTokenUseCase>();
        services.AddScoped<ISignInManager, SignInManager>();
        services.AddSingleton<IAuthorizationCodeUseCase, AuthorizationCodeUseCase>();
        services.AddClientUseCases();
        return services;
    }
}