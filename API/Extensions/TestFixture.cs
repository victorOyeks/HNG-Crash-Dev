using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace API.Extensions;

public class TestFixture : IDisposable
{
    public HttpClient Client { get; private set; }

    public TestFixture()
    {
        var factory = new WebApplicationFactory<Program>();
        Client = factory.CreateClient();
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}
