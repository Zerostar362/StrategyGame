using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StragyBuilder.Core.Debug;
using StragyBuilder.Shared.Input;
using StrategyBuilder.Buildings.Implementation;
using StrategyBuilder.Resources.Implementation;

namespace StragyBuilder.Core
{
    public class GameHost
    {
        private HostApplicationBuilder builder;


        public IServiceCollection Services { get => builder.Services; }
        public ILoggingBuilder Logging { get => builder.Logging; }
        public IHostEnvironment Environment { get => builder.Environment; }
        public IMetricsBuilder Metrics { get => builder.Metrics; }
        public IConfiguration Configuration { get => builder.Configuration; }


        private GameHost(HostApplicationBuilder builder)
        {
            this.builder = builder;
        }

        public static GameHost CreateHost()
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Logging.ClearProviders();
#if DEBUG
            builder.Services.AddDebugCommands();
#endif


            builder.Services.AddHostedService<GameCore>();
            builder.Services.AddHostedService<CommandResolver>();
            builder.Services.AddResources();
            builder.Services.AddBuildings();
            return new GameHost(builder);
        }

        public void Run()
        {
            var app = builder.Build();
            app.Run();
        }
    }
}
