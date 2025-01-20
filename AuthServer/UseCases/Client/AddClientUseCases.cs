namespace AuthServer.UseCases.Client;

public static class DependencyInjections
{
    public static void AddClientUseCases(this IServiceCollection services)
    {
        services.AddSingleton<IClientRepository, ClientRepository>();
        services.AddScoped<IClientService, ClientService>();
    }
}