using Microsoft.VisualBasic;
using StrategyBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.ConsoleController.Core
{
    public class ConsolePrinter
    {
        public event EventHandler PrintStarted;
        public event EventHandler PrintFinished;

        private bool _isRequested;
        
        public ConsolePrinter() 
        {
            
        }

        public void RequestPrint()
        {
            PrintStarted?.Invoke(this, EventArgs.Empty);
            _isRequested = true;
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
            _isRequested= false;
        }
    }
}
