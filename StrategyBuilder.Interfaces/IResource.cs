using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Interfaces
{
    public interface IResource
    {
        string Name { get; }
        int Amount { get; }
    }
}
