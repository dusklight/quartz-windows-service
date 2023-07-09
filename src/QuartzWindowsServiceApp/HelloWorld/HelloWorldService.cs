using Microsoft.Extensions.Options;

namespace QuartzWindowsServiceApp.HelloWorld;

public class HelloWorldService : IHelloWorldService
{
    private readonly ILogger<HelloWorldService> _log;
    private readonly HelloWorldServiceOptions _options;

    public HelloWorldService(ILogger<HelloWorldService> log, IOptions<HelloWorldServiceOptions> options)
    {
        _log = log;
        _options = options.Value;
    }

    public string GetMessage()
    {
        return _options.MessageText ?? "";
    }
}
