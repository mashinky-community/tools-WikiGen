using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Represents an aircraft vehicle that can transport passengers or cargo by air.
    /// Airplanes have unique properties like runway length requirements and flight altitude.
    /// </summary>
    public class Airplane : VehicleBase, IUseFuel, IHaveEngine
    {
        /// <summary>
        /// The minimum runway length required for this airplane to take off and land.
        /// </summary>
        public int RunwayLength { get; }
        
        /// <summary>
        /// The flight altitude for this airplane.
        /// </summary>
        public int FlightAltitude { get; }
        
        /// <summary>
        /// The amount of the first fuel type consumed per operation.
        /// </summary>
        public int FuelAmount1 { get; }

        /// <summary>
        /// The first fuel type consumed by this airplane.
        /// </summary>
        public Token FuelType1 { get; }

        /// <summary>
        /// The amount of the second fuel type consumed per operation (if any).
        /// </summary>
        public int FuelAmount2 { get; }

        /// <summary>
        /// The second fuel type consumed by this airplane (if any).
        /// </summary>
        public Token FuelType2 { get; }

        /// <summary>
        /// The whistle/horn sound file path for this airplane.
        /// </summary>
        public string SoundWhistle { get; }

        /// <summary>
        /// The standby/idle sound file path for this airplane.
        /// </summary>
        public string SoundStandby { get; }

        /// <summary>
        /// The startup sound file path for this airplane.
        /// </summary>
        public string SoundStart { get; }

        /// <summary>
        /// The slow speed sound file path for this airplane.
        /// </summary>
        public string SoundSlow { get; }

        /// <summary>
        /// The medium speed sound file path for this airplane.
        /// </summary>
        public string SoundMedium { get; }

        /// <summary>
        /// The fast speed sound file path for this airplane.
        /// </summary>
        public string SoundFast { get; }

        /// <summary>
        /// The braking sound file path for this airplane.
        /// </summary>
        public string SoundBrakes { get; }

        /// <summary>
        /// The power output of this airplane's engine in horsepower (hp).
        /// </summary>
        public int Power { get; }

        /// <summary>
        /// The maximum speed of this airplane in km/h.
        /// </summary>
        public int MaxSpeed { get; }

        /// <summary>
        /// The maximum speed of this airplane converted to miles per hour (mph).
        /// </summary>
        public int MaxSpeedMiles
        {
            get
            {
                return Convert.ToInt32(MaxSpeed * 0.621371192);
            }
        }

        /// <summary>
        /// Initializes a new instance of the Airplane class.
        /// </summary>
        /// <param name="runwayLength">The minimum runway length required for takeoff and landing</param>
        /// <param name="flightAltitude">The flight altitude for this airplane</param>
        /// <param name="soundWhistle">The whistle/horn sound file path</param>
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
        public Airplane(int runwayLength, int flightAltitude, string soundWhistle, string soundStandby, string soundStart, string soundSlow, string soundMedium, string soundFast, string soundBrakes, int power, int maxSpeed,
            int fuelAmount1, Token fuelType1, int fuelAmount2, Token fuelType2, string name, Token cargo, int capacity, int costAmount1, Token costType1, int costAmount2, Token costType2,
            int weight, double length, bool depotExtension, int startEpoch, int endEpoch, Bitmap icon, int vehicleType)
            : base(name, cargo, capacity, costAmount1, costType1, costAmount2, costType2, weight, length, depotExtension, startEpoch, endEpoch, icon, vehicleType)
        {
            RunwayLength = runwayLength;
            FlightAltitude = flightAltitude;
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
        }
    }
}
