using Quartz;

namespace QuartzWindowsServiceApp.HelloWorld;

internal class HelloWorldJob : IJob
{
    private readonly ILogger<HelloWorldJob> _log;
    private readonly IHelloWorldService _helloWorldService;

    public HelloWorldJob(ILogger<HelloWorldJob> log, IHelloWorldService helloWorldService)
    {
        _log = log;
        _helloWorldService = helloWorldService;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _log.LogInformation("Executing {jobName}", nameof(HelloWorldJob));

        var message = _helloWorldService.GetMessage();

        _log.LogInformation("Message from the service: {message}", message);

        return Task.CompletedTask;
    }
}
