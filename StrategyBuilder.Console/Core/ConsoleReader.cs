using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StrategyBuilder.Console.System;
using StrategyBuilder.Interfaces;

namespace StrategyBuilder.ConsoleController.Core
{
    public class ConsoleReader : IHostedService
    {
        private CancellationTokenSource _cancellationTokenSource = new();
        private SystemCommand _systemCommand;
        private Task _readLoop;
        private object _lock = new object();

        private EventHandler _printStarted;
        private EventHandler _printStopped;
        private CancellationTokenSource _printerTokenSource;


        private ConsolePrinter Printer { get; set; }
        private ConsoleEnvironmentContext _environmentContext;


        public ConsoleReader(SystemCommand systemCommand, IConsolePrinter printer, ConsoleEnvironmentContext context, IHostApplicationLifetime lifetime)
        {
            _systemCommand = systemCommand;
            Printer = (ConsolePrinter)printer; //todo this fucking shit has grown everywhere, it needs to be fixed now
            _environmentContext = context;
            lifetime.ApplicationStopping.Register(() => { System.Console.WriteLine("ConsoleReader stopping"); });
            lifetime.ApplicationStopped.Register(() => { System.Console.WriteLine("ConsoleReader stopped"); });
            RegisterPrinter();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _readLoop = Task.Run(() =>
            {
                MainLoop();
            });

            Printer.PrintStarted += _printStarted;
            Printer.PrintFinished += _printStopped;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();

            Printer.PrintStarted -= _printStarted;
            Printer.PrintFinished -= _printStopped;

            return Task.CompletedTask;
        }

        public void MainLoop()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                var reader = "";
                lock (_lock)
                {
                    reader = System.Console.ReadLine();
                }
                if (reader == "")
                {
                    Thread.Sleep(1000);
                    continue;
                }

                if (reader is null)
                    break;

                var split = reader?.Split(" ");
                var arguments = split?.Skip(1).ToArray();
                _systemCommand.CheckAndTryExecute(split[0], arguments, arguments);
                Thread.Sleep(200);
            }
        }


        private void RegisterPrinter()
        {
            _printStarted = (object? sender, EventArgs e) =>
            {
                _printerTokenSource = new();
                Task.Run(() =>
                {
                    lock (_lock)
                    {
                        while (!_printerTokenSource.IsCancellationRequested)
                        {
                            Thread.Sleep(100);
                        }
                    }
                });
            };

            _printStopped += (object? sender, EventArgs e) =>
            {
                _printerTokenSource.Cancel();
            };
        }
    }

    public static class ConsoleImplementation
    {
        public static IServiceCollection AddConsoleService(this IServiceCollection services)
        {
            services.AddHostedService<ConsoleReader>();
            services.AddSingleton<ConsoleEnvironmentContext>();
            services.AddSingleton<IConsolePrinter,ConsolePrinter>();
            //services.AddTransient<ConsoleEnvironment>();

            return services;
        }
    }
}
