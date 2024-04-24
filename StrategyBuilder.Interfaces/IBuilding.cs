using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Interfaces
{
    public interface IBuilding
    {
        string Name { get; }
        string Description { get; }
        int Level { get; }
        int Money_Upgrade_Need { get; }
        List<IResource> Resource_Upgrade_Need { get; }

        bool CanBeUpgraded();
        bool TryLevelUp();
        void Update();
    }
}
