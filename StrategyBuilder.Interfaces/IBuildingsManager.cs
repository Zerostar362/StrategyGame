namespace StrategyBuilder.Interfaces
{
    public interface IBuildingsManager
    {
        bool TryUpgradeBuilding<T>();
        void Update();
    }
}
