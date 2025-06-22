
namespace MashinkyWikiGen
{
    /// <summary>
    /// Represents a town decoration building that provides visual enhancement to cities.
    /// Town decorations are purely aesthetic buildings with no functional gameplay impact.
    /// </summary>
    public class TownDecoration : BuildingUpgrade
    {
        /// <summary>
        /// Initializes a new instance of the TownDecoration class.
        /// </summary>
        /// <param name="epoch">The epoch when this decoration becomes available for purchase</param>
        /// <param name="name">The display name of the building decoration</param>
        /// <param name="dimX">The width dimension of the decoration in tiles</param>
        /// <param name="dimY">The height dimension of the decoration in tiles</param>
        /// <param name="cost1Count">The amount of the first currency/resource required to purchase</param>
        /// <param name="cost1Type">The type of the first currency/resource required to purchase</param>
        /// <param name="cost2Count">The amount of the second currency/resource required to purchase (if any)</param>
        /// <param name="cost2Type">The type of the second currency/resource required to purchase (if any)</param>
        /// <param name="cost3Count">The amount of the third currency/resource required to purchase (if any)</param>
        /// <param name="cost3Type">The type of the third currency/resource required to purchase (if any)</param>
        public TownDecoration(int epoch, string name, int dimX, int dimY, int cost1Count, Token cost1Type, int cost2Count, Token cost2Type, int cost3Count, Token cost3Type) : base(epoch, name, dimX, dimY, cost1Count, cost1Type, cost2Count, cost2Type, cost3Count, cost3Type)
        {
            
        }
    }
}
