using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Zoho.HttpClients.Abstract;

/// <summary>
/// A .NET thread-safe singleton HttpClient for 
/// </summary>
public interface IZohoOpenApiHttpClient: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);

    /// <summary>Gets a client for a specific API key using the configured base URL.</summary>
    ValueTask<HttpClient> Get(string apiKey, CancellationToken cancellationToken = default);

    /// <summary>Gets a client for a specific Zoho connection.</summary>
    ValueTask<HttpClient> Get(string apiKey, string baseUrl, CancellationToken cancellationToken = default);
}
