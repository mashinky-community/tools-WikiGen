using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MashinkyWikiGen
{
    public class Building
    {

        public string Id { get; private set; }
        public string Name { get; private set; }
        public int DimX { get; private set; }
        public int DimY { get; private set; }
        public int[] CountPerEpoch { get; private set; }


        public Building(string id, string name, int dimX, int dimY, int[] countPerEpoch)
        {
            Id = id;
            Name = name;
            DimX = dimX;
            DimY = dimY;
            CountPerEpoch = countPerEpoch;
        }
    }
}
