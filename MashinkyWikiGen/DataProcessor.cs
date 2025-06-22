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
    class DataProcessor
    {
        private List<VehicleBase> wagons;
        private List<VehicleBase> engines;
        private List<VehicleBase> cars;
        private List<VehicleBase> airplanes;
        private List<Token> tokens;
        private List<Token> materials;
        private List<Building> buildings;
        List<BuildingUpgrade> townUpgrades;
        public string dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MashWikiGen";
        private WikiScriptGen WSGen;
        private List<Token> icons;

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

        public void GenerateTableByEpoch(List<VehicleBase> wagons, string path)
        {
            for (int i = 0; i < 8; i++)
            {
                string ePath = path + $"{ i}. epoch/";
                Directory.CreateDirectory(ePath);
                WSGen.CreateMultiTable(ePath, (from w in wagons where w.StartEpoch == i orderby w.Name select w).ToList());
            }
        }

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
