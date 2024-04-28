using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Shared.Wrapper
{
    public class CommandParameterOption
    {
        string[] options_names;
        public CommandParameterOption(string[] optionNames)
        {
            options_names = optionNames;
        }
    }
}
