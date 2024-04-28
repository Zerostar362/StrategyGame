using StrategyBuilder.Console.CommandHelper;
using StrategyBuilder.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Console
{
    public class ConsoleEnvironment
    {
        private string[] CurrentEnvironment { get; set; }

        private IEnumerable<CommandWrapper> AvailableCommands { get; set; }

        public ConsoleEnvironment(string[] environment, CommandResolver resolver) 
        {
            CurrentEnvironment = environment;
            GetCommands(resolver);
        }

        private void GetCommands(CommandResolver resolver) =>
            AvailableCommands = resolver.GetListOfAvailableCommands(CurrentEnvironment);
    }
}
