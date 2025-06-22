
namespace MashinkyWikiGen
{
    /// <summary>
    /// Base class for building upgrades that can be purchased to enhance existing buildings.
    /// Building upgrades have costs, dimensions, and availability epochs.
    /// </summary>
    public class BuildingUpgrade
    {
        /// <summary>
        /// The amount of the first currency/resource required to purchase this upgrade.
        /// </summary>
        public int Cost1Count { get; private set; }
        
        /// <summary>
        /// The type of the first currency/resource required to purchase this upgrade.
        /// </summary>
        public Token Cost1Type { get; private set; }
        
        /// <summary>
        /// The amount of the second currency/resource required to purchase this upgrade (if any).
        /// </summary>
        public int Cost2Count { get; private set; }
        
        /// <summary>
        /// The type of the second currency/resource required to purchase this upgrade (if any).
        /// </summary>
        public Token Cost2Type { get; private set; }
        
        /// <summary>
        /// The amount of the third currency/resource required to purchase this upgrade (if any).
        /// </summary>
        public int Cost3Count { get; private set; }
        
        /// <summary>
        /// The type of the third currency/resource required to purchase this upgrade (if any).
        /// </summary>
        public Token Cost3Type { get; private set; }
        
        /// <summary>
        /// The display name of the building upgrade.
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The width dimension of the upgrade in tiles.
        /// </summary>
        public int DimX { get; private set; }
        
        /// <summary>
        /// The height dimension of the upgrade in tiles.
        /// </summary>
        public int DimY { get; private set; }
        
        /// <summary>
        /// The epoch when this upgrade becomes available for purchase.
        /// </summary>
        public int Epoch { get; private set; }

        /// <summary>
        /// Initializes a new instance of the BuildingUpgrade class.
        /// </summary>
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
        public BuildingUpgrade(int epoch, string name, int dimX, int dimY, int cost1Count, Token cost1Type, int cost2Count, Token cost2Type, int cost3Count, Token cost3Type)
        {
            Name = name;
            DimX = dimX;
            DimY = dimY;
            Epoch = epoch;
            Cost1Count = cost1Count;
            Cost1Type = cost1Type;
            Cost2Count = cost2Count;
            Cost2Type = cost2Type;
            Cost3Count = cost3Count;
            Cost3Type = cost3Type;
        }
    }
}
