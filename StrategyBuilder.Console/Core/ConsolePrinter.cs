using Microsoft.VisualBasic;
using StrategyBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.ConsoleController.Core
{
    public class ConsolePrinter : IConsolePrinter
    {
        public event EventHandler PrintStarted;
        public event EventHandler PrintFinished;

        private bool _isRequested;
        
        //Console printer must be more autonomous
        //That means that it should do most of the render thing by itself

        //other class must only ask him to print certain information, but they should not know how/when or if it will be printed
        
        //ConsolePrinter will have an interface API to create more complex printables for command to create

        //1. It should print the environment by itself
        //that means that ConsolePrinter needs to be injected with ConsoleEnvironmentContext

        //2. No internal print functioning will be presented out of the class. That means request and end print for blocking reading.
        
        //3. Only one method should be called from outside. TM Print(Action<InterfacePrintAPI> action).

        public ConsolePrinter() 
        {
            
        }

        public void RequestPrint()
        {
            PrintStarted?.Invoke(this, EventArgs.Empty);
            _isRequested = true;
        }

        public void PrintLine(string text)
        {
            if (!_isRequested)
                throw new InvalidOperationException("Not under request");

            System.Console.WriteLine(text);
        }
        public void ClearConsole()
        {
            System.Console.Clear();
        }

        public void PrintPath(ConsoleEnvironment environment)
        {
            if (!_isRequested)
                throw new InvalidOperationException("Not under request");


            var sb = new StringBuilder();
            
            foreach (var e in environment.CurrentEnvironment)
            {
                sb.Append(e);
                sb.Append("/");
            }

            sb.Append(" ");
            System.Console.Write(sb.ToString());
        }

        public void PrintEmptyLine() =>
            System.Console.WriteLine();

        public void PrintList(List<string> strings)
        {
            if (!_isRequested)
                throw new InvalidOperationException("Not under request");

            var sb = new StringBuilder();

            foreach (var item in strings)
            {
                sb.Append(item);
                sb.Append(Environment.NewLine);
            }

            System.Console.Write(sb.ToString());
        }

        public void PrintDictionary(Dictionary<string,string> dic)
        {
            if (!_isRequested)
                throw new InvalidOperationException("Not under request");

            var sb = new StringBuilder();

            foreach (var item in dic)
            {
                sb.Append(item.Key);
                sb.Append(": ");
                sb.Append(item.Value);
                sb.Append(Environment.NewLine);
            }

            System.Console.Write(sb.ToString());
        }


        public void PrintEnded()
        {
            PrintFinished.Invoke(this, EventArgs.Empty);
            //System.Console.Clear();
            _isRequested= false;
        }
    }
}
