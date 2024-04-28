using Microsoft.Extensions.DependencyInjection;
using StragyBuilder.Shared.Input;
using StrategyBuilder.Interfaces;
using StrategyBuilder.Shared.Builder;
using StrategyBuilder.Shared.HelperClasses;
using StrategyBuilder.Shared.Input;
using StrategyBuilder.Shared.Wrapper;

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

            services.AddTransient<CommandWrapper>(provider =>
            {
                var manager = provider.GetService<IResourceManager>();

                var cmdBuilder = new CommandWrapperBuilder();
                cmdBuilder.AddOption(new CommandParameterOption(new string[] { "-f", "--filter" }));
                cmdBuilder.SetCommandName("show");
                cmdBuilder.SetEnvironment(new[] { "resources" });
                cmdBuilder.SetCommand(manager.PrintAllResources);

                var wrapper = cmdBuilder.Build();

                //var cmd = new Command("resources show", "Shows amount of all resources", manager.PrintAllResources, CommandHelper.CanExecuteTrue);
                return wrapper;
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
