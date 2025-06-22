using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MashinkyWikiGen
{
    public class PowerPlant : IndustryBuilding
    {
        public int ElGenerated { get; private set; }
        public PowerPlant(int elGenerated, List<IndustryBuildingUpgrade> upgrades, List<ProductionRule> rules, Bitmap mainOutput, int bonusDistance, int pollutionRange, string iD, string name, int dimX, int dimY, int[] countPerEpoch) : base(upgrades, rules, mainOutput, bonusDistance, pollutionRange, iD, name, dimX, dimY, countPerEpoch)
        {
            ElGenerated = elGenerated;
        }

        

    }
}
