// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TradeProcessor;
using Microsoft.Extensions.Configuration;
using TradeProcessor.Repositories;
using TradeProcessor.Services;
using Microsoft.ApplicationInsights.WorkerService;

// Application code should start here.

Console.WriteLine("Starting application");
BuildApp();

void BuildApp()
{
    // Create application and configure services
    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
    var app = ConfigureServices(builder);

    // Run the code
    IHost host = builder.Build();
    host.Run();
}

static HostApplicationBuilder ConfigureServices(HostApplicationBuilder builder)
{
    // Set up the objects to get to configuration settings
    var config = LoadConfiguration();
    // Add the config to DI container for later use
    builder.Services.AddSingleton(config);
    builder.Services.AddTransient<ITradesReader, TradesReader>();
    builder.Services.AddTransient<ITradesRepository, TradesRepository>();
    builder.Services.AddTransient<ITradesProcessorService, TradesProcessorService>();
    // Register our application entry point
    builder.Services.AddHostedService<TradeProcessorApplication>();
    return builder;
}

static IConfiguration LoadConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true,
                     reloadOnChange: true);
    return builder.Build();
}
