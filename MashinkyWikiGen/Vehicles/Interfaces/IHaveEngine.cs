using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Defines the contract for vehicles that have their own engine and power source.
    /// Note: This should be reworked to structure when refactoring Engines to Car and Locomotive.
    /// </summary>
    public interface IHaveEngine
    {
        /// <summary>
        /// The whistle/horn sound file path for this vehicle.
        /// </summary>
        string SoundWhistle { get; }
        
        /// <summary>
        /// The standby/idle sound file path for this vehicle.
        /// </summary>
        string SoundStandby { get; }
        
        /// <summary>
        /// The startup sound file path for this vehicle.
        /// </summary>
        string SoundStart { get; }
        
        /// <summary>
        /// The slow speed sound file path for this vehicle.
        /// </summary>
        string SoundSlow { get; }
        
        /// <summary>
        /// The medium speed sound file path for this vehicle.
        /// </summary>
        string SoundMedium { get; }
        
        /// <summary>
        /// The fast speed sound file path for this vehicle.
        /// </summary>
        string SoundFast { get; }
        
        /// <summary>
        /// The braking sound file path for this vehicle.
        /// </summary>
        string SoundBrakes { get; }
        
        /// <summary>
        /// The power output of this vehicle's engine in horsepower (hp).
        /// </summary>
        int Power { get; }
        
        /// <summary>
        /// The maximum speed of this vehicle in km/h.
        /// </summary>
        int MaxSpeed { get; }
        
        /// <summary>
        /// The maximum speed of this vehicle converted to miles per hour (mph).
        /// </summary>
        int MaxSpeedMiles { get; }
    }
}
