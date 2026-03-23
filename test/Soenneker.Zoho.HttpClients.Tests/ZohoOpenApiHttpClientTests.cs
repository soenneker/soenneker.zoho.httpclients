using Soenneker.Zoho.HttpClients.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Zoho.HttpClients.Tests;

[Collection("Collection")]
public sealed class ZohoOpenApiHttpClientTests : FixturedUnitTest
{
    private readonly IZohoOpenApiHttpClient _httpclient;

    public ZohoOpenApiHttpClientTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _httpclient = Resolve<IZohoOpenApiHttpClient>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
