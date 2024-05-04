using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Interfaces
{
    public interface IConsolePrinter
    {
        void PrintDictionary(Dictionary<string, string> dic);
        void PrintEmptyLine();
        void PrintEnded();
        void PrintLine(string text);
        void PrintList(List<string> strings);

        //todo needs a better fix than this shit
        //void PrintPath(StrategyBuilder.ConsoleController.Core.ConsoleEnvironment environment);
        void RequestPrint();
    }
}
