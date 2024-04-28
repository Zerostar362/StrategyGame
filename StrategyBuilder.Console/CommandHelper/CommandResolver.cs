using StragyBuilder.Shared.Input;
using StrategyBuilder.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StrategyBuilder.Console.CommandHelper
{
    public class CommandResolver : List<CommandWrapper>
    {
        public CommandResolver(IEnumerable<CommandWrapper> wrappers)
            : base(wrappers)
        {

        }


        public IEnumerable<CommandWrapper> GetListOfAvailableCommands(string[] environment)
        {
            if(environment.Length == 0)
            {
                return this;
            }

            return this.Where(wrapper =>
            {
                for (var i = 0; i < environment.Length; i++)
                {
                    if (wrapper.Environment[i] != environment[i])
                        return false;
                }

                return true;
            });
        }

        public IEnumerable<CommandWrapper> GetCommandsFromFilteredList(IEnumerable<CommandWrapper> commands,string[] environment)
        {
            return commands.Where(wrapper =>
            {
                for (var i = 0; i < environment.Length; i++)
                {
                    if (wrapper.Environment[i] != environment[i])
                        return false;
                }

                return true;
            });
        }
    }
}
