using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    public class BuildingUpgrade
    {
        public int Cost1C { get; private set; }
        public Token Cost1T { get; private set; }
        public int Cost2C { get; private set; }
        public Token Cost2T { get; private set; }
        public int Cost3C { get; private set; }
        public Token Cost3T { get; private set; }
        public string Name { get; private set; }
        public int DimX { get; private set; }
        public int DimY { get; private set; }
        public int Epoch { get; private set; }

        public BuildingUpgrade(int epoch, string name, int dimX, int dimY, int cost1C, Token cost1T, int cost2C, Token cost2T, int cost3C, Token cost3T)
        {
            Name = name;
            DimX = dimX;
            DimY = dimY;
            Epoch = epoch;
            Cost1C = cost1C;
            Cost1T = cost1T;
            Cost2C = cost2C;
            Cost2T = cost2T;
            Cost3C = cost3C;
            Cost3T = cost3T;
        }
    }
}
