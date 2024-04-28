using Microsoft.Extensions.Logging;
using StragyBuilder.Shared.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StrategyBuilder.Shared.Wrapper
{
    public class CommandWrapper
    {
        public string[] Environment { get; private set; }
        public string CommandName { get; private set; }
        public List<CommandParameterOption> AvailableArguments {  get; private set; }
        public ICommand Command { get; private set; }

        private ILogger<CommandWrapper> Logger { get; init; }

        public CommandWrapper(
            string[] environment,
            string cmdName, 
            List<CommandParameterOption> options,
            Action<object?> cmdExecute,
            ILogger<CommandWrapper> logger)
        {
            Environment = environment;
            CommandName = cmdName;
            AvailableArguments = options;
            Logger = logger;

            var cmd = new Command(cmdExecute, CanExecute);

            this.Command = cmd;
        }

        private bool CanExecute(object? obj)
        {
            try
            {
                var environment = (string[])obj ?? throw new NullReferenceException();
                if (environment == Environment)
                    return true;
            }
            catch(NullReferenceException ex)
            {
                Logger.LogWarning("CanExecute could not be created, cause string[] environment parameter was missing");
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.ToString());
                return false;
            }

            return false;
        }
    }
}
