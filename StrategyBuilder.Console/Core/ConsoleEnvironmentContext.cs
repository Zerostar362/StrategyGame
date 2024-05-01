using StrategyBuilder.Console.CommandHelper;
using StrategyBuilder.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StrategyBuilder.ConsoleController.Core
{
    /// <summary>
    /// Takes care about hierarchy
    /// </summary>
    public sealed class ConsoleEnvironmentContext
    {
        public ConsoleEnvironment CurrentEnvironment { get; private set; }
        private ConsoleEnvironmentList _environmentList;
        //private CommandResolver Resolver { get; set; }

        public ConsoleEnvironmentContext(/*CommandResolver resolver*/ConsoleEnvironmentList list) 
        {
            _environmentList = list;
          //Resolver = resolver;
          CurrentEnvironment = new ConsoleEnvironment(new string[0], new List<CommandWrapper>());
        }

        public void SwitchEnvironment(object? contextName)
        {
            var envArr = CurrentEnvironment.CurrentEnvironment.ToList();
            (contextName as string[]).ToList().ForEach(x => envArr.Add(x));

            var newEnvironment = _environmentList.Find(envArr.ToArray());

            CurrentEnvironment = newEnvironment;
        }

        public bool CanSwitchEnvironment(object? contextName)
        {
            var envList = CurrentEnvironment.CurrentEnvironment.ToList();
            (contextName as string[]).ToList().ForEach(x => envList.Add(x));

            var newEnvironment = _environmentList.Find(envList.ToArray());

            if(newEnvironment is null)
                return false;

            return true;

        }

        public bool CanDirCommands(object? paramater)
            => true;


        public void DirCommands(object? paramter)
        {
            var list = CurrentEnvironment.ShowListOfCommands();
            foreach (var commandName in list)
            {
                System.Console.WriteLine(commandName);
            }
        }

        public bool CanDirEnvironment(object? parameter)
            => true;

        public void DirEnvironment(object? parameter)
        {
            _environmentList.ForEach(x =>
            {
                x.CurrentEnvironment.ToList().ForEach(y => System.Console.WriteLine(y));
            });
        }


        private bool TryExecuteCommand(string input)
        {
            //Execute is anything send with "{Prefix} {some_text}"
            //complete execution command with parameters:
            //"{Prefix} {command_name} -f {parameter}"
            //"{Prefix} {command_name} -f {parameter} -r {parameter}"

            throw new NotImplementedException();
        }
    }
}
