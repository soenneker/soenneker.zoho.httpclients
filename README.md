[![](https://img.shields.io/nuget/v/soenneker.zoho.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.zoho.httpclients/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.zoho.httpclients/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.zoho.httpclients/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.zoho.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.zoho.httpclients/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.zoho.httpclients/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.zoho.httpclients/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Zoho.HttpClients

Provides cached `HttpClient` instances configured for Zoho CRM OAuth access tokens and data-center API origins.

## Installation

```shell
dotnet add package Soenneker.Zoho.HttpClients
```

## Configuration

```json
{
  "Zoho": {
    "AccessToken": "your-oauth-access-token",
    "ClientBaseUrl": "https://www.zohoapis.com/crm/v8/"
  }
}
```

The US CRM v8 base URL is used when `ClientBaseUrl` is omitted. Use the API domain returned by Zoho's token response for other data centers, such as `https://www.zohoapis.eu/crm/v8/`. `Zoho:ApiKey` remains supported as a legacy alias for `Zoho:AccessToken`.

The default authorization header is `Authorization: Zoho-oauthtoken {token}`. `AuthHeaderName` and `AuthHeaderValueTemplate` are available for nonstandard integrations.

## Registration

```csharp
services.AddZohoOpenApiHttpClientAsSingleton();
```

Scoped registration is also available:

```csharp
services.AddZohoOpenApiHttpClientAsScoped();
```

## Usage

```csharp
public sealed class ZohoUserReader
{
    private readonly IZohoOpenApiHttpClient _zoho;

    public ZohoUserReader(IZohoOpenApiHttpClient zoho)
    {
        _zoho = zoho;
    }

    public async Task<string> GetUsers(CancellationToken cancellationToken)
    {
        HttpClient client = await _zoho.Get(cancellationToken);
        return await client.GetStringAsync("users?type=AllUsers", cancellationToken);
    }
}
```

Pass connection values explicitly when serving multiple Zoho tenants or data centers:

```csharp
HttpClient tenantClient = await zohoHttpClient.Get(
    tenantAccessToken,
    "https://www.zohoapis.eu/crm/v8/",
    cancellationToken);
```

The provider caches a separate client per access-token/base-URL pair and removes the clients it owns when disposed. It does not obtain or refresh OAuth tokens; Zoho access tokens expire and should be refreshed by the application.
