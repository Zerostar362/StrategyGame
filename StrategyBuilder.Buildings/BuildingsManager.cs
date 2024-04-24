using StrategyBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Buildings
{
    public class BuildingsManager : IBuildingsManager
    {
        private List<IBuilding> Buildings { get; init; }
        public BuildingsManager(List<IBuilding> buildings) 
        {
            Buildings = buildings;
        }

        public bool TryUpgradeBuilding<T>()
        {
            var building = Buildings.Find(b => b.Name == typeof(T).Name);

            if (building is null)
                throw new NullReferenceException($"{typeof(T).Name} was not found");

            if (building.TryLevelUp())
                return true;
            return false;
        }

        public void Update()
        {
            foreach (var building in Buildings)
            {
                building.Update();
            }
        }
    }
}
