using StrategyBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Buildings
{
    public abstract class BuildBase<T> : BuildingBase
    {
        public BuildBase(IResourceManager manager)
            : base(manager)
        {
            Name = typeof(T).Name;
        }
    }

    public abstract class BuildingBase : IBuilding
    {
        public string Name { get; protected set; } = string.Empty;
        public string Description { get; protected set; } = string.Empty;
        public int Level { get; protected set; } = 0;
        public int Money_Upgrade_Need { get; protected set; } = 0;

        private List<IResource> _resources_upgrade_need = new();
        public List<IResource> Resource_Upgrade_Need
        {
            get
            {
                return new List<IResource>(_resources_upgrade_need);
            }
        }


        private IResourceManager _resourceMgr;

        public BuildingBase(IResourceManager manager)
        {
            _resourceMgr = manager;
        }

        public virtual bool CanBeUpgraded() =>
            _resourceMgr.IsEnoughResources(_resources_upgrade_need);

        public virtual bool TryLevelUp()
        {
            if (CanBeUpgraded())
                if (_resourceMgr.TryDrawResources(_resources_upgrade_need))
                    return true;

            return false;
        }
        public virtual void Update()
        {
            throw new NotImplementedException();
        }
    }
}
