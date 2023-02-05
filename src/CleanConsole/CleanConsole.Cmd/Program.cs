using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CleanConsole.Domain.Utilities;
//using static BindingCmdLineEvents;
using CleanConsole.Infrastructure.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

const string AppConfigurationConnectionStringEnvVarName = "AppConfigurationConnectionString";
string appConfigurationConnectionString = Environment.GetEnvironmentVariable(AppConfigurationConnectionStringEnvVarName);
IConfigurationRefresher _refresher = null;

await BuildCommndLine().UseHost(_ => Host.CreateDefaultBuilder(),
    host =>
    {
        HostBuilderContext ctx = new(host.Properties);

        host.ConfigureAppConfiguration(s =>
        {
            s.AddAzureAppConfiguration(options =>
            {
                options.Connect(appConfigurationConnectionString);
                options.ConfigureRefresh(refresh =>
                {
                    refresh.Register("TestApp:Settings:Message")
                    .SetCacheExpiration(TimeSpan.FromSeconds(5));
                });
                options.UseFeatureFlags(f =>
                {
                    f.CacheExpirationInterval = TimeSpan.FromSeconds(5);
                });

                _refresher = options.GetRefresher();
            });
        });

        host.ConfigureServices(services =>
        {
            services.AddDomainServices();
            services.AddInfrastructureServices(ctx.Configuration);
        });
    })
    .UseDefaults()
    .Build()
    .InvokeAsync(args);

static CommandLineBuilder BuildCommndLine()
{
    var root = new RootCommand(@"$ BindingCmdLine --name 'Joe'")
    {
        new Option<string>("--name")
        {
            IsRequired = true
        }
    };

    root.Handler = CommandHandler.Create<GreeterOptions, IHost>(Run);
    return new CommandLineBuilder(root);
}

static void Run(GreeterOptions options, IHost host)
{
    var serviceProvider = host.Services;
    var greeter = serviceProvider.GetRequiredService<IGreeter>();
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<Program>();

    logger.LogInformation(GreetEvent, "Greeting was requested for: {name}", options.Name);
    greeter.Greet(options.Name);
}