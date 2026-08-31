using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Zoho.HttpClients.Abstract;

/// <summary>
/// Provides cached HTTP clients for Zoho CRM access tokens and data-center API origins.
/// </summary>
public interface IZohoOpenApiHttpClient: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets a client using the configured access token and CRM API base URL.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);

    /// <summary>Gets a client for a specific access token using the configured base URL.</summary>
    /// <param name="apiKey">The Zoho OAuth access token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The configured HTTP client.</returns>
    ValueTask<HttpClient> Get(string apiKey, CancellationToken cancellationToken = default);

    /// <summary>Gets a client for a specific Zoho access token and data-center API base URL.</summary>
    /// <param name="apiKey">The Zoho OAuth access token.</param>
    /// <param name="baseUrl">The CRM API base URL, including the API version.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The configured HTTP client.</returns>
    ValueTask<HttpClient> Get(string apiKey, string baseUrl, CancellationToken cancellationToken = default);
}
