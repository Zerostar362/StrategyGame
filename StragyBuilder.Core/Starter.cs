using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StragyBuilder.Core.Debug;
using StrategyBuilder.Buildings.Implementation;
using StrategyBuilder.Console.CommandHelper;
using StrategyBuilder.Console.System;
using StrategyBuilder.ConsoleController.Core;
using StrategyBuilder.Interfaces;
using StrategyBuilder.Resources.Implementation;
using StrategyBuilder.Shared.Wrapper;

namespace StragyBuilder.Core
{
    public class GameHost
    {
        private HostApplicationBuilder builder;
#if DEBUG
        public IHost Host { get; set; }
#endif

        public IServiceCollection Services { get => builder.Services; }
        public ILoggingBuilder Logging { get => builder.Logging; }
        public IHostEnvironment Environment { get => builder.Environment; }
        public IMetricsBuilder Metrics { get => builder.Metrics; }
        public IConfiguration Configuration { get => builder.Configuration; }


        private GameHost(HostApplicationBuilder builder)
        {
            this.builder = builder;
        }

        public static GameHost CreateHost(string[] args)
        {
            var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();

            builder.Logging.ClearProviders();
#if DEBUG
            builder.Services.AddDebugCommands();
#endif


            builder.Services.AddHostedService<GameCore>();
            //builder.Services.AddHostedService<ConsolePrinter>();
            builder.Services.AddConsoleService();
            //builder.Services.AddHostedService<CommandResolver>();
            builder.Services.AddSingleton<IFacade, Facade>();


            builder.Services.AddSystemCommands();

            builder.Services.AddSingleton<CommandResolver>(provider =>
            {
                var wrappers = provider.GetServices<CommandWrapper>();
                return new StrategyBuilder.Console.CommandHelper.CommandResolver(wrappers);
            });
            builder.Services.AddResources();
            builder.Services.AddBuildings();
            return new GameHost(builder);
        }

        public void Run()
        {
            var app = builder.Build();
#if DEBUG
            Host = app;
#endif
            var lifetime = app.Services.GetService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(() => Console.WriteLine("Application started"));
            app.Run();
        }
    }
}
