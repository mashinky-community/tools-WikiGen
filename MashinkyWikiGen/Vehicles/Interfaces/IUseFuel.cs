using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    // this should be reworked to structure when refactoring Engines to Car and Locomotive
    public interface IUseFuel
    {
        int FuelAmount1 { get; }
        Token FuelType1 { get; }
        int FuelAmount2 { get; }
        Token FuelType2 { get; }
    }
}
