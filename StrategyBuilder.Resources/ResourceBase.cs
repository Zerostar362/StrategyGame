using StrategyBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Resources
{
    public abstract class ResourceBase<T> : ResourceBase
    {
        public ResourceBase()
            : base()
        {
            Name = typeof(T).Name;
        }
    }

    public abstract class ResourceBase : IResource
    {
        public string Name { get; protected set; } = string.Empty;
        public int Amount { get; protected set; } = 0;

        public void SetDefaultAmount()
        {
            Amount = 50;
        }

        public void DrawResources(int amount) => Amount -= amount;

        public void AddResource(int amount) => Amount += amount;
    }
}
