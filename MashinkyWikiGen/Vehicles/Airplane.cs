using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    public class Airplane : VehicleBase, IUseFuel, IHaveEngine
    {
        public int RunwayLength { get; }
        public int FlightAltitude { get; }
        public int FuelAmount1 { get; }

        public Token FuelType1 { get; }

        public int FuelAmount2 { get; }

        public Token FuelType2 { get; }

        public string SoundWhistle { get; }

        public string SoundStandby { get; }

        public string SoundStart { get; }

        public string SoundSlow { get; }

        public string SoundMedium { get; }

        public string SoundFast { get; }

        public string SoundBrakes { get; }

        public int Power { get; }

        public int MaxSpeed { get; }

        public int MaxSpeedMiles
        {
            get
            {
                return Convert.ToInt32(MaxSpeed * 0.621371192);
            }
        }

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
