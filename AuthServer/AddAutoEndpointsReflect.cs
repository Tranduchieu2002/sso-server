using System.Reflection;
using AuthServer.Enpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AuthServer;

public static class AddAutoEndpointsReflect
{
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        Assembly assembly)
    {
        // Register IEndpoint implementations as Scoped services
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Scoped(typeof(IEndpoint), type)) // Register as Scoped
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        // Create a scope to resolve scoped services
        using var scope = app.Services.CreateScope();
        var endpoints = scope.ServiceProvider.GetRequiredService<IEnumerable<IEndpoint>>();

        var builder = (IEndpointRouteBuilder)routeGroupBuilder! ?? app;

        var mappedEndpoints = new HashSet<string>(); // Track mapped routes

        foreach (IEndpoint endpoint in endpoints)
        {
            var routePath = endpoint.GetType().Name;

            // Check if the route is already mapped
            if (!mappedEndpoints.Contains(routePath))
            {
                endpoint.MapEndpoint(builder);
                mappedEndpoints.Add(routePath); // Track the mapped route
            }
        }

        return app;
    }
}