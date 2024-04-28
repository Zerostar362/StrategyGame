namespace StrategyBuilder.Interfaces
{
    public interface IResourceManager
    {
        void AddResources(IEnumerable<IResource> resources);
        int GetResourceAmount<T>() where T : IResource;
        Dictionary<string, int> GetResourceAmount_All();
        bool IsEnoughResources(List<IResource> upgradeDemands);
        void PrintAllResources(object? parameters);
        bool TryDrawResources(IEnumerable<IResource> resources);
    }
}
