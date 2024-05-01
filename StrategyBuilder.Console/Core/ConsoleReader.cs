using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StrategyBuilder.Console.System;

namespace StrategyBuilder.ConsoleController.Core
{
    public class ConsoleReader : IHostedService
    {
        private CancellationTokenSource _cancellationTokenSource = new();
        private SystemCommand _systemCommand;
        private Task _readLoop;
        

        public ConsoleReader(SystemCommand systemCommand)
        {
            _systemCommand = systemCommand;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _readLoop = Task.Run(() =>
            {
                MainLoop();
            });
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }

        public void MainLoop()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                var reader = System.Console.ReadLine();
                var split = reader.Split(" ");
                var arguments = split.Skip(1).ToArray();
                _systemCommand.CheckAndTryExecute(split[0], arguments, arguments);
            }
        }
    }

    public static class ConsoleImplementation
    {
        public static IServiceCollection AddConsoleService(this IServiceCollection services)
        {
            services.AddHostedService<ConsoleReader>();
            services.AddSingleton<ConsoleEnvironmentContext>();
            //services.AddTransient<ConsoleEnvironment>();

            return services;
        }
    }
}
