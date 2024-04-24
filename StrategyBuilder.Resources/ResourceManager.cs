using StrategyBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Resources
{
    public class ResourceManager : IResourceManager
    {
        private List<IResource> StoredResources { get; init; }


        public ResourceManager(List<IResource> resources)
        {
            StoredResources = resources;
        }

        public bool IsEnoughResources(List<IResource> upgradeDemands)
        {
            foreach (var demandedResource in upgradeDemands)
            {
                if (demandedResource.Amount == 0)
                    continue;

                //Search for demanded resource in a list of resources
                var pairedResource = StoredResources.First(resource => resource.Name == demandedResource.Name);

                //Checks if the player has enough resources
                if (pairedResource.Amount - demandedResource.Amount < 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Draws the resource from player storage
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public bool TryDrawResources(IEnumerable<IResource> resources)
        {

            List<Tuple<IResource, IResource>> resourcesToDraw = new();
            foreach (var demandedResource in resources)
            {
                if (demandedResource.Amount == 0)
                    continue;

                //Search for demanded resource in a list of resources
                var pairedResource = StoredResources.First(resource => resource.Name == demandedResource.Name);

                //Checks if the player has enough resources
                if (pairedResource.Amount - demandedResource.Amount < 0)
                    return false;

                resourcesToDraw.Add(new Tuple<IResource, IResource>(pairedResource, demandedResource));
            }

            //if player has enough of every needed resource, we can draw them from storage
            foreach (var tuple in resourcesToDraw)
            {
                var storage = tuple.Item1 as ResourceBase ?? throw new InvalidCastException($"Cannot convert {tuple.Item1} to ResourceBase");
                storage.DrawResources(tuple.Item2.Amount);
            }

            return true;
        }

        public void AddResources(IEnumerable<IResource> resources)
        {
            foreach (var storageRes in StoredResources)
            {
                var resToAdd = resources.First(res => res.Name == storageRes.Name);

                var storage = storageRes as ResourceBase ?? throw new InvalidCastException($"Cannot convert {storageRes} to ResourceBase");

                storage.AddResource(resToAdd.Amount);
            }
        }

        /// <summary>
        /// Gets the amount of selected resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public int GetResourceAmount<T>() where T : IResource
        {
            return StoredResources.Find(predicate => predicate.Name == typeof(T).Name)?.Amount ?? throw new NullReferenceException();
        }

        public void PrintAllResources(object? parameter)
        {
            var sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            foreach(var resource in StoredResources)
            {
                sb.Append(resource.Name);
                sb.Append(": ");
                sb.Append(resource.Amount);
            }

            Console.WriteLine(sb.ToString());
        }
    }
}
