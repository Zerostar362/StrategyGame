using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StragyBuilder.Shared.Input;
using StrategyBuilder.ConsoleController.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StrategyBuilder.Console.System
{
    public class SystemCommand
    {
        private IDictionary<string, ICommand> SystemCommands { get; init; }
        public SystemCommand(IDictionary<string, ICommand> systemCommands)
        {
            SystemCommands = systemCommands;
            var iniCmd = SystemCommands as CommandDictionary<string, ICommand>;
            iniCmd.Init();
        }

        public bool IsSystemCommand(string name) =>
            SystemCommands.ContainsKey(name);

        public bool CanExecuteCommand(string name, object? parameter)
        {
            var command = SystemCommands[name];
            return command?.CanExecute(parameter) ?? false;
        }

        public void ExecuteCommand(string name, object? parameter)
        {
            var command = SystemCommands[name];
            command?.Execute(parameter);
        }

        public bool CheckAndTryExecute(string name, object? canExecuteParam, object? executeParam)
        {
            if (name.Contains("./"))
            {
                name = new string(name.Skip(2).ToArray());
                var cep = canExecuteParam as string[];
                var ep = executeParam as string[];

                cep = cep.Prepend(name).ToArray();
                ep = ep.Prepend(name).ToArray();

                var command = SystemCommands["./*"];

                if (!command.CanExecute(cep))
                    return false;

                command.Execute(ep);

                return true;
            }


            if (!IsSystemCommand(name))
                return false;

            if (!CanExecuteCommand(name, canExecuteParam))
                return false;

            ExecuteCommand(name, executeParam);

            return true;
        }
    }

    public class CommandDictionary<TKey, TValue> : Dictionary<string, ICommand>
    {
        private IServiceProvider _serviceProvider;
        private bool initialized;
        public CommandDictionary(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        public void Init()
        {
            initialized = true;
            _serviceProvider.GetServices<ICommand>();
        }
    }

    public static class SystemCommandImplementation
    {
        public static IServiceCollection AddSystemCommands(this IServiceCollection services)
        {
            services.AddSingleton<ConsoleEnvironmentList>();
            services.AddSingleton<SystemCommand>();
            services.AddSingleton<IDictionary<string, ICommand>, CommandDictionary<string, ICommand>>();
            services.AddSingleton<ICommand, Command>(provider =>
            {
                var context = provider.GetService<ConsoleEnvironmentContext>();
                var dictionary = provider.GetService<IDictionary<string, ICommand>>();

                if (dictionary is null)
                    throw new NullReferenceException("SystemCommand dictionary was null");
                if (context is null)
                    throw new NullReferenceException("ConsoleEnvironmentContext was null");

                var command = new Command((object? parameter) => context.SwitchEnvironment(parameter), (object? parameter) => context.CanSwitchEnvironment(parameter));
                dictionary.Add("cd", command);
                return command;
            });

            services.AddSingleton<ICommand, Command>(provider =>
            {
                var context = provider.GetService<ConsoleEnvironmentContext>();
                var dictionary = provider.GetService<IDictionary<string, ICommand>>();

                if (dictionary is null)
                    throw new NullReferenceException("SystemCommand dictionary was null");
                if (context is null)
                    throw new NullReferenceException("ConsoleEnvironmentContext was null");

                var command = new Command((object? paramter) => context.DirCommands(paramter), (object? parameter) => context.CanDirCommands(parameter));
                dictionary.Add("dirCmd", command);
                return command;
            });

            services.AddSingleton<ICommand, Command>(provider =>
            {
                var context = provider.GetService<ConsoleEnvironmentContext>();
                var dictionary = provider.GetService<IDictionary<string, ICommand>>();

                if (dictionary is null)
                    throw new NullReferenceException("SystemCommand dictionary was null");
                if (context is null)
                    throw new NullReferenceException("ConsoleEnvironmentContext was null");

                var command = new Command((object? parameter) => context.DirEnvironment(parameter), (object? parameter) => context.CanDirEnvironment((object?)parameter));
                dictionary.Add("dir", command);
                return command;
            });

            services.AddSingleton<ICommand, Command>(provider =>
            {
                var context = provider.GetService<ConsoleEnvironmentContext>();
                var dictionary = provider.GetService<IDictionary<string, ICommand>>();

                if (dictionary is null)
                    throw new NullReferenceException("SystemCommand dictionary was null");
                if (context is null)
                    throw new NullReferenceException("ConsoleEnvironmentContext was null");

                var command = new Command((object? parameter) => context.ExecuteGameCommand(parameter), (object? parameter) => context.CanExecuteGameCommand((object?)parameter));
                dictionary.Add("./*", command);
                return command;
            });

            services.AddSingleton<ICommand, Command>(provider =>
            {
                var dictionary_interface = provider.GetService<IDictionary<string, ICommand>>();
                var dic = dictionary_interface as Dictionary<string, ICommand> ?? throw new Exception();
                var printer = provider.GetService<StrategyBuilder.Interfaces.IConsolePrinter>();

                var command = new Command((object? parameter) =>
                {
                    printer.RequestPrint();

                    foreach (var command in dic)
                    {
                        printer.PrintLine(command.Key);
                        printer.PrintEmptyLine();
                    }

                    printer.PrintEnded();
                }, 
                (object? parameter) => true);

                dictionary_interface.Add("help", command);
                dictionary_interface.Add("?", command);

                return command;
            });

            services.AddSingleton<ICommand, Command>(provider =>
            {
                var dic = provider.GetService<IDictionary<string, ICommand>>();

                var command = new Command((object? parameter) =>
                {
                    var host = provider.GetService<IHost>();
                    var task = Task.Run(() => host.StopAsync());
                    task.Wait();
                    Environment.Exit(0);

                },(object? parameter) => true);

                dic.Add("exit", command); 
                return command;
            });

            return services;
        }
    }
}
