using Microsoft.Extensions.DependencyInjection;
using StragyBuilder.Shared.Input;
using StrategyBuilder.ConsoleController.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StrategyBuilder.Console.System
{
    public class SystemCommand
    {
        private IDictionary<string, ICommand> SystemCommands { get; init; }
        public SystemCommand(IDictionary<string,ICommand> systemCommands)
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
            if (!IsSystemCommand(name))
                return false;

            if (!CanExecuteCommand(name, canExecuteParam))
                return false;

            ExecuteCommand(name, executeParam);

            return true;
        }
    }

    public class CommandDictionary<TKey,TValue> : Dictionary<string, ICommand>
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

                var command =  new Command((object? parameter) => context.SwitchEnvironment(parameter),(object? parameter) => context.CanSwitchEnvironment(parameter));
                dictionary.Add("cd",command);
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

            return services;
        }
    }
}
