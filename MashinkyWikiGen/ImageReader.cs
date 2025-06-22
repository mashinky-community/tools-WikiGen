using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MashinkyWikiGen
{
    class ImageReader
    {
        private Bitmap wagonsSet;
        private Bitmap vehicleSet;
        private Bitmap planesSet;
        public ImageReader(string gameFolderPath)
        {
            wagonsSet = Image.FromFile(gameFolderPath + "\\media\\map\\gui\\wagons_basic_set.png") as Bitmap;
            vehicleSet = Image.FromFile(gameFolderPath + "\\media\\map\\gui\\cars_basic_set.png") as Bitmap;
            planesSet = Image.FromFile(gameFolderPath + "\\media\\map\\gui\\planes_basic_set.png") as Bitmap;
        }

        public Bitmap CreateBlankImage()
        {
            Bitmap image = new Bitmap(10, 10);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    image.SetPixel(x, y, System.Drawing.Color.Crimson);
                }
            }


            return image;
        }

        public Bitmap ReadIcon(string iconSource, int[] coords)
        {
            Bitmap source;
            if (iconSource.Contains("/map/gui/wagons_basic_set.png"))
                source = wagonsSet;
            else if (iconSource.Contains("/map/gui/cars_basic_set.png"))
                source = vehicleSet;
            else if (iconSource.Contains("map/gui/planes_basic_set.png"))
            {
                source = planesSet;
            }    
            else
                source = Image.FromFile(iconSource) as Bitmap;
            Rectangle cloneArea = new Rectangle(coords[0], coords[1], coords[2], coords[3]);
            Bitmap bmp = source.Clone(cloneArea, PixelFormat.Format32bppArgb);

            return bmp;

        }
    }
}
