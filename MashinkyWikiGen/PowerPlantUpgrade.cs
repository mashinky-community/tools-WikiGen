using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    public class PowerPlantUpgrade : IndustryBuildingUpgrade
    {
        public int ElGeneration { get; private set; }
        public string Effect { get; private set; }
        public PowerPlantUpgrade(string effect, int elGeneration, List<ProductionRule> rules, string desc, int epoch, string name, int dimX, int dimY, int cost1C, Token cost1T, int cost2C, Token cost2T, int cost3C, Token cost3T) : base(rules, desc, epoch, name, dimX, dimY, cost1C, cost1T, cost2C, cost2T, cost3C, cost3T)
        {
            ElGeneration = elGeneration;
            Effect = effect;
        }
    }
}
