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
        private ConsolePrinter _printer;

        public ConsoleEnvironmentContext(ConsoleEnvironmentList list, ConsolePrinter printer)
        {
            _environmentList = list;
            _printer = printer;
            CurrentEnvironment = new ConsoleEnvironment(new string[0], new List<CommandWrapper>());
        }

        public void SwitchEnvironment(object? contextName)
        {
            var envArr = CurrentEnvironment.CurrentEnvironment.ToList();
            (contextName as string[]).ToList().ForEach(x => envArr.Add(x));

            var newEnvironment = _environmentList.Find(envArr.ToArray());

            CurrentEnvironment = newEnvironment;
            Print();
        }

        public bool CanSwitchEnvironment(object? contextName)
        {
            var envList = CurrentEnvironment.CurrentEnvironment.ToList();
            (contextName as string[]).ToList().ForEach(x => envList.Add(x));

            var newEnvironment = _environmentList.Find(envList.ToArray());

            if (newEnvironment is null)
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

            Print();
        }

        public bool CanDirEnvironment(object? parameter)
            => true;

        public void DirEnvironment(object? parameter)
        {
            Print(printer =>
            {
                _environmentList.ForEach(x =>
                {
                    printer.PrintList(x.CurrentEnvironment.ToList());
                });
            });
        }








        private void PrintPath()
        {
            _printer.PrintPath(CurrentEnvironment);
        }

        private void Print(Action<ConsolePrinter> action = null)
        {
            _printer.RequestPrint();
            action?.Invoke(_printer);
            _printer.PrintEmptyLine();
            PrintPath();
            _printer.PrintEnded();
        }
    }
}
