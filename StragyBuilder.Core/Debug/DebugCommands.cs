using Microsoft.Extensions.DependencyInjection;
using StragyBuilder.Shared.Input;
using StrategyBuilder.Interfaces;
using System.Security.Cryptography;

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
            string[]? arguments = null;

            if (obj is not null)
                arguments = (string[])obj ?? throw new InvalidCastException("Cannot cast arguments to string array");

            List<string> filterList = new List<string>();

            if (arguments is not null)
                if (arguments.Contains("-f") || arguments.Contains("--filter"))
                {
                    int index = int.MaxValue;
                    if (arguments.Contains("-f"))
                        index = Array.IndexOf(arguments, "-f");

                    if (arguments.Contains("--filter"))
                        index = Array.IndexOf(arguments, "--filter");

                    //if (index == 0)
                    //throw new ArgumentException("Application argument parser failed to fetch parameters");


                    if (index != int.MaxValue)
                    {
                        for (int i = index + 1; i < arguments.Length; i++)
                        {
                            if (arguments[i].Contains("-"))
                                break;
                            filterList.Add(arguments[i]);
                        }
                    }

                }

            foreach (var resource in resources)
            {
                if (filterList.Count > 0)
                {
                    if (filterList.Contains(resource.Name.ToLower()))
                        continue;
                }

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
                return new Command("debug resources show", "Shows resources that are available in the game", dbg.PrintAllResources, dbg.InstantCanExecute);
            });

            return services;
        }
    }
}
