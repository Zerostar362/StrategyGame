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
        private object _lock = new object();

        private ConsolePrinter Printer { get; set; }
        private CancellationTokenSource _printerTokenSource;


        public ConsoleReader(SystemCommand systemCommand, ConsolePrinter printer)
        {
            _systemCommand = systemCommand;
            Printer = printer;
            RegisterPrinter();
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
                var reader = "";
                lock (_lock)
                {
                    reader = System.Console.ReadLine();
                }
                if (reader == "")
                    Thread.Sleep(1000);

                var split = reader.Split(" ");
                var arguments = split.Skip(1).ToArray();
                _systemCommand.CheckAndTryExecute(split[0], arguments, arguments);
                Thread.Sleep(200);
            }
        }


        private void RegisterPrinter()
        {
            Printer.PrintStarted += (object sender, EventArgs e) =>
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

            Printer.PrintFinished += (object sender, EventArgs e) =>
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
            services.AddSingleton<ConsolePrinter>();
            //services.AddTransient<ConsoleEnvironment>();

            return services;
        }
    }
}
