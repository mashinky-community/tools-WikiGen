using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Defines the contract for vehicles that consume fuel for operation.
    /// Note: This should be reworked to structure when refactoring Engines to Car and Locomotive.
    /// </summary>
    public interface IUseFuel
    {
        /// <summary>
        /// The amount of the first fuel type consumed per operation.
        /// </summary>
        int FuelAmount1 { get; }
        
        /// <summary>
        /// The first fuel type consumed by this vehicle.
        /// </summary>
        Token FuelType1 { get; }
        
        /// <summary>
        /// The amount of the second fuel type consumed per operation (if any).
        /// </summary>
        int FuelAmount2 { get; }
        
        /// <summary>
        /// The second fuel type consumed by this vehicle (if any).
        /// </summary>
        Token FuelType2 { get; }
    }
}
