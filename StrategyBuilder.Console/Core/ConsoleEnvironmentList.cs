using Microsoft.Extensions.DependencyInjection;
using StrategyBuilder.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.ConsoleController.Core
{
    public class ConsoleEnvironmentList : List<ConsoleEnvironment>
    {
        //private IServiceProvider serviceProvider;
        public ConsoleEnvironmentList(IServiceProvider provider) 
        {
            Init(provider);
        }

        public void Init(IServiceProvider serviceProvider)
        {
            //first we filter all the commands by environment
            var wrappers = serviceProvider.GetServices<CommandWrapper>();
            var dictionary = new Dictionary<string[], List<CommandWrapper>>();
            foreach (var w in wrappers)
            {
                if (!dictionary.ContainsKey(w.Environment))
                {
                    dictionary.Add(w.Environment, new List<CommandWrapper>() { w });
                }

                dictionary[w.Environment].Add(w);
            }

            //with filtered commands, we can safely add them to each one of the environments

            foreach (var item in dictionary)
            {
                this.Add(new ConsoleEnvironment(item.Key, item.Value));
            }
        }

        public ConsoleEnvironment? Find(string[] arr)
        {
            foreach (var env in this) 
            {
                for (var i = 0; i < arr.Length; i++)
                {
                    if (env.CurrentEnvironment.Length <= i)
                        break;

                    if (arr[i] != env.CurrentEnvironment[i])
                        break;

                    if(i + 1 == arr.Length)
                        return env;
                }
            }

            return null;
        }

        /*public void AddEnvironment(string[] environment)
        {
            var keyedService = serviceProvider.GetKeyedServices<CommandWrapper>(environment) ?? throw new NullReferenceException($"Keyed service {environment} was not found");
            var ConsoleEnvironment = new ConsoleEnvironment(environment, keyedService);
        }*/
    }
}
