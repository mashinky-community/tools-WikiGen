using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Represents a complete production rule for a building that combines both normal and electric operation modes.
    /// Contains timing information and references to both normal and electric rule variants.
    /// </summary>
    public class ProductionRule
    {
        /// <summary>
        /// The upgrade level required for this production rule to be available.
        /// </summary>
        public int ReqUpgrade { get; private set; }
        
        /// <summary>
        /// The number of game ticks required to complete this production cycle.
        /// </summary>
        public int TimeSteps { get; private set; }
        
        /// <summary>
        /// The normal (non-electric) variant of this production rule.
        /// </summary>
        public RuleNormal RuleN { get; private set; }
        
        /// <summary>
        /// The electric variant of this production rule.
        /// </summary>
        public RuleElectric RuleEl { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ProductionRule class.
        /// </summary>
        /// <param name="reqUpgrade">The upgrade level required for this production rule to be available</param>
        /// <param name="timeSteps">The number of game ticks required to complete this production cycle</param>
        /// <param name="ruleNormal">The normal (non-electric) variant of this production rule</param>
        /// <param name="ruleElectric">The electric variant of this production rule</param>
        public ProductionRule(int reqUpgrade, int timeSteps, RuleNormal ruleNormal, RuleElectric ruleElectric)
        {
            ReqUpgrade = reqUpgrade;
            TimeSteps = timeSteps;
            RuleN = ruleNormal;
            RuleEl = ruleElectric;
        }
    }
}
