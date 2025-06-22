using System.Drawing;
using System.Drawing.Imaging;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Reads and extracts icons from Mashinky game sprite sheets.
    /// Manages cached bitmap resources for wagons, vehicles, and planes to improve performance.
    /// </summary>
    class ImageReader
    {
        /// <summary>
        /// The cached bitmap for the wagon sprite sheet.
        /// </summary>
        private Bitmap wagonsSet;
        
        /// <summary>
        /// The cached bitmap for the vehicle sprite sheet.
        /// </summary>
        private Bitmap vehicleSet;
        
        /// <summary>
        /// The cached bitmap for the airplane sprite sheet.
        /// </summary>
        private Bitmap planesSet;
        
        /// <summary>
        /// Initializes a new instance of the ImageReader class.
        /// Loads and caches the main sprite sheets for efficient icon extraction.
        /// </summary>
        /// <param name="gameFolderPath">The path to the Mashinky game folder</param>
        public ImageReader(string gameFolderPath)
        {
            wagonsSet = Image.FromFile(gameFolderPath + "\\media\\map\\gui\\wagons_basic_set.png") as Bitmap;
            vehicleSet = Image.FromFile(gameFolderPath + "\\media\\map\\gui\\cars_basic_set.png") as Bitmap;
            planesSet = Image.FromFile(gameFolderPath + "\\media\\map\\gui\\planes_basic_set.png") as Bitmap;
        }

        /// <summary>
        /// Creates a blank crimson-colored image for use as a placeholder.
        /// Used when an icon cannot be found or loaded properly.
        /// </summary>
        /// <returns>A 10x10 crimson-colored bitmap</returns>
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

        /// <summary>
        /// Extracts an icon from a sprite sheet using the specified coordinates.
        /// Uses cached bitmaps for common sprite sheets to improve performance.
        /// </summary>
        /// <param name="iconSource">The path to the source image file</param>
        /// <param name="coords">An array containing [x, y, width, height] coordinates for extraction</param>
        /// <returns>A cropped bitmap containing the requested icon</returns>
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
