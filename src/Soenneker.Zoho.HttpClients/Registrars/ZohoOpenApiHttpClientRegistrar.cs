using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Zoho.HttpClients.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.Zoho.HttpClients.Registrars;

/// <summary>
/// Registers the OpenAPI HttpClient wrapper for dependency injection.
/// </summary>
public static class ZohoOpenApiHttpClientRegistrar
{
    /// <summary>
    /// Adds <see cref="IZohoOpenApiHttpClient"/> as a singleton service.
    /// </summary>
    public static IServiceCollection AddZohoOpenApiHttpClientAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddSingleton<IZohoOpenApiHttpClient, ZohoOpenApiHttpClient>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IZohoOpenApiHttpClient"/> as a scoped service while retaining the singleton HTTP client cache.
    /// </summary>
    public static IServiceCollection AddZohoOpenApiHttpClientAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddScoped<IZohoOpenApiHttpClient, ZohoOpenApiHttpClient>();

        return services;
    }
}
