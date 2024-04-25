using Microsoft.Extensions.DependencyInjection;
using StrategyBuilder.Interfaces;

namespace StrategyBuilder.Buildings.Implementation
{
    public static class DIExtension
    {
        public static IServiceCollection AddBuildings(this IServiceCollection services)
        {
            services.AddSingleton<IBuildingsManager, BuildingsManager>(provider =>
            {
                var buildings = provider.GetServices<IBuilding>();

                var list = buildings.ToList();

                var manager = new BuildingsManager(list);

                return manager;
            });

            services.AddTransient<IBuilding, WoodCutter>();

            return services;
        }
    }
}
