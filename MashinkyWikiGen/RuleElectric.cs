using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    public class RuleElectric : IRule
    {

        public Token Input1T { get; private set; }
        public int Input1C { get; private set; }
        public Token Input2T { get; private set; }
        public int Input2C { get; private set; }
        public Token Input3T { get; private set; }
        public int Input3C { get; private set; }
        public Token Output1T { get; private set; }
        public int Output1C { get; private set; }
        public Token Output2T { get; private set; }
        public int Output2C { get; private set; }
        public Token Output3T { get; private set; }
        public int Output3C { get; private set; }



        public RuleElectric(Token input1T, int input1C, Token input2T, int input2C, Token input3T, int input3C, Token output1T, int output1C, Token output2T, int output2C, Token output3T, int output3C)
        {
            Input1T = input1T;
            Input1C = input1C;
            Input2T = input2T;
            Input2C = input2C;
            Input3T = input3T;
            Input3C = input3C;
            Output1T = output1T;
            Output1C = output1C;
            Output2T = output2T;
            Output2C = output2C;
            Output3T = output3T;
            Output3C = output3C;
        }

    }
}
