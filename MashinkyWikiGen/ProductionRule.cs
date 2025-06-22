using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    public class ProductionRule
    {
        public int ReqUpgrade { get; private set; }
        public int TimeSteps { get; private set; }
        public RuleNormal RuleN { get; private set; }
        public RuleElectric RuleEl { get; private set; }

        public ProductionRule(int reqUpgrade, int timeSteps, RuleNormal ruleNormal, RuleElectric ruleElectric)
        {
            ReqUpgrade = reqUpgrade;
            TimeSteps = timeSteps;
            RuleN = ruleNormal;
            RuleEl = ruleElectric;
        }
    }
}
