using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyWikiGen
{
    public class VehicleBase
    {
        public string Name { get; private set; }
        public Token Cargo { get; private set; }
        public int Capacity { get; private set; }
        public int CostAmount1 { get; private set; }
        public Token CostType1 { get; private set; }
        public int CostAmount2 { get; private set; }
        public Token CostType2 { get; private set; }
        public int Weight { get; private set; }
        public double Length { get; private set; }
        public bool ReqDepotExtension { get; private set; }
        public int StartEpoch { get; private set; }
        public int EndEpoch { get; private set; }
        public string Bonus { get; private set; }
        public Bitmap Icon { get; private set; }
        public int VehicleType { get; private set; }



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
