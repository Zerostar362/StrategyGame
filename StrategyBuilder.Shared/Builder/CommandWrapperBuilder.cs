using Microsoft.Extensions.Logging;
using StrategyBuilder.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StrategyBuilder.Shared.Builder
{
    public class CommandWrapperBuilder
    {
        private List<CommandParameterOption> options = new();
        private string[] environment;
        private string cmdName;
        private Action<object?> command;

        private ILoggerFactory loggerFactory;
        public CommandWrapperBuilder(ILoggerFactory factory) 
        {
            loggerFactory = factory;
        }

        public CommandWrapperBuilder AddOption(CommandParameterOption option)
        {
            options.Add(option);
            return this;
        }

        public CommandWrapperBuilder SetEnvironment(string[]? environment = null)
        {
            this.environment = environment ?? new string[0];
            return this;
        }

        public CommandWrapperBuilder SetCommandName(string name)
        {
            cmdName = name;
            return this;
        }

        public CommandWrapperBuilder SetCommand(Action<object?> executeAction)
        {
            this.command = command;
            return this;
        }

        public CommandWrapper Build()
        {
            var logger = loggerFactory.CreateLogger<CommandWrapper>();
            return new CommandWrapper(environment, cmdName, options, command, logger);
        }
    }
}
