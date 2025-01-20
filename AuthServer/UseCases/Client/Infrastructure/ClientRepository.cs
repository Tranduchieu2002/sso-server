using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace AuthServer.UseCases.Client;

public class ClientRepository(IMemoryCache memoryCache) : IClientRepository
{
    private const string CacheKeyPrefix = "Client_";

    /// <summary>
    /// Retrieves a client by ID from the memory cache.
    /// </summary>
    public Task<Client?> GetClientById(string id)
    {
        var cacheKey = GetCacheKey(id);

        if (!memoryCache.TryGetValue(cacheKey, out string? cachedData)) return Task.FromResult<Client?>(null);
        // Deserialize the cached JSON back to a Client object
        return cachedData != null
            ? Task.FromResult(JsonSerializer.Deserialize<Client>(cachedData))
            : Task.FromResult<Client?>(null);
    }

    /// <summary>
    /// Saves a client in the memory cache with an expiration time.
    /// </summary>
    public Task SaveClient(string id, Client client)
    {
        var cacheKey = GetCacheKey(id);
        var serializedClient = JsonSerializer.Serialize(client);

        // Set cache with expiration of 1 hour
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };

        memoryCache.Set(cacheKey, serializedClient, cacheEntryOptions);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Generates a unique cache key for the given client ID.
    /// </summary>
    private static string GetCacheKey(string id) => $"{CacheKeyPrefix}{id}";
}