
namespace MashinkyWikiGen
{
    /// <summary>
    /// Base class for all buildings in Mashinky including industry buildings and power plants.
    /// Contains common properties like dimensions, availability per epoch, and identification.
    /// </summary>
    public class Building
    {
        /// <summary>
        /// The unique identifier for this building from the game data.
        /// </summary>
        public string Id { get; private set; }
        
        /// <summary>
        /// The display name of the building.
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The width dimension of the building in tiles.
        /// </summary>
        public int DimX { get; private set; }
        
        /// <summary>
        /// The height dimension of the building in tiles.
        /// </summary>
        public int DimY { get; private set; }
        
        /// <summary>
        /// The number of buildings of this type spawned at the start of each epoch.
        /// This is a base number that gets multiplied based on map size.
        /// </summary>
        public int[] CountPerEpoch { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Building class.
        /// </summary>
        /// <param name="id">The unique identifier for this building from the game data</param>
        /// <param name="name">The display name of the building</param>
        /// <param name="dimX">The width dimension of the building in tiles</param>
        /// <param name="dimY">The height dimension of the building in tiles</param>
        /// <param name="countPerEpoch">The number of buildings spawned at the start of each epoch (base number, multiplied by map size)</param>
        public Building(string id, string name, int dimX, int dimY, int[] countPerEpoch)
        {
            Id = id;
            Name = name;
            DimX = dimX;
            DimY = dimY;
            CountPerEpoch = countPerEpoch;
        }
    }
}
