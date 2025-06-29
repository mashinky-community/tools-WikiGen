using System.Collections.Generic;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Represents an upgrade for industry buildings that can enhance production capabilities.
    /// Industry building upgrades can add new production rules and provide additional functionality.
    /// </summary>
    public class IndustryBuildingUpgrade : BuildingUpgrade
    {        
        /// <summary>
        /// The list of production rules that this upgrade adds to the base building.
        /// </summary>
        public List<ProductionRule> Rules { get; private set; }
        
        /// <summary>
        /// The description text explaining what this upgrade does.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Initializes a new instance of the IndustryBuildingUpgrade class.
        /// </summary>
        /// <param name="rules">The list of production rules that this upgrade adds to the base building</param>
        /// <param name="description">The description text explaining what this upgrade does</param>
        /// <param name="epoch">The epoch when this upgrade becomes available for purchase</param>
        /// <param name="name">The display name of the building upgrade</param>
        /// <param name="dimX">The width dimension of the upgrade in tiles</param>
        /// <param name="dimY">The height dimension of the upgrade in tiles</param>
        /// <param name="cost1Count">The amount of the first currency/resource required to purchase</param>
        /// <param name="cost1Type">The type of the first currency/resource required to purchase</param>
        /// <param name="cost2Count">The amount of the second currency/resource required to purchase (if any)</param>
        /// <param name="cost2Type">The type of the second currency/resource required to purchase (if any)</param>
        /// <param name="cost3Count">The amount of the third currency/resource required to purchase (if any)</param>
        /// <param name="cost3Type">The type of the third currency/resource required to purchase (if any)</param>
        public IndustryBuildingUpgrade(List<ProductionRule> rules, string description, int epoch, string name, int dimX, int dimY, int cost1Count, Token cost1Type, int cost2Count, Token cost2Type, int cost3Count, Token cost3Type) : base(epoch, name, dimX, dimY, cost1Count, cost1Type, cost2Count, cost2Type, cost3Count, cost3Type)
        {
            Rules = rules;
            Description = description;
        }

    }
}
