using Soenneker.Zoho.HttpClients.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Zoho.HttpClients.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class ZohoOpenApiHttpClientTests : HostedUnitTest
{
    private readonly IZohoOpenApiHttpClient _httpclient;

    public ZohoOpenApiHttpClientTests(Host host) : base(host)
    {
        _httpclient = Resolve<IZohoOpenApiHttpClient>(true);
    }

    [Test]
    public void Default()
    {

    }
}
