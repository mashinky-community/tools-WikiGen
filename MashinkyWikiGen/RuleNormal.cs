using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Represents a normal (non-electric) production rule for buildings that defines input/output resource requirements.
    /// </summary>
    public class RuleNormal : IRule
    {        
        /// <summary>
        /// The first input resource type required for this production rule.
        /// </summary>
        public Token Input1Type { get; private set; }
        
        /// <summary>
        /// The quantity of the first input resource required.
        /// </summary>
        public int Input1Count { get; private set; }
        
        /// <summary>
        /// The second input resource type required for this production rule.
        /// </summary>
        public Token Input2Type { get; private set; }
        
        /// <summary>
        /// The quantity of the second input resource required.
        /// </summary>
        public int Input2Count { get; private set; }
        
        /// <summary>
        /// The third input resource type required for this production rule.
        /// </summary>
        public Token Input3Type { get; private set; }
        
        /// <summary>
        /// The quantity of the third input resource required.
        /// </summary>
        public int Input3Count { get; private set; }
        
        /// <summary>
        /// The first output resource type produced by this production rule.
        /// </summary>
        public Token Output1Type { get; private set; }
        
        /// <summary>
        /// The quantity of the first output resource produced.
        /// </summary>
        public int Output1Count { get; private set; }
        
        /// <summary>
        /// The second output resource type produced by this production rule.
        /// </summary>
        public Token Output2Type { get; private set; }
        
        /// <summary>
        /// The quantity of the second output resource produced.
        /// </summary>
        public int Output2Count { get; private set; }
        
        /// <summary>
        /// The third output resource type produced by this production rule.
        /// </summary>
        public Token Output3Type { get; private set; }
        
        /// <summary>
        /// The quantity of the third output resource produced.
        /// </summary>
        public int Output3Count { get; private set; }
        
        /// <summary>
        /// The quantity of bonus output produced when building is near specific resources.
        /// </summary>
        public int OutputDistBonusCount { get; private set; }
        
        /// <summary>
        /// The type of bonus output produced when building is near specific resources.
        /// </summary>
        public Token OutputDistBonusType { get; private set; }



        /// <summary>
        /// Initializes a new instance of the RuleNormal class.
        /// </summary>
        /// <param name="input1Type">The first input resource type</param>
        /// <param name="input1Count">The quantity of the first input resource</param>
        /// <param name="input2Type">The second input resource type</param>
        /// <param name="input2Count">The quantity of the second input resource</param>
        /// <param name="input3Type">The third input resource type</param>
        /// <param name="input3Count">The quantity of the third input resource</param>
        /// <param name="output1Type">The first output resource type</param>
        /// <param name="output1Count">The quantity of the first output resource</param>
        /// <param name="output2Type">The second output resource type</param>
        /// <param name="output2Count">The quantity of the second output resource</param>
        /// <param name="output3Type">The third output resource type</param>
        /// <param name="output3Count">The quantity of the third output resource</param>
        /// <param name="outputDistBonusCount">The quantity of bonus output when near specific resources</param>
        /// <param name="outputDistBonusType">The type of bonus output when near specific resources</param>
        public RuleNormal(Token input1Type, int input1Count, Token input2Type, int input2Count, Token input3Type, int input3Count, Token output1Type, int output1Count, Token output2Type, int output2Count, Token output3Type, 
            int output3Count, int outputDistBonusCount, Token outputDistBonusType)
        {
            Input1Type = input1Type;
            Input1Count = input1Count;
            Input2Type = input2Type;
            Input2Count = input2Count;
            Input3Type = input3Type;
            Input3Count = input3Count;
            Output1Type = output1Type;
            Output1Count = output1Count;
            Output2Type = output2Type;
            Output2Count = output2Count;
            Output3Type = output3Type;
            Output3Count = output3Count;
            OutputDistBonusCount = outputDistBonusCount;
            OutputDistBonusType = outputDistBonusType;
        }
    }
}
