using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Processes parsed game data and generates organized wiki resources with icons and tables.
    /// Handles creation of folder structures, icon extraction, and wiki table generation for all game entities.
    /// </summary>
    class DataProcessor
    {
        /// <summary>
        /// The list of wagon vehicles to process.
        /// </summary>
        private List<VehicleBase> wagons;
        
        /// <summary>
        /// The list of engine vehicles to process.
        /// </summary>
        private List<VehicleBase> engines;
        
        /// <summary>
        /// The list of car vehicles to process.
        /// </summary>
        private List<VehicleBase> cars;
        
        /// <summary>
        /// The list of airplane vehicles to process.
        /// </summary>
        private List<VehicleBase> airplanes;
        
        /// <summary>
        /// The list of token resources to process.
        /// </summary>
        private List<Token> tokens;
        
        /// <summary>
        /// The list of material resources to process.
        /// </summary>
        private List<Token> materials;
        
        /// <summary>
        /// The list of buildings to process.
        /// </summary>
        private List<Building> buildings;
        
        /// <summary>
        /// The list of town upgrades to process.
        /// </summary>
        List<BuildingUpgrade> townUpgrades;
        
        /// <summary>
        /// The output folder path where all generated wiki resources are saved.
        /// </summary>
        public string dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MashWikiGen";
        
        /// <summary>
        /// The wiki script generator used to create MediaWiki-formatted tables.
        /// </summary>
        private WikiScriptGen WSGen;
        
        /// <summary>
        /// The list of icon resources to process.
        /// </summary>
        private List<Token> icons;

        /// <summary>
        /// Initializes a new instance of the DataProcessor class.
        /// </summary>
        /// <param name="airplanes">The list of airplane vehicles to process</param>
        /// <param name="wagons">The list of wagon vehicles to process</param>
        /// <param name="engines">The list of engine vehicles to process</param>
        /// <param name="cars">The list of car vehicles to process</param>
        /// <param name="tokens">The list of token resources to process</param>
        /// <param name="materials">The list of material resources to process</param>
        /// <param name="buildings">The list of buildings to process</param>
        /// <param name="townUpgrades">The list of town upgrades to process</param>
        /// <param name="resources">The resources manager for wiki generation</param>
        /// <param name="icons">The list of icon resources to process</param>
        public DataProcessor(List<VehicleBase> airplanes, List<VehicleBase> wagons, List<VehicleBase> engines, List<VehicleBase> cars, List<Token> tokens, List<Token> materials, List<Building> buildings, List<BuildingUpgrade> townUpgrades ,Resources resources, List<Token> icons)
        {
            this.wagons = wagons;
            this.engines = engines;
            this.cars = cars;
            this.airplanes = airplanes;
            this.tokens = tokens;
            this.materials = materials;
            this.buildings = buildings;
            this.icons = icons;
            this.townUpgrades = townUpgrades;
            WSGen = new WikiScriptGen(resources);
        }

        /// <summary>
        /// Generates and saves all token and icon images to their respective folders.
        /// Creates /Tokens/ and /Icons/ folders and saves PNG files for each resource.
        /// </summary>
        public void GenerateTokens()
        {
            string path = dataFolder + "/Tokens/";
            Directory.CreateDirectory(path);
            foreach (Token token in tokens)
            {
                token.Icon.Save(path + token.IconPath, ImageFormat.Png);
            }
            path = dataFolder + "/Icons/";
            Directory.CreateDirectory(path);
            foreach (Token token in icons)
            {
                try
                {
                    token.Icon.Save(path + token.IconPath, ImageFormat.Png);
                }
                catch
                {

                    File.AppendAllText("traceWikiGen.txt", "\nFailed to save icon for token " + token.Name);
                }
                
            }

        }

        /// <summary>
        /// Generates and saves all material icon images to the Materials folder.
        /// Creates /Materials/ folder and saves PNG files for each material resource.
        /// </summary>
        public void GenerateMaterials()
        {
            string path = dataFolder + "/Materials/";
            Directory.CreateDirectory(path);
            foreach (Token material in materials)
            {
                try
                {
                    material.Icon.Save(path + material.IconPath, ImageFormat.Png);
                }
                catch
                {
                    File.AppendAllText("traceWikiGen.txt", "\nFailed to save icon for material " + material.Name);
                }
                
            }
        }

        /// <summary>
        /// Generates wagon wiki resources organized by cargo type.
        /// Creates individual folders for each wagon with icons and parameter tables.
        /// </summary>
        public void GenerateWagons()
        {
            string path = dataFolder + "/Wagons/";
            Directory.CreateDirectory(path);
            GenerateTableByCargo(wagons, path);
            foreach (VehicleBase w in wagons)
            {
                string fullPath = $"{path}/{w.Cargo.Name}/{w.Name}/";
                Directory.CreateDirectory(fullPath);
                try
                {
                    w.Icon.Save(fullPath + w.Name + " icon" + ".png", ImageFormat.Png);
                }
                catch
                {

                    File.AppendAllText("traceWikiGen.txt", "\nFailed to save icon for wagon " + w.Name);
                }
                
                WSGen.CreateSingleTable(fullPath, w);
            }
        }

        /// <summary>
        /// Generates engine wiki resources organized by epoch.
        /// Creates individual folders for each engine with icons and parameter tables.
        /// </summary>
        public void GenerateEngines()
        {
            string path = dataFolder + "/Engines/";
            Directory.CreateDirectory(path);
            GenerateTableByEpoch(engines, path);
            foreach (Engine e in engines)
            {
                string fullPath = "";
                fullPath = $"{path}/{e.StartEpoch}. epoch/{e.Name}/";
                Directory.CreateDirectory(fullPath);
                try
                {
                    e.Icon.Save(fullPath + e.Name + " icon" + ".png", ImageFormat.Png);
                }
                catch
                {

                    File.AppendAllText("traceWikiGen.txt", "\nFailed to save icon for engine " + e.Name);
                }
                e.Icon.Save(fullPath + e.Name + " icon" + ".png", ImageFormat.Png);
                WSGen.CreateSingleTable(fullPath, e);

            }
        }

        /// <summary>
        /// Generates car wiki resources organized by cargo type.
        /// Creates individual folders for each car with icons and parameter tables.
        /// </summary>
        public void GenerateCars()
        {
            string path = dataFolder + "/Cars/";
            Directory.CreateDirectory(path);
            GenerateTableByCargo(cars, path);
            foreach (Engine v in cars)
            {
                string fullPath = $"{path}/{v.Cargo.Name}/{v.Name}/";
                Directory.CreateDirectory(fullPath);
                try
                {
                    v.Icon.Save(fullPath + v.Name + " icon" + ".png", ImageFormat.Png);
                }
                catch
                {
                    File.AppendAllText("traceWikiGen.txt", "\nFailed to save icon for car " + v.Name);
                }
                WSGen.CreateSingleTable(fullPath, v);
            }
        }

        /// <summary>
        /// Generates airplane wiki resources organized by epoch and cargo type.
        /// Creates individual folders for each airplane with icons and parameter tables.
        /// </summary>
        public void GenerateAirplanes()
        {
            string path = dataFolder + "/Airplanes/";
            Directory.CreateDirectory(path);
            GenerateTableByEpoch(airplanes, path);
            GenerateTableByCargo(airplanes, path);
            foreach (Airplane airplane in airplanes)
            {
                string fullPath = $"{path}/{airplane.Cargo.Name}/{airplane.Name}/";
                Directory.CreateDirectory(fullPath);
                try
                {
                    airplane.Icon.Save(fullPath + airplane.Name + " icon" + ".png", ImageFormat.Png);
                }
                catch
                {
                    File.AppendAllText("traceWikiGen.txt", "\nFailed to save icon for airplane " + airplane.Name);
                }
                WSGen.CreateSingleTable(fullPath, airplane);
            }
        }

        /// <summary>
        /// Generates building wiki resources organized by type.
        /// Creates folders for industry buildings, town extensions, and decorations with icons.
        /// </summary>
        public void GenerateBuildings()
        {
            string path = dataFolder + "/Buildings/";
            Directory.CreateDirectory(path);
            GenerateBTableByType(townUpgrades , buildings, path);
            List<TownExtension> extensions = (from te in townUpgrades
                                              where te is TownExtension
                                              select te as TownExtension).ToList();
            GenerateTETableByType(extensions, path);
            path += "/Icons/";
            Directory.CreateDirectory(path);
            foreach (IndustryBuilding b in buildings)
            {
                try
                {
                    b.Icon.Save($"{path}icon {b.Name}.png", ImageFormat.Png);
                }
                catch
                {
                    File.AppendAllText("traceWikiGen.txt", "\nFailed to save icon for building " + b.Name);
                }
                
            }
        }

        /// <summary>
        /// Generates vehicle tables organized by epoch (time period).
        /// Creates folders for each epoch (0-7) with corresponding vehicle tables.
        /// </summary>
        /// <param name="wagons">The list of vehicles to organize by epoch</param>
        /// <param name="path">The base path where epoch folders will be created</param>
        public void GenerateTableByEpoch(List<VehicleBase> wagons, string path)
        {
            for (int i = 0; i < 8; i++)
            {
                string ePath = path + $"{ i}. epoch/";
                Directory.CreateDirectory(ePath);
                WSGen.CreateMultiTable(ePath, (from w in wagons where w.StartEpoch == i orderby w.Name select w).ToList());
            }
        }

        /// <summary>
        /// Generates vehicle tables organized by cargo type.
        /// Creates folders for each cargo type with corresponding vehicle tables.
        /// </summary>
        /// <param name="wagons">The list of vehicles to organize by cargo type</param>
        /// <param name="path">The base path where cargo folders will be created</param>
        public void GenerateTableByCargo(List<VehicleBase> wagons, string path)
        {
            List<string> cargos = (from w in wagons orderby w.Cargo.Name select w.Cargo.Name).Distinct().ToList();
            foreach (string cargo in cargos)
            {
                string ePath = path + cargo + "/";
                Directory.CreateDirectory(ePath);
                WSGen.CreateMultiTable(ePath, (from w in wagons where w.Cargo.Name == cargo orderby w.StartEpoch select w).ToList());
            }
        }


        /// <summary>
        /// Generates building tables organized by building type.
        /// Creates folders for industrial buildings, town extensions (relax/amenities/luxury), and decorations.
        /// </summary>
        /// <param name="townUpgrades">The list of town upgrades to organize</param>
        /// <param name="buildings">The list of buildings to organize</param>
        /// <param name="path">The base path where type folders will be created</param>
        public void GenerateBTableByType(List<BuildingUpgrade> townUpgrades, List<Building> buildings, string path)
        {
            foreach (Building b in buildings)
            {
                if (b is IndustryBuilding)
                {
                    string ePath = path + "Industrial buildings/" + b.Name + "/";
                    Directory.CreateDirectory(ePath);
                    WSGen.CreateSingleTable(ePath, b);
                    WSGen.CreateMultiTable(ePath, (b as IndustryBuilding).Upgrades);
                }
            }

            foreach (BuildingUpgrade ext in townUpgrades)
            {
                string subfolder = "";
                if (ext is TownExtension)
                {
                    TownExtension te = ext as TownExtension;
                    if (te.Relax > 0)
                        subfolder = "Relax/";
                    else if (te.Amenities > 0)
                        subfolder = "Amenities/";
                    else if (te.Luxury > 0)
                        subfolder = "Luxury/";
                    string ePath = path + "Town extensions/" + subfolder + ext.Name + "/";
                    Directory.CreateDirectory(ePath);
                    WSGen.CreateSingleTable(ePath, te);
                }
                else if (ext is TownDecoration)
                {
                    string ePath = path + "Town decorations/" + ext.Name + "/";
                    Directory.CreateDirectory(ePath);
                    WSGen.CreateSingleTable(ePath, ext as TownDecoration);
                }
            }
        }

        /// <summary>
        /// Generates town extension tables organized by bonus type.
        /// Creates separate tables for relax, amenities, and luxury town extensions.
        /// </summary>
        /// <param name="ext">The list of town extensions to organize</param>
        /// <param name="path">The base path where extension tables will be created</param>
        public void GenerateTETableByType(List<TownExtension> ext, string path)
        {
            string ePath = path + "Town extensions/";
            List<TownExtension> relax = (from e in ext
                                         where e.Relax > 0
                                         select e).ToList();
            WSGen.CreateMultiTable(ePath + "Relax/", relax);
            List<TownExtension> amenities = (from e in ext
                                         where e.Amenities > 0
                                         select e).ToList();
            WSGen.CreateMultiTable(ePath + "Amenities/", amenities);
            List<TownExtension> luxury = (from e in ext
                                         where e.Luxury > 0
                                         select e).ToList();
            WSGen.CreateMultiTable(ePath + "Luxury/", luxury);
        }

    }
}
