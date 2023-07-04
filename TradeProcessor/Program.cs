// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TradeProcessor;
using Microsoft.Extensions.Configuration;
using TradeProcessor.Repositories;
using TradeProcessor.Services;

// Application code should start here.

Console.WriteLine("Starting application");
BuildApp();

void BuildApp()
{
    // Create service collection and configure our services
    var services = ConfigureServices();
    // Generate a provider
    var serviceProvider = services.BuildServiceProvider();

    // Run the code
    serviceProvider.GetService<TradeProcessorApplication>().Run();
}

static IServiceCollection ConfigureServices()
{
    IServiceCollection services = new ServiceCollection();
    // Set up the objects we need to get to configuration settings
    var config = LoadConfiguration();
    // Add the config to our DI container for later use
    services.AddSingleton(config);
    services.AddTransient<ITradesReader, TradesReader>();
    services.AddTransient<ITradesRepository, TradesRepository>();
    services.AddTransient<ITradesProcessorService, TradesProcessorService>();
    // Register our application entry point
    services.AddTransient<TradeProcessorApplication>();
    return services;
}

static IConfiguration LoadConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true,
                     reloadOnChange: true);
    return builder.Build();
}
