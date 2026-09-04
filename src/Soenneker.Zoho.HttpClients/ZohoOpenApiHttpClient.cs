using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.Hashing.XxHash;
using Soenneker.Zoho.HttpClients.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.Zoho.HttpClients;

/// <inheritdoc cref="IZohoOpenApiHttpClient" />
public sealed class ZohoOpenApiHttpClient : IZohoOpenApiHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _configuration;
    private readonly string _baseUrl;
    private readonly string _authHeaderName;
    private readonly string _authHeaderValueTemplate;
    private readonly string _clientIdPrefix = $"{nameof(ZohoOpenApiHttpClient)}:{Guid.NewGuid():N}";
    private readonly ConcurrentDictionary<string, byte> _clientIds = new();

    private const string _prodBaseUrl = "https://www.zohoapis.com/crm/v8/";

    public ZohoOpenApiHttpClient(IHttpClientCache httpClientCache, IConfiguration config)
    {
        _httpClientCache = httpClientCache;
        _configuration = config;
        _baseUrl = config["Zoho:ClientBaseUrl"] ?? _prodBaseUrl;
        _authHeaderName = config["Zoho:AuthHeaderName"] ?? "Authorization";
        _authHeaderValueTemplate = config["Zoho:AuthHeaderValueTemplate"] ?? "Zoho-oauthtoken {token}";
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        string accessToken = _configuration["Zoho:AccessToken"] ?? _configuration.GetValueStrict<string>("Zoho:ApiKey");
        return Get(accessToken, _baseUrl, cancellationToken);
    }

    public ValueTask<HttpClient> Get(string apiKey, CancellationToken cancellationToken = default)
    {
        return Get(apiKey, _baseUrl, cancellationToken);
    }

    public ValueTask<HttpClient> Get(string apiKey, string baseUrl, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);

        var baseUri = new Uri(baseUrl.EndsWith("/", StringComparison.Ordinal) ? baseUrl : $"{baseUrl}/", UriKind.Absolute);
        string clientId = GetClientId(apiKey, baseUri);
        _clientIds.TryAdd(clientId, 0);

        return _httpClientCache.Get(clientId,
            (apiKey, baseUri, authHeaderName: _authHeaderName, authHeaderValueTemplate: _authHeaderValueTemplate), static state =>
            {
                string authHeaderValue = state.authHeaderValueTemplate.Replace("{token}", state.apiKey, StringComparison.Ordinal);

                return new HttpClientOptions
                {
                    BaseAddress = state.baseUri,
                    DefaultRequestHeaders = new Dictionary<string, string>
                    {
                        {state.authHeaderName, authHeaderValue},
                    }
                };
            }, cancellationToken);
    }

    private string GetClientId(string apiKey, Uri baseUri)
    {
        string value = string.Concat(apiKey, "\0", baseUri, "\0", _authHeaderName, "\0", _authHeaderValueTemplate);

        return $"{_clientIdPrefix}:{XxHash3Util.Hash(value)}";
    }

    public void Dispose()
    {
        foreach (string clientId in _clientIds.Keys)
        {
            _httpClientCache.RemoveSync(clientId);
        }
    }

    public async ValueTask DisposeAsync()
    {
        foreach (string clientId in _clientIds.Keys)
        {
            await _httpClientCache.Remove(clientId);
        }
    }
}
