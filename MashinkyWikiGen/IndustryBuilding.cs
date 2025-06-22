using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Represents an industry building that produces goods by transforming input resources into output resources.
    /// Industry buildings can have multiple production rules, upgrades, and may provide distance bonuses.
    /// </summary>
    public class IndustryBuilding : Building
    {
        /// <summary>
        /// The list of available upgrades for this industry building.
        /// </summary>
        public List<IndustryBuildingUpgrade> Upgrades { get; private set; }
        
        /// <summary>
        /// The list of production rules that define what this building can produce.
        /// </summary>
        public List<ProductionRule> Rules { get; private set; }
        
        /// <summary>
        /// The icon image for this building.
        /// </summary>
        public Bitmap Icon { get; private set; }
        
        /// <summary>
        /// The distance in tiles within which bonus resources can be produced when near specific resource types.
        /// </summary>
        public int BonusDistance { get; private set; }
        
        /// <summary>
        /// The list of epochs when this building is available.
        /// </summary>
        public List<int> Epochs { get; private set; } = new List<int>();
        
        /// <summary>
        /// The pollution range of this building in tiles (0 if no pollution).
        /// </summary>
        public int PollutionRange { get; private set; }

        /// <summary>
        /// Initializes a new instance of the IndustryBuilding class.
        /// </summary>
        /// <param name="upgrades">The list of available upgrades for this building</param>
        /// <param name="rules">The list of production rules that define what this building can produce</param>
        /// <param name="icon">The icon image for this building</param>
        /// <param name="bonusDistance">The distance in tiles for bonus resource production</param>
        /// <param name="pollutionRange">The pollution range in tiles (0 if no pollution)</param>
        /// <param name="iD">The unique identifier for this building from the game data</param>
        /// <param name="name">The display name of the building</param>
        /// <param name="dimX">The width dimension of the building in tiles</param>
        /// <param name="dimY">The height dimension of the building in tiles</param>
        /// <param name="countPerEpoch">The number of buildings spawned at the start of each epoch</param>
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
