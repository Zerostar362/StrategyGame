using Microsoft.Extensions.DependencyInjection;
using StragyBuilder.Shared.Input;
using StrategyBuilder.Interfaces;
using StrategyBuilder.Shared.HelperClasses;

namespace StrategyBuilder.Resources.Implementation
{
    public static class DIExtension
    {
        public static IServiceCollection AddResources(this IServiceCollection services)
        {
            services.AddSingleton<IResourceManager, ResourceManager>(provider =>
            {
                var resources = provider.GetServices<IResource>();

                var list = resources.ToList();


                return new ResourceManager(list);
            });

            services.AddTransient<IResource, Wood>();
            services.AddTransient<IResource, Iron>();
            services.AddTransient<IResource, Oat>();
            services.AddTransient<IResource, Gold>();
            services.AddTransient<IResource, People>();

            services.AddTransient<ICommand, Command>(provider =>
            {
                var manager = provider.GetService<IResourceManager>();


                var cmd = new Command("resources show", "Shows amount of all resources", manager.PrintAllResources, CommandHelper.CanExecuteTrue);
                return cmd;
            });

            /*services.AddTransient<ICommand, Command>(provider =>
            {
                var manager = provider.GetService<IResourceManager>();

                var cmd = new Command("resources show")
            })*/



            return services;
        }
    }
}
