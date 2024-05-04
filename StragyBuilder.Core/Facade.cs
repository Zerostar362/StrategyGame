using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StrategyBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StragyBuilder.Core
{
    public class Facade : IFacade
    {
        private IServiceProvider ServiceProvider { get; init; }
        private ILogger<Facade> Logger { get; init; }


        public Facade(IServiceProvider provider, ILogger<Facade> logger)
        {
            ServiceProvider = provider;
            Logger = logger;
        }

        public int GetResourceAmount<T>() where T : IResource
        {
            using (Logger.BeginScope("Resource manager scope"))
            {
                try
                {
                    var manager = ServiceProvider.GetService<IResourceManager>() ?? throw new NullReferenceException();
                    return manager.GetResourceAmount<T>();

                }
                catch(NullReferenceException ex)
                {
                    Logger.LogError($"Resource manager was null. {0} || {Environment.NewLine} StackTrace: {1}", ex.Message, ex.StackTrace);
                    return 0;
                }
            }
        }

        public Dictionary<string,int>? GetResourceAmount_All()
        {
            using (Logger.BeginScope("Resource manager scope"))
            {
                throw new NotImplementedException();
                try
                {
                    var manager = ServiceProvider.GetService<IResourceManager>() ?? throw new NullReferenceException();
                    //return manager.GetResourceAmount_All();

                }
                catch (NullReferenceException ex)
                {
                    Logger.LogError($"Resource manager was null. {0} || {Environment.NewLine} StackTrace: {1}", ex.Message, ex.StackTrace);
                    return null;
                }
            }
        }
    }
}
