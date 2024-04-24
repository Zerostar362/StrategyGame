using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Interfaces
{
    public interface IResourceManager
    {
        void AddResources(IEnumerable<IResource> resources);
        int GetResourceAmount<T>() where T : IResource;
        bool IsEnoughResources(List<IResource> upgradeDemands);
        void PrintAllResources(object? parameters);
        bool TryDrawResources(IEnumerable<IResource> resources);
    }
}
