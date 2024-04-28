using StrategyBuilder.Console.CommandHelper;
using StrategyBuilder.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Console
{
    /// <summary>
    /// Takes care about hierarchy
    /// </summary>
    public sealed class ConsoleEnvironmentContext
    {
        public ConsoleEnvironment Environment { get; private set; }
        private CommandResolver Resolver { get; set; }

        public ConsoleEnvironmentContext(CommandResolver resolver) 
        {
            Resolver = resolver;
            Environment = new ConsoleEnvironment(new string[0], resolver);
        }

        public bool TryExecute()
        {
            throw new NotImplementedException();

            //Some input parser
            //var splitInput = input.Split(" ")
            //if(splitInput.Contains == {Prefix})
            //   TryExecuteCommand
            //else
            //   TrySwitchEnvironment
        }

        private bool TrySwitchEnvironment()
        {
            throw new NotImplementedException();
        }

        private bool TryExecuteCommand(string input)
        {
            //Execute is anything send with "{Prefix} {some_text}"
            //complete execution command with parameters:
            //"{Prefix} {command_name} -f {parameter}"
            //"{Prefix} {command_name} -f {parameter} -r {parameter}"

            
        }
    }
}
