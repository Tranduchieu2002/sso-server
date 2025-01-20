using System.Collections.Concurrent;
using AuthServer.Helpers;

namespace AuthServer.UseCases.Authentication.Infrastructure;

public static class AuthenticationInMemoryCache
{
    public static readonly ConcurrentDictionary<string, PkceToken> PkceStore = new();
}