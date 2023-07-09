using Quartz;
using QuartzWindowsServiceApp.HelloWorld;
using Serilog;

namespace QuartzWindowsServiceApp;

public class Program
{
    const string WINDOWS_SERVICE_NAME = "QuartzWindowsServiceDemo";
    const string QUARTZ_SCHEDULER_ID = "MainQuartzScheduler";

    public static void Main(string[] args)
    {
        // Create a logger that can be used here as we setup the host. (https://github.com/serilog/serilog-aspnetcore#two-stage-initialization)
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        IHost host = Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(context.Configuration);
            })
            .ConfigureServices((context, services) =>
            {
                ConfigureOptions(context, services);

                ConfigureQuartz(context, services);

                AddServices(services);
            })
            .UseWindowsService(options =>
            {
                options.ServiceName = WINDOWS_SERVICE_NAME;
            })
            .Build();

        try
        {
            Log.Information("Running the host.");

            host.Run();

            Log.Information("Host has ended.");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "{WindowsServiceName} terminated unexpectedly.", WINDOWS_SERVICE_NAME);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// Demonstrates how to provide configurable settings using the <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/options">Options</see>
    /// pattern.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="services"></param>
    private static void ConfigureOptions(HostBuilderContext context, IServiceCollection services)
    {
        var configRoot = context.Configuration;

        services.Configure<SchedulerOptions>(configRoot.GetSection(nameof(SchedulerOptions)));
        services.Configure<HelloWorldServiceOptions>(configRoot.GetSection(nameof(HelloWorldServiceOptions)));
    }

    /// <summary>
    /// Configures Quartz and its jobs.
    /// </summary>
    /// <param name="services"></param>
    private static void ConfigureQuartz(HostBuilderContext context, IServiceCollection services)
    {
        // Need to retrieve the scheduler options directly here, since the DI is not built yet.
        var schedulerOptions = new SchedulerOptions();
        context.Configuration.Bind(nameof(SchedulerOptions), schedulerOptions);

        services.AddQuartz(q =>
        {
            q.SchedulerId = QUARTZ_SCHEDULER_ID;

            q.UseMicrosoftDependencyInjectionJobFactory();

            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();
            q.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = schedulerOptions.MaxConcurrency;
            });

            q.ScheduleJob<HelloWorldJob>(
                triggerConfigurator => triggerConfigurator
                    .WithIdentity(nameof(HelloWorldJob))
                    .WithSimpleSchedule(s => s
                        .WithIntervalInSeconds(schedulerOptions.HelloWorldJobIntervalInSeconds)
                        .RepeatForever())
                    .StartNow(),
                jobConfigurator => jobConfigurator
                    .WithIdentity(nameof(HelloWorldJob))
            );
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }

    /// <summary>
    /// Adds services to DI.
    /// </summary>
    /// <param name="services"></param>
    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient<HelloWorldJob>();

        services.AddTransient<IHelloWorldService, HelloWorldService>();
    }
}

