using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    // this should be reworked to structure when refactoring Engines to Car and Locomotive
    public interface IHaveEngine
    {
        string SoundWhistle { get; }
        string SoundStandby { get; }
        string SoundStart { get; }
        string SoundSlow { get; }
        string SoundMedium { get; }
        string SoundFast { get; }
        string SoundBrakes { get; }
        int Power { get; }
        int MaxSpeed { get; }
        int MaxSpeedMiles { get; }
    }
}
