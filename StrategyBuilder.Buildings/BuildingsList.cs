using StrategyBuilder.Interfaces;

namespace StrategyBuilder.Buildings
{
    public class WoodCutter : BuildBase<WoodCutter>
    {
        public WoodCutter(IResourceManager manager) :
            base(manager)
        {

        }
    }


}
