using StrategyBuilder.Console.CommandHelper;
using StrategyBuilder.Interfaces;
using StrategyBuilder.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

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

        public ConsoleEnvironmentContext(ConsoleEnvironmentList list, IConsolePrinter printer)
        {
            _environmentList = list;
            _printer = (ConsolePrinter)printer; //this is shit and needs to be fixed
            CurrentEnvironment = new ConsoleEnvironment(new string[0], new List<CommandWrapper>());
        }

        public void SwitchEnvironment(object? contextName)
        {
            var envArr = CurrentEnvironment.CurrentEnvironment.ToList();
            (contextName as string[]).ToList().ForEach(x => envArr.Add(x));

            var newEnvironment = _environmentList.Find(envArr.ToArray());

            CurrentEnvironment = newEnvironment;
            PrintWithClear();
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
            _printer.RequestPrint();

            foreach (var commandName in list)
            {
                _printer.PrintLine(commandName);
                //System.Console.WriteLine(commandName);
            }

            _printer.PrintEnded();
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

        public bool CanExecuteGameCommand(object? parameter)
        {
            var strParams = parameter as string[];
            
            if(strParams is null)
                return false;

            var name = strParams[0];
            var param = strParams.Skip(1).ToArray();

            return CurrentEnvironment.CanExecuteCommand(name,param);
        }

        public void ExecuteGameCommand(object? parameter)
        {
            //after returning true in CanExecute in CommandWrapper, it will crash here, so my desparate state is going even deeper :(
            var strParams = parameter as string[];

            var name = strParams[0];
            var param = strParams.Skip(1).ToArray();

            CurrentEnvironment.ExecuteCommand(name, param);

            Print();
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

        private void PrintWithClear(Action<ConsolePrinter> action = null)
        {
            _printer.ClearConsole();
            _printer.RequestPrint();
            action?.Invoke(_printer);
            _printer.PrintEmptyLine();
            PrintPath();
            _printer.PrintEnded();
        }
    }
}
