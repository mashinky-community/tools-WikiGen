using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Defines the contract for production rules that specify input and output requirements for buildings.
    /// Each rule can have up to 3 different input types and 3 different output types with their respective quantities.
    /// Rules are parsed from Mashinky XML data like: &lt;Rule input="3199AA74,3199AA74,3199AA74"&gt; where the hex code is the Token ID and count represents quantity.
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// The first input resource type required for this production rule.
        /// </summary>
        Token Input1Type { get;}
        
        /// <summary>
        /// The quantity of the first input resource required.
        /// </summary>
        int Input1Count { get;}
        
        /// <summary>
        /// The second input resource type required for this production rule.
        /// </summary>
        Token Input2Type { get;}
        
        /// <summary>
        /// The quantity of the second input resource required.
        /// </summary>
        int Input2Count { get;}
        
        /// <summary>
        /// The third input resource type required for this production rule.
        /// </summary>
        Token Input3Type { get;}
        
        /// <summary>
        /// The quantity of the third input resource required.
        /// </summary>
        int Input3Count { get;}
        
        /// <summary>
        /// The first output resource type produced by this production rule.
        /// </summary>
        Token Output1Type { get;}
        
        /// <summary>
        /// The quantity of the first output resource produced.
        /// </summary>
        int Output1Count { get;}
        
        /// <summary>
        /// The second output resource type produced by this production rule.
        /// </summary>
        Token Output2Type { get;}
        
        /// <summary>
        /// The quantity of the second output resource produced.
        /// </summary>
        int Output2Count { get;}
        
        /// <summary>
        /// The third output resource type produced by this production rule.
        /// </summary>
        Token Output3Type { get;}
        
        /// <summary>
        /// The quantity of the third output resource produced.
        /// </summary>
        int Output3Count { get;}
    }
}
