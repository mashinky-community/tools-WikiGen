using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyWikiGen
{
    public class Engine : VehicleBase
    {
        public string SoundWhistle { get; private set; }
        public string SoundStandby { get; private set; }
        public string SoundStart { get; private set; }
        public string SoundSlow { get; private set; }
        public string SoundMedium { get; private set; }
        public string SoundFast { get; private set; }
        public string SoundBrakes { get; private set; }
        public int Power { get; private set;  }
        public int MaxSpeed { get; private set; }
        public int MaxSpeedMiles
        {
            get
            { 
                return Convert.ToInt32(MaxSpeed * 0.621371192); 
            }
        }
        public int FuelAmount1 { get; private set; }
        public Token FuelType1 { get; private set; }
        public int FuelAmount2 { get; private set; }
        public Token FuelType2 { get; private set; }
        public int TrackType { get; private set; }
        public int Pull
        {
            get
            {
                return (int)Math.Round(Power * 0.745699872 / (MaxSpeed / 3.6) / (9.81 * 0.0065)) - Weight;
            }
        }

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
