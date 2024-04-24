using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Interfaces
{
    public interface ICommand : System.Windows.Input.ICommand
    {
        string[] Flag { get; }
        string Description { get; init; }
    }
}
