using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StragyBuilder.Shared.Input;
using StrategyBuilder.DTO.LocalResponse.Extensions;
using StrategyBuilder.DTO.Resources;
using StrategyBuilder.Interfaces;
using StrategyBuilder.Shared.Builder;
using StrategyBuilder.Shared.HelperClasses;
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
                var facade = provider.GetService<IFacade>();
                var printer = provider.GetService<IConsolePrinter>();

                var cmdBuilder = new CommandWrapperBuilder(provider.GetService<ILoggerFactory>());
                cmdBuilder.AddOption(new CommandParameterOption(new string[] { "-f", "--filter" }));
                cmdBuilder.SetCommandName("show");
                cmdBuilder.SetEnvironment(new[] { "resource" });
                cmdBuilder.SetCommand(
                    (object? obj) => 
                    { 
                        var response = facade.GetResourceAmount_All();
                        
                        if (!response.TryGetValue<ResourcesAmountDTO>(out var resources))
                            return;

                        var dictionary = new Dictionary<string, string>();
                        resources.Amounts.ForEach(r => dictionary.Add(r.Name, r.Amount.ToString()));

                        printer.RequestPrint();
                        printer.PrintDictionary(dictionary);
                        printer.PrintEnded();

                    });

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
