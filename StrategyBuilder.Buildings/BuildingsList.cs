using StrategyBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
