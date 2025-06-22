using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Translates attribute strings from Mashinky XML data into meaningful game objects.
    /// Handles parsing of resource costs, production rules, and token identification.
    /// </summary>
    public class XMLTranslator
    {
        /// <summary>
        /// The combined list of tokens and materials used for ID lookup.
        /// </summary>
        public List<Token> tokMat;
        
        /// <summary>
        /// The fallback token used when a requested ID cannot be found.
        /// </summary>
        private Token invalidToken;

        /// <summary>
        /// Initializes a new instance of the XMLTranslator class.
        /// </summary>
        /// <param name="tokMat">The combined list of tokens and materials used for ID lookup</param>
        /// <param name="invalid">The fallback token used when a requested ID cannot be found</param>
        public XMLTranslator(List<Token> tokMat, Token invalid)
        {
            this.tokMat = tokMat;
            invalidToken = invalid;
        }

        /// <summary>
        /// Finds and returns the token with the specified ID.
        /// </summary>
        /// <param name="ID">The hex ID of the token to find</param>
        /// <returns>The matching token, or the invalid token if no match is found</returns>
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

        /// <summary>
        /// Extracts the cost amount from a cost string before the bracket.
        /// </summary>
        /// <param name="s">The cost string in format "amount[type]"</param>
        /// <returns>The numeric cost amount</returns>
        public int SeparateCost(string s)
        {
            string[] temp = s.Split('[');  // type is in brackets, used to split
            int cost = 0;
            if (!string.IsNullOrWhiteSpace(temp[0]))
                cost = int.Parse(temp[0]);
            return cost;
        }
        /// <summary>
        /// Extracts the token type ID from a cost string within the brackets.
        /// </summary>
        /// <param name="s">The cost string in format "amount[type]"</param>
        /// <returns>The token type ID from within the brackets</returns>
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
            int input1Count;
            int input2Count;
            int input3Count;
            Token input1Type;
            Token input2Type;
            Token input3Type;
            int output1Count;
            int output2Count;
            int output3Count;
            Token output1Type;
            Token output2Type;
            Token output3Type;
            int outputDistBonusCount;
            Token outputDistBonusType;

            TranslateHashSerie(input, out input1Count, out input2Count, out input3Count, out input1Type, out input2Type, out input3Type);
            TranslateHashSerie(output, out output1Count, out output2Count, out output3Count, out output1Type, out output2Type, out output3Type);
            TranslateHashSerie(outputDistBonus, out outputDistBonusCount, out int unused1, out int unused2, out outputDistBonusType, out Token unused3, out Token unused4);
            RuleNormal ruleN = new RuleNormal(input1Type, input1Count, input2Type, input2Count, input3Type, input3Count, output1Type, output1Count, output2Type, output2Count, output3Type, output3Count, outputDistBonusCount, outputDistBonusType);
            TranslateHashSerie(inputEl, out input1Count, out input2Count, out input3Count, out input1Type, out input2Type, out input3Type);
            TranslateHashSerie(outputEl, out output1Count, out output2Count, out output3Count, out output1Type, out output2Type, out output3Type);
            RuleElectric ruleEl = new RuleElectric(input1Type, input1Count, input2Type, input2Count, input3Type, input3Count, output1Type, output1Count, output2Type, output2Count, output3Type, output3Count);

            return new ProductionRule(reqUpgrade, timeSteps, ruleN, ruleEl);

        }
        /// <summary>
        /// From single string - returns count of individual hashes and its type converted to Token. Works for maximum 3 different types.
        /// Parses comma-separated hex IDs from XML and converts them to Token objects with counts.
        /// </summary>
        /// <param name="s">The comma-separated string of hex token IDs</param>
        /// <param name="result1Count">The count of the first token type found</param>
        /// <param name="result2Count">The count of the second token type found</param>
        /// <param name="result3Count">The count of the third token type found</param>
        /// <param name="result1Type">The first token type found</param>
        /// <param name="result2Type">The second token type found</param>
        /// <param name="result3Type">The third token type found</param>
        public void TranslateHashSerie(string s, out int result1Count, out int result2Count, out int result3Count, out Token result1Type, out Token result2Type, out Token result3Type)
        {
            result1Count = 0;
            result2Count = 0;
            result3Count = 0;
            result1Type = invalidToken;
            result2Type = invalidToken;
            result3Type = invalidToken;
            string[] split;
            if (!string.IsNullOrEmpty(s))
            {
                split = s.Split(',');
                result1Type = AssignToken(split[0]);
                result1Count = 1;

                int result2I = 0;
                int result3I = 0;
                for (int i = 1; i < split.Length; i++)
                {
                    if (!split[i].Contains("B388ED9D")) //throw away christmass presents
                    {
                        if (split[i] == split[0]) //first result is always at 0 index
                            result1Count++;
                        else if (result2I == 0) // not same as first type, save to second result if empty
                        {
                            result2I = i;
                            result2Count++;
                            result2Type = AssignToken(split[i]);
                        }
                        else if (split[i] == split[result2I])
                            result2Count++;
                        else if (result3I == 0) //not same as first or second type, save to third result if empty
                        {
                            result3I = i;
                            result3Count++;
                            result3Type = AssignToken(split[i]);
                        }
                        else if (split[i] == split[result3I])
                            result3Count++;
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
