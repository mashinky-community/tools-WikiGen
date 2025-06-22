using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyWikiGen
{
    public class Token
    {
        public string Name { get; private set; }
        public string IconPath { get; set; }
        public string LinkedPage { get; set; }
        public Bitmap Icon { get; private set; }
        public string ID { get; private set; }

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
