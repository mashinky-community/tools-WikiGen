using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    public class TownDecoration : BuildingUpgrade
    {

        public TownDecoration(int epoch, string name, int dimX, int dimY, int cost1C, Token cost1T, int cost2C, Token cost2T, int cost3C, Token cost3T) : base(epoch, name, dimX, dimY, cost1C, cost1T, cost2C, cost2T, cost3C, cost3T)
        {
            
        }
    }
}
