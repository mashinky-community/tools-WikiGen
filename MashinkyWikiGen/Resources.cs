using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Windows.Media.Imaging;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Central coordinator class that manages all game data processing and wiki generation.
    /// Handles initialization of tokens, materials, XML reading, and orchestrates the entire wiki generation workflow.
    /// </summary>
    public class Resources
    {
        // Token properties for currencies
        /// <summary>The money token used for purchases.</summary>
        public Token MoneyToken { get; set; }
        /// <summary>The timber token used for construction.</summary>
        public Token TimberToken { get; set; }
        /// <summary>The coal token used for fuel and production.</summary>
        public Token CoalToken { get; set; }
        /// <summary>The iron token used for production.</summary>
        public Token IronToken { get; set; }
        /// <summary>The diesel token used for fuel.</summary>
        public Token DieselToken { get; set; }
        /// <summary>The steel token used for advanced production.</summary>
        public Token SteelToken { get; set; }
        /// <summary>The electricity token used for power.</summary>
        public Token ElectricityToken { get; set; }
        /// <summary>The cement token used for construction.</summary>
        public Token CementToken { get; set; }
        /// <summary>The first reserve token (unused).</summary>
        public Token Reserve1Token { get; set; }
        /// <summary>The second reserve token (unused).</summary>
        public Token Reserve2Token { get; set; }

        // Material properties for cargo types
        /// <summary>The log material token for raw timber.</summary>
        public Token LogMaterial { get; set; }
        /// <summary>The coal material token for mining.</summary>
        public Token CoalMaterial { get; set; }
        /// <summary>The iron ore material token for mining.</summary>
        public Token IronOreMaterial { get; set; }
        /// <summary>The crude oil material token for extraction.</summary>
        public Token CrudeOilMaterial { get; set; }
        /// <summary>The sand material token for construction.</summary>
        public Token SandMaterial { get; set; }
        /// <summary>The timber material token for processed wood.</summary>
        public Token TimberMaterial { get; set; }
        /// <summary>The iron material token for processed metal.</summary>
        public Token IronMaterial { get; set; }
        /// <summary>The goods material token for manufactured items.</summary>
        public Token GoodsMaterial { get; set; }
        /// <summary>The food material token for consumption.</summary>
        public Token FoodMaterial { get; set; }
        /// <summary>The diesel material token for refined fuel.</summary>
        public Token DieselMaterial { get; set; }
        /// <summary>The steel material token for advanced manufacturing.</summary>
        public Token SteelMaterial { get; set; }
        /// <summary>The limestone material token for cement production.</summary>
        public Token LimestoneMaterial { get; set; }
        /// <summary>The cement material token for construction.</summary>
        public Token CementMaterial { get; set; }
        /// <summary>The passengers material token for transportation.</summary>
        public Token PassengersMaterial { get; set; }
        /// <summary>The mail material token for postal services.</summary>
        public Token MailMaterial { get; set; }
        /// <summary>The garbage material token for waste management.</summary>
        public Token GarbageMaterial { get; set; }
        /// <summary>The second reserve material token (unused).</summary>
        public Token Reserve2Material { get; set; }

        /// <summary>The XML reader for parsing game data files.</summary>
        private XMLReader xmlReader;
        /// <summary>The data processor for generating wiki resources.</summary>
        private DataProcessor dataProcessor;
        /// <summary>The list of all token resources.</summary>
        private List<Token> tokens = new List<Token>();
        /// <summary>The list of all material resources.</summary>
        private List<Token> materials = new List<Token>();
        /// <summary>The list of paths to WGT mod files.</summary>
        private List<string> WGTmodPaths = new List<string>();
        /// <summary>The list of paths to text mod files.</summary>
        private List<string> textmodPaths = new List<string>();
        /// <summary>The list of icon resources.</summary>
        private List<Token> icons = new List<Token>();

        // Token IDs for currencies (TID = Token ID)
        /// <summary>The hex ID for the money token.</summary>
        public string MoneyTID { get; set; } = "F0000000";
        /// <summary>The hex ID for the timber token.</summary>
        public string TimberTID { get; set; } = "F07C5273";
        /// <summary>The hex ID for the coal token.</summary>
        public string CoalTID { get; set; } = "F1283C03";
        /// <summary>The hex ID for the iron token.</summary>
        public string IronTID { get; set; } = "F27DB683";
        /// <summary>The hex ID for the diesel token.</summary>
        public string DieselTID { get; set; } = "F29BF6C1";
        /// <summary>The hex ID for the steel token.</summary>
        public string SteelTID { get; set; } = "F8E3D3EC";
        /// <summary>The hex ID for the electricity token.</summary>
        public string ElectricityTID { get; set; } = "F9D8BF64";
        /// <summary>The hex ID for the cement token.</summary>
        public string CementTID { get; set; } = "FAD8464A";
        /// <summary>The hex ID for the first reserve token.</summary>
        public string Reserve1TID { get; set; } = "";
        /// <summary>The hex ID for the second reserve token.</summary>
        public string Reserve2TID { get; set; } = "";

        // Material IDs (MID = Material ID)
        /// <summary>The hex ID for the log material.</summary>
        public string LogMID { get; set; } = "3199AA74";
        /// <summary>The hex ID for the timber material.</summary>
        public string TimberMID { get; set; } = "448DDF23";
        /// <summary>The hex ID for the coal material.</summary>
        public string CoalMID { get; set; } = "6ACBCBA9";
        /// <summary>The hex ID for the iron ore material.</summary>
        public string IronOreMID { get; set; } = "61A13BCE";
        /// <summary>The hex ID for the crude oil material.</summary>
        public string CrudeOilMID { get; set; } = "19CFBDA7";
        /// <summary>The hex ID for the sand material.</summary>
        public string SandMID { get; set; } = "94032E35";
        /// <summary>The hex ID for the iron material.</summary>
        public string IronMID { get; set; } = "7C13D1C9";
        /// <summary>The hex ID for the goods material.</summary>
        public string GoodsMID { get; set; } = "B388ED8C";
        /// <summary>The hex ID for the food material.</summary>
        public string FoodMID { get; set; } = "C74198A7";
        /// <summary>The hex ID for the diesel material.</summary>
        public string DieselMID { get; set; } = "53F1B093";
        /// <summary>The hex ID for the steel material.</summary>
        public string SteelMID { get; set; } = "88D1A491";
        /// <summary>The hex ID for the passengers material.</summary>
        public string PassengersMID { get; set; } = "0BA458C8";
        /// <summary>The hex ID for the mail material.</summary>
        public string MailMID { get; set; } = "0F822763";
        /// <summary>The hex ID for the limestone material.</summary>
        public string LimestoneMID { get; set; } = "762F8F3E";
        /// <summary>The hex ID for the cement material.</summary>
        public string CementMID { get; set; } = "8DFA75B5";
        /// <summary>The hex ID for the garbage material.</summary>
        public string GarbageMID { get; set; } = "9A988702";
        /// <summary>The hex ID for the second reserve material.</summary>
        public string Reserve2MID { get; set; } = "";

        /// <summary>The path to the cargo icons image file.</summary>
        public string CargoIconsPath { get; set; } = "/media/map/gui/cargo_basic_set.png";
        /// <summary>The wiki link for pollution information.</summary>
        public string LinkPollution { get; set; } = "City Upgrades#Pollution";
        /// <summary>The electrification token for power systems.</summary>
        public Token Electrification { get; private set; }
        /// <summary>The hex ID for the electrification token.</summary>
        public string ElectrificationTID { get; set; } = "CE900010";
        /// <summary>The wiki link for game tick information.</summary>
        public string LinkTick { get; set; } = "Game_tick";
        /// <summary>The wiki link for operating costs information.</summary>
        public string LinkOperatingCost { get; set; } = "Operating_Costs";




        public Resources()
        {
            xmlReader = new XMLReader(tokens, materials, textmodPaths);
            InitializeAllTokens();
            xmlReader.ConcatTokMat();
            File.AppendAllText("traceWikiGen.txt", "\nTokens initalizied");
            try
            {
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MashWikiGen" + @"\mods\", true);
            }
            catch (Exception)
            {
            }
            UnpackTexts();
            File.AppendAllText("traceWikiGen.txt", "\nVanila wgt loaded");
            File.AppendAllText("traceWikiGen.txt", "\nZip mods loaded");
            dataProcessor = new DataProcessor(xmlReader.Airplanes ,xmlReader.Wagons, xmlReader.Engines, xmlReader.Cars, tokens, materials, xmlReader.Buildings, xmlReader.TownUpgrades, this, icons);
            dataProcessor.GenerateTokens();
            dataProcessor.GenerateMaterials();
        }

        public void ReadUniqueVehicles()
        {
            UnpackWGTZips();
            WGTmodPaths = WGTmodPaths.Distinct().ToList();
            foreach (string path in WGTmodPaths)
            {
                xmlReader.ReadWGT(path);
            }
        }

        public void UnpackWGTZips()
        {
            string[] zipMods = Directory.GetFiles(xmlReader.gameFolderPath + "\\mods\\", "*.zip", SearchOption.TopDirectoryOnly);
            string path = "";
            foreach (string s in zipMods)
            {
                if (!s.Contains("christmas"))
                {


                    using (ZipArchive zip = ZipFile.Open(s, ZipArchiveMode.Read))
                    {

                        foreach (ZipArchiveEntry entry in zip.Entries)
                        {
                            if (entry.Name == "wagon_types.xml" || entry.Name == "tcoords.xml")
                            {

                                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MashWikiGen" + @"\mods\" + Path.GetFileNameWithoutExtension(s); // folders structure to extract mods into
                                Directory.CreateDirectory(path + "\\media\\" + Path.GetFileName(Path.GetDirectoryName(entry.FullName)));
                                WGTmodPaths.Add(path);
                                try
                                {
                                    entry.ExtractToFile(path + "\\media\\" + entry.FullName);
                                }
                                catch (Exception)
                                {

                                }

                            }
                        }
                    }
                }
            }

        }

        public void UnpackTexts()
        {
            string[] zipMods = Directory.GetFiles(xmlReader.gameFolderPath + "\\mods\\", "*.zip", SearchOption.TopDirectoryOnly);
            string path = "";
            foreach (string s in zipMods)
            {
                if (!s.Contains("christmas") && !s.Contains("news") && !s.Contains("vc1m1"))
                {


                    using (ZipArchive zip = ZipFile.Open(s, ZipArchiveMode.Read))
                    {

                        foreach (ZipArchiveEntry entry in zip.Entries)
                        {
                            if (entry.Name == "texts.xml")
                            {

                                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MashWikiGen" + @"\mods\" + Path.GetFileNameWithoutExtension(s); // folders structure to extract mods into
                                Directory.CreateDirectory(path + "\\media\\" + Path.GetFileName(Path.GetDirectoryName(entry.FullName)));
                                textmodPaths.Add(path);
                                try
                                {
                                   entry.ExtractToFile(path + "\\media\\" + entry.FullName);
                                }
                                catch (Exception)
                                {

                                }

                            }
                        }
                    }
                }
            }
        }

        public void GenerateAllData()
        {
            ReadUniqueVehicles();
            xmlReader.ReadWGT(xmlReader.gameFolderPath);
            xmlReader.ReadBuildings(xmlReader.gameFolderPath);
            dataProcessor.GenerateWagons();
            dataProcessor.GenerateEngines();
            dataProcessor.GenerateAirplanes();
            dataProcessor.GenerateCars();
            Process.Start("explorer.exe", dataProcessor.dataFolder);
        }

        public void GenerateAllVehicles()
        {
            ReadUniqueVehicles();
            xmlReader.ReadWGT(xmlReader.gameFolderPath);
            dataProcessor.GenerateWagons();
            dataProcessor.GenerateEngines();
            dataProcessor.GenerateAirplanes();
            dataProcessor.GenerateCars();
            Process.Start("explorer.exe", dataProcessor.dataFolder);
        }

        public void GenerateAllBuildings()
        {
            xmlReader.ReadBuildings(xmlReader.gameFolderPath);
            dataProcessor.GenerateBuildings();
            Process.Start("explorer.exe", dataProcessor.dataFolder);
        }

        public void InitializeAllTokens()
        {
            MoneyToken = new Token("Money", "token money.png", "Token", xmlReader.ReadIcon(xmlReader.ReadTokenIconHash(MoneyTID), CargoIconsPath, xmlReader.gameFolderPath), MoneyTID);
            tokens.Add(MoneyToken);
            TimberToken = new Token("Timber", "token timber.png", "Token", xmlReader.ReadIcon(xmlReader.ReadTokenIconHash(TimberTID), CargoIconsPath, xmlReader.gameFolderPath), TimberTID);
            tokens.Add(TimberToken);
            CoalToken = new Token("Coal", "token coal.png", "Token", xmlReader.ReadIcon(xmlReader.ReadTokenIconHash(CoalTID), CargoIconsPath, xmlReader.gameFolderPath), CoalTID);
            tokens.Add(CoalToken);
            IronToken = new Token("Iron", "token iron.png", "Token", xmlReader.ReadIcon(xmlReader.ReadTokenIconHash(IronTID), CargoIconsPath, xmlReader.gameFolderPath), IronTID);
            tokens.Add(IronToken);
            DieselToken = new Token("Diesel", "token diesel.png", "Token", xmlReader.ReadIcon(xmlReader.ReadTokenIconHash(DieselTID), CargoIconsPath, xmlReader.gameFolderPath), DieselTID);
            tokens.Add(DieselToken);
            SteelToken = new Token("Steel", "token steel.png", "Token", xmlReader.ReadIcon(xmlReader.ReadTokenIconHash(SteelTID), CargoIconsPath, xmlReader.gameFolderPath), SteelTID);
            tokens.Add(SteelToken);
            ElectricityToken = new Token("Electricity", "token electricity.png", "Token", xmlReader.ReadIcon(xmlReader.ReadTokenIconHash(ElectricityTID), CargoIconsPath, xmlReader.gameFolderPath), ElectricityTID);
            tokens.Add(ElectricityToken);
            CementToken = new Token("Cement", "token cement.png", "Token", xmlReader.ReadIcon(xmlReader.ReadTokenIconHash(CementTID), CargoIconsPath, xmlReader.gameFolderPath), CementTID);
            tokens.Add(CementToken);
            Reserve1Token = new Token("Reserve1", "token reserve1.png", "Token", xmlReader.ReadIcon(Reserve1TID, CargoIconsPath, xmlReader.gameFolderPath), Reserve1TID);
            tokens.Add(Reserve1Token);
            Reserve2Token = new Token("Reserve2", "token reserve2.png", "Token", xmlReader.ReadIcon(Reserve2TID, CargoIconsPath, xmlReader.gameFolderPath), Reserve2TID);
            tokens.Add(Reserve2Token);

            LogMaterial = new Token("Log", "icon logs.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(LogMID), CargoIconsPath, xmlReader.gameFolderPath), LogMID);
            materials.Add(LogMaterial);
            CoalMaterial = new Token("Coal", "icon coal.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(CoalMID), CargoIconsPath, xmlReader.gameFolderPath), CoalMID);
            materials.Add(CoalMaterial);
            IronOreMaterial = new Token("Iron ore", "icon iron ore.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(IronOreMID), CargoIconsPath, xmlReader.gameFolderPath), IronOreMID);
            materials.Add(IronOreMaterial);
            CrudeOilMaterial = new Token("Crude oil", "icon oil.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(CrudeOilMID), CargoIconsPath, xmlReader.gameFolderPath), CrudeOilMID);
            materials.Add(CrudeOilMaterial);
            SandMaterial = new Token("Sand", "icon sand.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(SandMID), CargoIconsPath, xmlReader.gameFolderPath), SandMID);
            materials.Add(SandMaterial);
            TimberMaterial = new Token("Timber", "icon timber.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(TimberMID), CargoIconsPath, xmlReader.gameFolderPath), TimberMID);
            materials.Add(TimberMaterial);
            IronMaterial = new Token("Iron", "icon iron.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(IronMID), CargoIconsPath, xmlReader.gameFolderPath), IronMID);
            materials.Add(IronMaterial);
            GoodsMaterial = new Token("Goods", "icon goods.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(GoodsMID), CargoIconsPath, xmlReader.gameFolderPath), GoodsMID);
            materials.Add(GoodsMaterial);
            FoodMaterial = new Token("Food", "icon food.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(FoodMID), CargoIconsPath, xmlReader.gameFolderPath), FoodMID);
            materials.Add(FoodMaterial);
            DieselMaterial = new Token("Diesel", "icon diesel.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(DieselMID), CargoIconsPath, xmlReader.gameFolderPath), DieselMID);
            materials.Add(DieselMaterial);
            SteelMaterial = new Token("Steel", "icon steel.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(SteelMID), CargoIconsPath, xmlReader.gameFolderPath), SteelMID);
            materials.Add(SteelMaterial);
            PassengersMaterial = new Token("Passengers", "icon passengers.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(PassengersMID), CargoIconsPath, xmlReader.gameFolderPath), PassengersMID);
            materials.Add(PassengersMaterial);
            LimestoneMaterial = new Token("Limestone", "icon limestone.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(LimestoneMID), CargoIconsPath, xmlReader.gameFolderPath), LimestoneMID);
            materials.Add(LimestoneMaterial);
            CementMaterial = new Token("Cement", "icon cement.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(CementMID), CargoIconsPath, xmlReader.gameFolderPath), CementMID);
            materials.Add(CementMaterial);
            MailMaterial = new Token("Mail", "icon mail.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(MailMID), CargoIconsPath, xmlReader.gameFolderPath), MailMID);
            materials.Add(MailMaterial);
            GarbageMaterial = new Token("Garbage", "icon garbage.png", "Material", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(GarbageMID), CargoIconsPath, xmlReader.gameFolderPath), GarbageMID);
            materials.Add(GarbageMaterial);
            Reserve2Material = new Token("Reserve2", "icon reserve2.png", "Material", xmlReader.ReadIcon(Reserve2MID, CargoIconsPath, xmlReader.gameFolderPath), Reserve2MID);
            materials.Add(Reserve2Material);

            Electrification = new Token("Electrification", "E_energy.png", "Electrification", xmlReader.ReadIcon(xmlReader.ReadMaterialIconHash(ElectrificationTID), CargoIconsPath, xmlReader.gameFolderPath), ElectricityTID);
            icons.Add(Electrification);
        }
    }


}
