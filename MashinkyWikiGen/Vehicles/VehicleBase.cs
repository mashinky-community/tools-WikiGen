using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Base class for all vehicles in Mashinky including wagons, engines, and aircraft.
    /// Contains common properties like cargo capacity, cost, weight, and availability epochs.
    /// </summary>
    public class VehicleBase
    {
        /// <summary>
        /// The display name of the vehicle.
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The type of cargo this vehicle can transport.
        /// </summary>
        public Token Cargo { get; private set; }
        
        /// <summary>
        /// The cargo capacity of the vehicle.
        /// </summary>
        public int Capacity { get; private set; }
        
        /// <summary>
        /// The amount of the first currency/resource required to purchase this vehicle.
        /// </summary>
        public int CostAmount1 { get; private set; }
        
        /// <summary>
        /// The type of the first currency/resource required to purchase this vehicle.
        /// </summary>
        public Token CostType1 { get; private set; }
        
        /// <summary>
        /// The amount of the second currency/resource required to purchase this vehicle (if any).
        /// </summary>
        public int CostAmount2 { get; private set; }
        
        /// <summary>
        /// The type of the second currency/resource required to purchase this vehicle (if any).
        /// </summary>
        public Token CostType2 { get; private set; }
        
        /// <summary>
        /// The weight of the vehicle in tons.
        /// </summary>
        public int Weight { get; private set; }
        
        /// <summary>
        /// The length of the vehicle.
        /// </summary>
        public double Length { get; private set; }
        
        /// <summary>
        /// Whether this vehicle requires a depot extension to be purchased.
        /// </summary>
        public bool ReqDepotExtension { get; private set; }
        
        /// <summary>
        /// The epoch when this vehicle becomes available for purchase (0-7).
        /// </summary>
        public int StartEpoch { get; private set; }
        
        /// <summary>
        /// The epoch when this vehicle is no longer available for purchase.
        /// </summary>
        public int EndEpoch { get; private set; }
        
        /// <summary>
        /// Any special bonus or capability description for this vehicle.
        /// Note: This property appears to be unused in the current implementation.
        /// </summary>
        public string Bonus { get; private set; }
        
        /// <summary>
        /// The icon image for this vehicle.
        /// </summary>
        public Bitmap Icon { get; private set; }
        
        /// <summary>
        /// The vehicle type identifier (0 = engine, 1 = road vehicle, etc.).
        /// </summary>
        public int VehicleType { get; private set; }



        /// <summary>
        /// Initializes a new instance of the VehicleBase class.
        /// </summary>
        /// <param name="name">The display name of the vehicle</param>
        /// <param name="cargo">The type of cargo this vehicle can transport</param>
        /// <param name="capacity">The cargo capacity of the vehicle</param>
        /// <param name="costAmount1">The amount of the first currency/resource required to purchase</param>
        /// <param name="costType1">The type of the first currency/resource required to purchase</param>
        /// <param name="costAmount2">The amount of the second currency/resource required to purchase (if any)</param>
        /// <param name="costType2">The type of the second currency/resource required to purchase (if any)</param>
        /// <param name="weight">The weight of the vehicle in tons</param>
        /// <param name="length">The length of the vehicle</param>
        /// <param name="depotExtension">Whether this vehicle requires a depot extension to be purchased</param>
        /// <param name="startEpoch">The epoch when this vehicle becomes available for purchase (0-7)</param>
        /// <param name="endEpoch">The epoch when this vehicle is no longer available for purchase</param>
        /// <param name="icon">The icon image for this vehicle</param>
        /// <param name="vehicleType">The vehicle type identifier (0 = engine, 1 = road vehicle, etc.)</param>
        public VehicleBase(string name, Token cargo, int capacity, int costAmount1, Token costType1, int costAmount2, Token costType2, int weight, double length, bool depotExtension, int startEpoch, int endEpoch, Bitmap icon, int vehicleType)
        {
            Name = name;
            Capacity = capacity;
            Cargo = cargo;
            CostAmount1 = costAmount1;
            CostType1 = costType1;
            CostAmount2 = costAmount2;
            CostType2 = costType2;
            Weight = weight;
            Length = length;
            ReqDepotExtension = depotExtension;
            StartEpoch = startEpoch;
            EndEpoch = endEpoch;
            Icon = icon;
            VehicleType = vehicleType;

        }
    }
}
