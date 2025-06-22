using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Represents a game resource token (currency, material, or cargo) with associated icon and wiki information.
    /// Used for displaying resources in wiki tables and linking to their respective wiki pages.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The display name of the token.
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The file path to the token's icon image.
        /// </summary>
        public string IconPath { get; set; }
        
        /// <summary>
        /// The wiki page name that this token should link to.
        /// </summary>
        public string LinkedPage { get; set; }
        
        /// <summary>
        /// The bitmap image of the token's icon.
        /// </summary>
        public Bitmap Icon { get; private set; }
        
        /// <summary>
        /// The unique identifier for this token from the game data.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Token class.
        /// </summary>
        /// <param name="name">The display name of the token</param>
        /// <param name="iconPath">The file path to the token's icon image</param>
        /// <param name="linkedPage">The wiki page name that this token should link to</param>
        /// <param name="icon">The bitmap image of the token's icon</param>
        /// <param name="id">The unique identifier for this token from the game data</param>
        public Token(string name, string iconPath, string linkedPage, Bitmap icon, string id)
        {
            Name = name;
            IconPath = iconPath;
            LinkedPage = linkedPage;
            Icon = icon;
            ID = id;
        }
    }
}
