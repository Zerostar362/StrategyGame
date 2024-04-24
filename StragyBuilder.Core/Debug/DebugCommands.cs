using Microsoft.Extensions.DependencyInjection;
using StragyBuilder.Shared.Input;
using StrategyBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StragyBuilder.Core.Debug
{
    public class DebugCommands
    {
        private IServiceProvider ServiceProvider { get; init; }
        public DebugCommands(IServiceProvider provider) 
        {
            ServiceProvider = provider;
        }

        public bool InstantCanExecute(object? obj) => true;
        public void PrintAllResources(object? obj)
        {
            var resources = ServiceProvider.GetServices<IResource>();

            foreach (var resource in resources)
            {
                Console.WriteLine(resource.Name);
            }
        }
    }

    public static class DIExtension 
    {
        public static IServiceCollection AddDebugCommands(this IServiceCollection services)
        {
            services.AddSingleton<ICommand, Command>(impl =>
            {
                var dbg = new DebugCommands(impl);
                return new Command("debug resources show","Shows resources that are available in the game", dbg.PrintAllResources, dbg.InstantCanExecute);
            });

            return services;
        }
    }
}
