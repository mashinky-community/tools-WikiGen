using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    public class TownExtension : BuildingUpgrade
    {
        public int Relax { get; private set; }
        public int Amenities { get; private set; }
        public int Luxury { get; private set; }
        public TownExtension(int relax, int amenities, int luxury, int epoch, string name, int dimX, int dimY, int cost1C, Token cost1T, int cost2C, Token cost2T, int cost3C, Token cost3T) : base(epoch, name, dimX, dimY, cost1C, cost1T, cost2C, cost2T, cost3C, cost3T)
        {
            Relax = relax;
            Amenities = amenities;
            Luxury = luxury;
        }
    }
}
