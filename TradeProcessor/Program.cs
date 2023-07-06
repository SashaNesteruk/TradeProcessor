// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TradeProcessor;
using Microsoft.Extensions.Configuration;
using TradeProcessor.Repositories;
using TradeProcessor.Services;
using TradeProcessor.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Microsoft.Extensions.Logging;

// Application code entry point
Log.Logger = new LoggerConfiguration()
    .CreateLogger();

Log.Information("Starting application");
BuildApp();

void BuildApp()
{
    // Create application and configure services
    var builder = Host.CreateApplicationBuilder(args);
    var app = ConfigureServices(builder);

    // Configure Logger
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(app.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);

    app.Logging.ClearProviders();
    app.Logging.AddSerilog(logger);

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

    // Add Context
    builder.Services.AddDbContext<TradesContext>
   (opts => opts.UseSqlite(config.GetConnectionString("Trades_db")));

    // Register application entry point
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
