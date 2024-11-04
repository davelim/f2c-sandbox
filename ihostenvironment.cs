using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var env = host.Services.GetRequiredService<IHostEnvironment>();
        
        // You can now use the env object
        Console.WriteLine($"Environment: {env.EnvironmentName}");

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<MyService>();
            });
}

public class MyService
{
    private readonly IHostEnvironment _env;
    private readonly ILogger<MyService> _logger;

    public MyService(IHostEnvironment env, ILogger<MyService> logger)
    {
        _env = env;
        _logger = logger;
    }

    public void DoWork()
    {
        _logger.LogInformation($"Environment: {_env.EnvironmentName}");
    }
}
