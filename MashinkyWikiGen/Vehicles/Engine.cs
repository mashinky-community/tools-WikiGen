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
    /// Represents a locomotive or road vehicle that can pull other vehicles and has its own power source.
    /// Engines have additional properties like power, fuel consumption, and track type requirements.
    /// </summary>
    public class Engine : VehicleBase
    {
        /// <summary>
        /// The whistle sound file path for this engine.
        /// </summary>
        public string SoundWhistle { get; private set; }
        
        /// <summary>
        /// The standby/idle sound file path for this engine.
        /// </summary>
        public string SoundStandby { get; private set; }
        
        /// <summary>
        /// The startup sound file path for this engine.
        /// </summary>
        public string SoundStart { get; private set; }
        
        /// <summary>
        /// The slow speed sound file path for this engine.
        /// </summary>
        public string SoundSlow { get; private set; }
        
        /// <summary>
        /// The medium speed sound file path for this engine.
        /// </summary>
        public string SoundMedium { get; private set; }
        
        /// <summary>
        /// The fast speed sound file path for this engine.
        /// </summary>
        public string SoundFast { get; private set; }
        
        /// <summary>
        /// The braking sound file path for this engine.
        /// </summary>
        public string SoundBrakes { get; private set; }
        
        /// <summary>
        /// The power output of this engine in horsepower (hp).
        /// </summary>
        public int Power { get; private set;  }
        
        /// <summary>
        /// The maximum speed of this engine in km/h.
        /// </summary>
        public int MaxSpeed { get; private set; }
        
        /// <summary>
        /// The maximum speed of this engine converted to miles per hour (mph).
        /// </summary>
        public int MaxSpeedMiles
        {
            get
            { 
                return Convert.ToInt32(MaxSpeed * 0.621371192); 
            }
        }
        
        /// <summary>
        /// The amount of the first fuel type consumed per operation.
        /// </summary>
        public int FuelAmount1 { get; private set; }
        
        /// <summary>
        /// The first fuel type consumed by this engine.
        /// </summary>
        public Token FuelType1 { get; private set; }
        
        /// <summary>
        /// The amount of the second fuel type consumed per operation (if any).
        /// </summary>
        public int FuelAmount2 { get; private set; }
        
        /// <summary>
        /// The second fuel type consumed by this engine (if any).
        /// </summary>
        public Token FuelType2 { get; private set; }
        
        /// <summary>
        /// The track type required for this engine.
        /// 0 = normal track, 1 = fast track, 2 = electric track.
        /// </summary>
        public int TrackType { get; private set; }
        
        /// <summary>
        /// The pulling capacity of this engine in tons.
        /// Calculated based on power, max speed, and engine weight using physics formulas.
        /// </summary>
        public int Pull
        {
            get
            {
                return (int)Math.Round(Power * 0.745699872 / (MaxSpeed / 3.6) / (9.81 * 0.0065)) - Weight;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Engine class.
        /// </summary>
        /// <param name="trackType">The track type required (0 = normal, 1 = fast, 2 = electric)</param>
        /// <param name="soundWhistle">The whistle sound file path</param>
        /// <param name="soundStandby">The standby/idle sound file path</param>
        /// <param name="soundStart">The startup sound file path</param>
        /// <param name="soundSlow">The slow speed sound file path</param>
        /// <param name="soundMedium">The medium speed sound file path</param>
        /// <param name="soundFast">The fast speed sound file path</param>
        /// <param name="soundBrakes">The braking sound file path</param>
        /// <param name="power">The power output in horsepower (hp)</param>
        /// <param name="maxSpeed">The maximum speed in km/h</param>
        /// <param name="fuelAmount1">The amount of the first fuel type consumed</param>
        /// <param name="fuelType1">The first fuel type consumed</param>
        /// <param name="fuelAmount2">The amount of the second fuel type consumed (if any)</param>
        /// <param name="fuelType2">The second fuel type consumed (if any)</param>
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
        /// <param name="vehicleType">The vehicle type identifier</param>
        public Engine(int trackType, string soundWhistle, string soundStandby, string soundStart, string soundSlow, string soundMedium, string soundFast, string soundBrakes, int power, int maxSpeed,
            int fuelAmount1, Token fuelType1, int fuelAmount2, Token fuelType2, string name, Token cargo, int capacity, int costAmount1, Token costType1, int costAmount2, Token costType2,
            int weight, double length, bool depotExtension, int startEpoch, int endEpoch, Bitmap icon, int vehicleType)
            : base(name, cargo, capacity, costAmount1, costType1, costAmount2, costType2, weight, length, depotExtension, startEpoch, endEpoch, icon, vehicleType)
        {
            SoundWhistle = soundWhistle;
            SoundStandby = soundStandby;
            SoundStart = soundStart;
            SoundSlow = soundSlow;
            SoundMedium = soundMedium;
            SoundFast = soundFast;
            SoundBrakes = soundBrakes;
            Power = power;
            MaxSpeed = maxSpeed;
            FuelAmount1 = fuelAmount1;
            FuelType1 = fuelType1;
            FuelAmount2 = fuelAmount2;
            FuelType2 = fuelType2;
            TrackType = trackType;
        }
    }
}
