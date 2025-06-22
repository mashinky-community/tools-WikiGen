using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MashinkyWikiGen
{
    /// <summary>
    /// translates attributes strings into meaningful data
    /// </summary>
    public class XMLTranslator
    {

        public List<Token> tokMat;
        private Token invalidToken;

        public XMLTranslator(List<Token> tokMat, Token invalid)
        {
            this.tokMat = tokMat;
            invalidToken = invalid;
        }

        public Token AssignToken(string ID)
        {
            List<Token> matchingTokens = (from t in tokMat
                                          where t.ID == ID
                                          select t).ToList();
            if (matchingTokens.Count == 0)
                return invalidToken;
            else
                return matchingTokens.First();
        }

        public int SeparateCost(string s)
        {

            string[] temp = s.Split('[');  // type is in brackets, used to split
            int cost = 0;
            if (!string.IsNullOrWhiteSpace(temp[0]))
                cost = int.Parse(temp[0]);
            return cost;
        }
        public string SeparateType(string s)
        {
            string[] temp = s.Split('[');  // type is in brackets, used to split
            string type = "";
            if (!string.IsNullOrWhiteSpace(temp[0]))
            {
                if (temp.Length > 1) // type is specified
                    type = temp[1].Replace("]", string.Empty); //remove ending bracket
                else
                    type = "F0000000"; // default type when not specified in game files = money hash
            }
            return type;
        }
        /// <summary>
        /// Converts disposition attribute to X,Y dimension
        /// </summary>
        /// <param name="s"></param>
        /// <returns>X and Y dimension</returns>
        public int[] ConvDispToDim(string s)
        {
            int[] coords = new int[2] { 0, 0 };
            List<int> XTiles = new List<int>();
            List<int> YTiles = new List<int>();
            int[] dim = new int[2] { 1, 1 };
            int orientation = 0;
            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case 'L':
                        orientation--;
                        if (orientation == -1)
                            orientation = 3;
                        else if (orientation == 4)
                            orientation = 0;
                        break;

                    case 'R':
                        orientation++;
                        if (orientation == -1)
                            orientation = 3;
                        else if (orientation == 4)
                            orientation = 0;
                        break;

                    case 'U':
                        switch (orientation)
                        {
                            case 0:
                                coords[1]++;
                                break;
                            case 1:
                                coords[0]++;
                                break;
                            case 2:
                                coords[1]--;
                                break;
                            case 3:
                                coords[0]--;
                                break;
                        }
                        break;
                    case ',': //makes random orientation, undeterminable shape
                        dim[0] = 0;
                        dim[1] = s.Replace(",", "").Replace("L", "").Replace("R", "").Replace("U", "").Length; // number of tiles
                        return dim;
                    default:
                        XTiles.Add(coords[0]);
                        YTiles.Add(coords[1]);
                        break;
                }
            }
            dim[0] = XTiles.Distinct().Count();
            dim[1] = YTiles.Distinct().Count();
            return dim;
        }

        public int[] ConvLaunchesPerEpoch(string s)
        {
            return Array.ConvertAll(s.Split(','), int.Parse);
        }

        public ProductionRule CreateRule(string input, string inputEl, string output, string outputEl, int reqUpgrade, int timeSteps, string outputDistBonus)
        {
            int input1C;
            int inputt2C;
            int inputt3C;
            Token input1T;
            Token input2T;
            Token input3T;
            int output1C;
            int outputt2C;
            int outputt3C;
            Token output1T;
            Token output2T;
            Token output3T;
            int outputDBC;
            Token outputDBT;

            TranslateHashSerie(input, out input1C, out inputt2C, out inputt3C, out input1T, out input2T, out input3T);
            TranslateHashSerie(output, out output1C, out outputt2C, out outputt3C, out output1T, out output2T, out output3T);
            TranslateHashSerie(outputDistBonus, out outputDBC, out int unused1, out int unused2, out outputDBT, out Token unused3, out Token unused4);
            RuleNormal ruleN = new RuleNormal(input1T, input1C, input2T, inputt2C, input3T, inputt3C, output1T, output1C, output2T, outputt2C, output3T, outputt3C, outputDBC, outputDBT);
            TranslateHashSerie(inputEl, out input1C, out inputt2C, out inputt3C, out input1T, out input2T, out input3T);
            TranslateHashSerie(outputEl, out output1C, out outputt2C, out outputt3C, out output1T, out output2T, out output3T);
            RuleElectric ruleEl = new RuleElectric(input1T, input1C, input2T, inputt2C, input3T, inputt3C, output1T, output1C, output2T, outputt2C, output3T, outputt3C);

            return new ProductionRule(reqUpgrade, timeSteps, ruleN, ruleEl);

        }
        /// <summary>
        /// from single string - returns count of individual hashes and its type converted to Token. Works for maximum 3 different types.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result1C"></param>
        /// <param name="result2C"></param>
        /// <param name="result3C"></param>
        /// <param name="result1T"></param>
        /// <param name="result2T"></param>
        /// <param name="result3T"></param>
        public void TranslateHashSerie(string s, out int result1C, out int result2C, out int result3C, out Token result1T, out Token result2T, out Token result3T)
        {
            result1C = 0;
            result2C = 0;
            result3C = 0;
            result1T = invalidToken;
            result2T = invalidToken;
            result3T = invalidToken;
            string[] split;
            if (!string.IsNullOrEmpty(s))
            {
                split = s.Split(',');
                result1T = AssignToken(split[0]);
                result1C = 1;

                int result2I = 0;
                int result3I = 0;
                for (int i = 1; i < split.Length; i++)
                {
                    if (!split[i].Contains("B388ED9D")) //throw away christmass presents
                    {
                        if (split[i] == split[0]) //first result is always at 0 index
                            result1C++;
                        else if (result2I == 0) // not same as first type, save to second result if empty
                        {
                            result2I = i;
                            result2C++;
                            result2T = AssignToken(split[i]);
                        }
                        else if (split[i] == split[result2I])
                            result2C++;
                        else if (result3I == 0) //not same as first or second type, save to third result if empty
                        {
                            result3I = i;
                            result3C++;
                            result3T = AssignToken(split[i]);
                        }
                        else if (split[i] == split[result3I])
                            result3C++;
                    }
                }
            }
        }

        public int GetFlagValue(string s, string flag)
        {
            string[] splits = s.Split(';');
            foreach (string split in splits)
            {
                if (split.Contains(flag))
                {
                    return int.Parse(RemoveAllButNumbers(split));
                }
            }
            return 0;
        }

        private string RemoveAllButNumbers(string s)
        {
            return Regex.Replace(s, @"[^\d]*", "");
        }
    }
}
