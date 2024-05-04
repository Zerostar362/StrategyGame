using StrategyBuilder.DTO.LocalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Interfaces
{
    public interface IFacade
    {
        Response GetResourceAmount<T>() where T : IResource;
        Response GetResourceAmount_All();
    }
}
