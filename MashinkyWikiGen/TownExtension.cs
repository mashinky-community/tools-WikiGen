using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Represents a town extension building that provides happiness bonuses to nearby citizens.
    /// Town extensions can provide relax, amenities, or luxury bonuses within their catchment area.
    /// </summary>
    public class TownExtension : BuildingUpgrade
    {
        /// <summary>
        /// The relax bonus provided by this town extension within its catchment area.
        /// </summary>
        public int Relax { get; private set; }
        
        /// <summary>
        /// The amenities bonus provided by this town extension within its catchment area.
        /// </summary>
        public int Amenities { get; private set; }
        
        /// <summary>
        /// The luxury bonus provided by this town extension within its catchment area.
        /// </summary>
        public int Luxury { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the TownExtension class.
        /// </summary>
        /// <param name="relax">The relax bonus provided within the catchment area</param>
        /// <param name="amenities">The amenities bonus provided within the catchment area</param>
        /// <param name="luxury">The luxury bonus provided within the catchment area</param>
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
        public TownExtension(int relax, int amenities, int luxury, int epoch, string name, int dimX, int dimY, int cost1Count, Token cost1Type, int cost2Count, Token cost2Type, int cost3Count, Token cost3Type) : base(epoch, name, dimX, dimY, cost1Count, cost1Type, cost2Count, cost2Type, cost3Count, cost3Type)
        {
            Relax = relax;
            Amenities = amenities;
            Luxury = luxury;
        }
    }
}
