# jaytwo.SolutionResolution

[![NuGet version (jaytwo.SolutionResolution)](https://img.shields.io/nuget/v/jaytwo.SolutionResolution.svg?style=flat-square)](https://www.nuget.org/packages/jaytwo.SolutionResolution/)

When I'm setting up integration tests in AspNetCore, the content root is all messed up because `Directory.GetCurrentDirectory()` 
resolves to something like `./test/MySolution.Web.Tests/bin/Debug/netcoreapp2.1/`, and the most reliable way to get it back to the
expected `./src/MySolution.Web/` is to traverse directories up until you find a `.sln` file, and then find some known path relative
to the folder in which the `.sln` file lives.

The code is simple, but I got tired of the copy pasta whenever I set up an integration test project. Bingo bango, the `SolutionResolution` was born.

## Installation

Add the NuGet package

```
PM> Install-Package jaytwo.SolutionResolution
```

## Normal Usage

### AspNetCore 1.1 Integration Tests

```cs
public class TestServerFixture
{
    private readonly TestServer _server;
    
    public HttpClient CreateClient() => _server.CreateClient();

    public TestServerFixture()
    {
        var contentRoot = new SlnFileResolver().ResolvePathRelativeToSln("examples/AspNetCore1_1");

        _server = new TestServer(new WebHostBuilder()
            .UseEnvironment("Development")
            .UseContentRoot(contentRoot)
            .UseStartup<Startup>());
    }

    public void Dispose()
    {
        _server?.Dispose();
    }
}
```

### AspNetCore 2.1 Integration Tests

```cs
public class WebApplicationFactory
    : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        var contentRoot = new SlnFileResolver().ResolvePathRelativeToSln("examples/AspNetCore2_1");
        builder.UseContentRoot(contentRoot);
    }
}
```

---

Made with &hearts; by Jake
