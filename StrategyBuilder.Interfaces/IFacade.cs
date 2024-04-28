using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Interfaces
{
    public interface IFacade
    {
        int GetResourceAmount<T>() where T : IResource;
        Dictionary<string, int>? GetResourceAmount_All();
    }
}
