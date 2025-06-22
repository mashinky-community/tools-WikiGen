using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MashinkyWikiGen
{
    public class IndustryBuilding : Building
    {

        public List<IndustryBuildingUpgrade> Upgrades { get; private set; }
        public List<ProductionRule> Rules { get; private set; }
        public Bitmap Icon { get; private set; }
        public int BonusDistance { get; private set; }
        public List<int> Epochs { get; private set; } = new List<int>();
        public int PollutionRange { get; private set; }


        public IndustryBuilding(List<IndustryBuildingUpgrade> upgrades, List<ProductionRule> rules, Bitmap icon, int bonusDistance, int pollutionRange, string iD, string name, int dimX, int dimY, int[] countPerEpoch) : base(iD, name, dimX, dimY, countPerEpoch)
        {
            Upgrades = upgrades;
            Rules = rules;
            Icon = icon;
            BonusDistance = bonusDistance;
            TranslateEpochs();
            PollutionRange = pollutionRange;
        }

        private void TranslateEpochs()
        {
            for (int i = 0; i < CountPerEpoch.Length; i++)
            {
                if (CountPerEpoch[i] > 0)
                {
                    Epochs.Add(i + 1);
                }
            }
        }


    }
}
