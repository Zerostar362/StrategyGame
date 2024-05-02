using StrategyBuilder.Console.CommandHelper;
using StrategyBuilder.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.ConsoleController.Core
{
    public class ConsoleEnvironment
    {
        public string[] CurrentEnvironment { get; private set; }

        private IEnumerable<CommandWrapper> AvailableCommands { get; set; }

        public ConsoleEnvironment(string[] environment, IEnumerable<CommandWrapper> cmds) 
        {
            CurrentEnvironment = environment;
            AvailableCommands = cmds;
            //GetCommands(resolver);
        }

        public IEnumerable<string> ShowListOfCommands()
        {
            foreach (var cmd in AvailableCommands)
            {
                if(CurrentEnvironment == cmd.Environment)
                {
                    var sb = new StringBuilder();
                    sb.Append(cmd.CommandName); 
                    sb.Append(" ");
                    cmd.AvailableArguments.ForEach(arg => sb.Append($"{arg} "));
                    yield return sb.ToString();
                }
            }
        }

        public bool CanExecuteCommand(string cmdName, object? parameter)
        {
            var cmd = AvailableCommands.Where(x => x.CommandName == cmdName).FirstOrDefault();

            return cmd?.Command.CanExecute(parameter) ?? false;
        }

        public void ExecuteCommand(string cmdName, object? parameters)
        {
            var cmd = AvailableCommands.Where(x => x.CommandName == cmdName).First();

            cmd.Command.Execute(parameters);
        }

        private void GetCommands(CommandResolver resolver) =>
            AvailableCommands = resolver.GetListOfAvailableCommands(CurrentEnvironment);
    }
}
