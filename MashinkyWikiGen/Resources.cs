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
    public class Resources
    {
        public Token MoneyToken { get; set; }
        public Token TimberToken { get; set; }
        public Token CoalToken { get; set; }
        public Token IronToken { get; set; }
        public Token DieselToken { get; set; }
        public Token SteelToken { get; set; }
        public Token ElectricityToken { get; set; }
        public Token CementToken { get; set; }
        public Token Reserve1Token { get; set; }
        public Token Reserve2Token { get; set; }
        public Token LogMaterial { get; set; }
        public Token CoalMaterial { get; set; }
        public Token IronOreMaterial { get; set; }
        public Token CrudeOilMaterial { get; set; }
        public Token SandMaterial { get; set; }
        public Token TimberMaterial { get; set; }
        public Token IronMaterial { get; set; }
        public Token GoodsMaterial { get; set; }
        public Token FoodMaterial { get; set; }
        public Token DieselMaterial { get; set; }
        public Token SteelMaterial { get; set; }
        public Token LimestoneMaterial { get; set; }
        public Token CementMaterial { get; set; }
        public Token PassengersMaterial { get; set; }
        public Token MailMaterial { get; set; }
        public Token GarbageMaterial { get; set; }
        public Token Reserve2Material { get; set; }

        private XMLReader xmlReader;
        private DataProcessor dataProcessor;
        private List<Token> tokens = new List<Token>();
        private List<Token> materials = new List<Token>();
        private List<string> WGTmodPaths = new List<string>();
        private List<string> textmodPaths = new List<string>();
        private List<Token> icons = new List<Token>();
        public string MoneyTID { get; set; } = "F0000000";
        public string TimberTID { get; set; } = "F07C5273";
        public string CoalTID { get; set; } = "F1283C03";
        public string IronTID { get; set; } = "F27DB683";
        public string DieselTID { get; set; } = "F29BF6C1";
        public string SteelTID { get; set; } = "F8E3D3EC";
        public string ElectricityTID { get; set; } = "F9D8BF64";
        public string CementTID { get; set; } = "FAD8464A";
        public string Reserve1TID { get; set; } = "";
        public string Reserve2TID { get; set; } = "";
        public string LogMID { get; set; } = "3199AA74";
        public string TimberMID { get; set; } = "448DDF23";
        public string CoalMID { get; set; } = "6ACBCBA9";
        public string IronOreMID { get; set; } = "61A13BCE";
        public string CrudeOilMID { get; set; } = "19CFBDA7";
        public string SandMID { get; set; } = "94032E35";
        public string IronMID { get; set; } = "7C13D1C9";
        public string GoodsMID { get; set; } = "B388ED8C";
        public string FoodMID { get; set; } = "C74198A7";
        public string DieselMID { get; set; } = "53F1B093";
        public string SteelMID { get; set; } = "88D1A491";
        public string PassengersMID { get; set; } = "0BA458C8";
        public string MailMID { get; set; } = "0F822763";
        public string LimestoneMID { get; set; } = "762F8F3E";
        public string CementMID { get; set; } = "8DFA75B5";
        public string GarbageMID { get; set; } = "9A988702";
        public string Reserve2MID { get; set; } = "";
        public string CargoIconsPath { get; set; } = "/media/map/gui/cargo_basic_set.png";
        public string LinkPollution { get; set; } = "City Upgrades#Pollution";
        public Token Electrification { get; private set; }
        public string ElectrificationTID { get; set; } = "CE900010";
        public string LinkTick { get; set; } = "Game_tick";
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
