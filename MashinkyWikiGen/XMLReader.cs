using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace MashinkyWikiGen
{
    public class XMLReader
    {
        private ImageReader imageReader;
        public string gameFolderPath;
        private XMLCorrector xmlCorrector = new XMLCorrector();
        public List<VehicleBase> Wagons { get; private set; } = new List<VehicleBase>();
        public List<VehicleBase> Engines { get; private set; } = new List<VehicleBase>();
        public List<VehicleBase> Cars { get; private set; } = new List<VehicleBase>();
        public List<VehicleBase> Airplanes { get; } = new List<VehicleBase>();
        public List<Building> Buildings { get; private set; } = new List<Building>();
        public List<BuildingUpgrade> TownUpgrades { get; private set; } = new List<BuildingUpgrade>();
        private List<Token> tokens;
        private List<Token> materials;
        private Token invalidToken;
        private XMLTranslator xmlTranslator;
        private List<Token> tokMat;
        private List<string> textmodPaths = new List<string>();



        public XMLReader(List<Token> tokens, List<Token> materials, List<string> textmodPaths)
        {
            File.WriteAllText("traceWikiGen.txt", "\nInitializing xmlReader");
            gameFolderPath = GetGameFolder();
            File.AppendAllText("traceWikiGen.txt", "\ngame folder is " + gameFolderPath);
            imageReader = new ImageReader(gameFolderPath);
            File.AppendAllText("traceWikiGen.txt", "\nInitialized imageReader");
            this.tokens = tokens;
            this.materials = materials;
            invalidToken = new Token("NA", "NA", "NA", imageReader.CreateBlankImage(), "NA");
            xmlTranslator = new XMLTranslator(tokMat, invalidToken);
            this.textmodPaths = textmodPaths;

        }

        public void ConcatTokMat()
        {
            tokMat = tokens.Concat(materials).ToList();
            xmlTranslator.tokMat = tokMat;
        }
        public string GetGameFolder()
        {

            /* try
             {
                 RegistryKey localKey;
                 if (Environment.Is64BitOperatingSystem)
                     localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                 else
                     localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                 // using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Valve\\Steam"))
                 using (RegistryKey registryKey = localKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 598960"))
                 {

                     // string path = (string)registryKey.GetValue("InstallPath");
                     string path = (string)registryKey.GetValue("InstallLocation");
                     return path;
                 }
             }
             catch (Exception)
             {*/
            //  return AppDomain.CurrentDomain.BaseDirectory;
            return Environment.CurrentDirectory;
            //MessageBox.Show("Failed to find game folder");
            //return "";
            // }

        }

        public int[] ReadIconCoords(string path, string ID)
        {
            int scale = 2;
            try
            {
                XDocument data = xmlCorrector.CorrectXmlFile(path);
                IEnumerable<XElement> matchElements = from el in data.Element("root").Elements("Coord")
                                                      where el.Attribute("id").Value == ID
                                                      select el;
                XElement element = matchElements.First();
                int[] coords = new int[4];
                // if (ID == "829B3189")
                coords[0] = int.Parse(element.Attribute("x").Value) * scale;
                coords[1] = int.Parse(element.Attribute("y").Value) * scale;
                coords[2] = int.Parse(element.Attribute("w").Value) * scale;
                coords[3] = int.Parse(element.Attribute("h").Value) * scale;
                return coords;
            }
            catch (Exception)
            {
                int[] coords = new int[4] { 0, 0, 0, 0 };
                return coords;
            }


        }
        /// <summary>
        /// Crops image
        /// </summary>
        /// <param name="iconSource"></param>
        /// <param name="coords">X, Y, weight, height</param>
        /// <returns>Cloned Bitmap within specified coords</returns>
        public Bitmap ReadIcon(string hash, string iconTexturePath, string path)
        {
            Bitmap iconImage = null;
            if (String.IsNullOrEmpty(hash) || String.IsNullOrEmpty(iconTexturePath))
            {
                iconImage = imageReader.CreateBlankImage();
            }
            else
            {
                string pathTcoords = path + "\\media\\config\\tcoords.xml";
                int[] iconCoords = ReadIconCoords(pathTcoords, hash);
                if (iconCoords[2] == 0 || iconCoords[3] == 0)
                    iconImage = imageReader.CreateBlankImage();
                else
                    iconImage = imageReader.ReadIcon(gameFolderPath + "/" + iconTexturePath, iconCoords);
            }
            return iconImage;
        }

        public void ReadWGT(string path)
        {
            string pathXML = path + @"\media\config\wagon_types.xml";
            File.AppendAllText("traceWikiGen.txt", "\nLoading " + pathXML);
            XDocument Data;

            Data = xmlCorrector.CorrectXmlFile(pathXML);
            if (String.IsNullOrEmpty(Data.ToString()))
                return;
            try
            {
                foreach (XElement w in Data.Element("root").Elements("WagonType"))
                {

                    bool validEntry = true;
                    int vehicleType = 0; // 0 - train, 1 - road vehicle
                    string name = "";
                    if (w.Attribute("name") != null)
                        name = w.Attribute("name").Value;
                    else
                        validEntry = false;
                    if (w.Attribute("vehicle_type") != null)
                    {
                        vehicleType = int.Parse(w.Attribute("vehicle_type").Value);
                    }
                    else
                        validEntry = false;
                    string XMLcost = "9999";
                    if (w.Attribute("cost") != null)
                        XMLcost = w.Attribute("cost").Value;
                    string[] costs = XMLcost.Split(';');
                    int cost1 = xmlTranslator.SeparateCost(costs[0]);
                    Token costToken1 = xmlTranslator.AssignToken(xmlTranslator.SeparateType(costs[0]));
                    int cost2 = 0;
                    Token costToken2 = null;
                    if (costs.Length > 1 && costs[1] != "") // multiple costs, treated against lonely ';'
                    {
                        cost2 = xmlTranslator.SeparateCost(costs[1]);
                        costToken2 = xmlTranslator.AssignToken(xmlTranslator.SeparateType(costs[1]));
                    }

                    string iconTexture = "";   // check that XML contains attributes it should to prevent null exceptions
                    if (w.Attribute("icon_texture") != null)
                        iconTexture = w.Attribute("icon_texture").Value;
                    else
                        validEntry = false;
                    string ID = "";
                    if (w.Attribute("id") != null)
                        ID = w.Attribute("id").Value;
                    else
                        validEntry = false;
                    string icon = "";
                    if (w.Attribute("icon") != null)
                        icon = w.Attribute("icon").Value;
                    else
                        validEntry = false;
                    string iconColor = "";
                    if (w.Attribute("icon_color") != null)
                        iconColor = w.Attribute("icon_color").Value;
                    int track = 0;
                    if (w.Attribute("track") != null)
                        track = int.Parse(w.Attribute("track").Value);
                    else if (vehicleType != 3) // airplanes have no tracks yet
                        validEntry = false;
                    int capacity = 0;
                    if (w.Attribute("capacity") != null)
                        capacity = int.Parse(w.Attribute("capacity").Value);
                    Token cargo = invalidToken;
                    if (w.Attribute("cargo") != null)
                        cargo = xmlTranslator.AssignToken(w.Attribute("cargo").Value);
                    int weightFull = 0;
                    if (w.Attribute("weight_full") != null)
                        weightFull = int.Parse(w.Attribute("weight_full").Value);
                    else
                        validEntry = false;
                    int startEpoch = 0;
                    int endEpoch = 0;
                    if (w.Attribute("epoch") != null) //epoch in game files is in range format, e.g. 1-3
                    {
                        startEpoch = int.Parse(w.Attribute("epoch").Value.First().ToString());
                        endEpoch = int.Parse(w.Attribute("epoch").Value.Last().ToString());
                    }
                    else
                        validEntry = false;
                    bool reqDepoUpgrade;
                    if (w.Attribute("depo_upgrade") != null)
                        reqDepoUpgrade = true;
                    else
                        reqDepoUpgrade = false;


                    int maxSpeed = 0;
                    if (w.Attribute("max_speed") != null)
                        maxSpeed = int.Parse(w.Attribute("max_speed").Value);
                    int power = 0;
                    if (w.Attribute("power") != null)
                        power = int.Parse(w.Attribute("power").Value);

                    double length = 0;
                    if (w.Attribute("length") != null)
                        length = double.Parse(w.Attribute("length").Value, System.Globalization.CultureInfo.InvariantCulture);
                    else
                        validEntry = false;
                    if (w.Attribute("tail_length") != null)
                        length += double.Parse(w.Attribute("tail_length").Value, System.Globalization.CultureInfo.InvariantCulture);
                    if (w.Attribute("head_length") != null)
                        length += double.Parse(w.Attribute("head_length").Value, System.Globalization.CultureInfo.InvariantCulture);

                    Bitmap iconImage;

                    if (w.Attribute("power") == null && vehicleType == 0 && validEntry /*&& capacity > 0*/) // vehicle_type="0" = train, no power = wagon
                    {
                        iconImage = ReadIcon(icon, iconTexture, path);
                        Wagons.Add(new VehicleBase(name, cargo, capacity, cost1, costToken1, cost2, costToken2, weightFull, length, reqDepoUpgrade, startEpoch, endEpoch, iconImage, 0)); // improvement - use Builder pattern
                    }
                    else if ((w.Attribute("power") != null || vehicleType == 3) && validEntry) //is engine
                    {
                        iconImage = ReadIcon(icon, iconTexture, path);
                        // File.AppendAllText("traceWikiGen.txt", "\nLoading engine" + name);
                        string XMLfuel = w.Attribute("fuel_cost").Value;
                        string[] fuels = XMLfuel.Split(';');
                        int fuelAmount1 = xmlTranslator.SeparateCost(fuels[0]);
                        Token fuelToken1 = xmlTranslator.AssignToken(xmlTranslator.SeparateType(fuels[0]));
                        int fuelAmount2 = 0;
                        Token fuelType2 = null;
                        if (fuels.Length > 1)
                        {
                            fuelAmount2 = xmlTranslator.SeparateCost(fuels[1]);
                            fuelType2 = xmlTranslator.AssignToken(xmlTranslator.SeparateType(fuels[1]));
                        }
                        int trackType = 0;
                        if (w.Attribute("track") != null)
                            trackType = int.Parse(w.Attribute("track").Value);
                        string soundWhistle = "";
                        if (w.Attribute("sound_tunel") != null)
                            soundWhistle = w.Attribute("sound_tunel").Value;
                        string soundStandby = "";
                        if (w.Attribute("sound_engine_standby") != null)
                            soundStandby = w.Attribute("sound_engine_standby").Value;
                        string soundStart = "";
                        if (w.Attribute("sound_engine_start") != null)
                            soundStart = w.Attribute("sound_engine_start").Value;
                        string soundSlow = "";
                        if (w.Attribute("sound_engine_slow") != null)
                            soundSlow = w.Attribute("sound_engine_slow").Value;
                        string soundMedium = "";
                        if (w.Attribute("sound_engine_medium") != null)
                            soundMedium = w.Attribute("sound_engine_medium").Value;
                        string soundFast = "";
                        if (w.Attribute("sound_engine_fast") != null)
                            soundFast = w.Attribute("sound_engine_fast").Value;
                        string soundBrakes = "";
                        if (w.Attribute("sound_breaks") != null)
                            soundBrakes = w.Attribute("sound_breaks").Value;
                        if (vehicleType == 0)
                            Engines.Add(new Engine(trackType, soundWhistle, soundStandby, soundSlow, soundStart, soundMedium, soundFast, soundBrakes, power, maxSpeed, fuelAmount1, fuelToken1, fuelAmount2, fuelType2, name, cargo, capacity, cost1, costToken1, cost2, costToken2, weightFull, length, reqDepoUpgrade, startEpoch, endEpoch, iconImage, 0)); // improvement - use Builder pattern
                        else if (vehicleType == 1)
                            Cars.Add(new Engine(trackType, soundWhistle, soundStandby, soundSlow, soundStart, soundMedium, soundFast, soundBrakes, power, maxSpeed, fuelAmount1, fuelToken1, fuelAmount2, fuelType2, name, cargo, capacity, cost1, costToken1, cost2, costToken2, weightFull, length, reqDepoUpgrade, startEpoch, endEpoch, iconImage, 1));
                        else if (vehicleType == 3)
                        {
                            // airplane
                            int runwayLength = w.Attribute("runway_length_needed") != null ? int.Parse(w.Attribute("runway_length_needed").Value) : 0;
                            int flightAltitude = w.Attribute("flight_altitude") != null ? int.Parse(w.Attribute("flight_altitude").Value) : 0;
                            Airplanes.Add(new Airplane(runwayLength, flightAltitude, soundWhistle, soundStandby, soundSlow, soundStart, soundMedium, soundFast, soundBrakes, power, maxSpeed, fuelAmount1, fuelToken1, fuelAmount2, fuelType2, name, cargo, capacity, cost1, costToken1, cost2, costToken2, weightFull, length, reqDepoUpgrade, startEpoch, endEpoch, iconImage, 3));
                        }
                    }
                    //File.AppendAllText("traceWikiGen.txt", name);
                }
                //File.AppendAllText("traceWikiGen.txt", "\nFile loaded");
            }
            catch
            {
                //File.AppendAllText(Settings.Path + @"\InvalidXMLContent.xml", Data.ToString() + "\n" + e.Message + "\n\n");

                throw;
            }
        }

        public void ReadBuildings(string path)
        {
            string pathXML = path + @"\media\config\building_types.xml";
            File.AppendAllText("traceWikiGen.txt", "\nLoading " + pathXML);
            XDocument Data;
            Data = xmlCorrector.CorrectBuildingsXmlFile(pathXML);

            try
            {
                foreach (XElement b in Data.Element("root").Elements("BuildingType"))
                {
                    List<IndustryBuildingUpgrade> upgrades = new List<IndustryBuildingUpgrade>();
                    List<ProductionRule> rules = new List<ProductionRule>();
                    string ID = "";
                    if (b.Attribute("id") != null)
                        ID = b.Attribute("id").Value;
                    if (ID == "27D6C338")
                    {

                    }
                    string name = "";
                    if (b.Attribute("nameId") != null)
                        name = ReadTextFromHash(b.Attribute("nameId").Value);                    
                    Bitmap icon = invalidToken.Icon;
                    string cargoSign = "";
                    cargoSign = b.Attribute("cargo_sign").Value;
                    
                    int[] DimXY = new int[2];
                    if (b.Attribute("disposition") != null)
                        DimXY = xmlTranslator.ConvDispToDim(b.Attribute("disposition").Value);
                    int[] epochs = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
                    if (b.Attribute("launches_per_epocha") != null)
                        epochs = xmlTranslator.ConvLaunchesPerEpoch(b.Attribute("launches_per_epocha").Value);
                    int bonusDistance = 0;
                    if (b.Attribute("bonus_distance") != null)
                        bonusDistance = int.Parse(b.Attribute("bonus_distance").Value);
                    int pollution = 0;
                    if (b.Attribute("flags") != null && b.Attribute("flags").Value.Contains("town_industry"))
                        pollution = xmlTranslator.GetFlagValue(b.Attribute("flags").Value, "town_industry");
                    int electricity = 0;
                    if (b.Attribute("flags") != null && b.Attribute("flags").Value.Contains("generates_electricity"))
                        electricity = xmlTranslator.GetFlagValue(b.Attribute("flags").Value, "generates_electricity");


                    foreach (XElement r in b.Elements("Rule"))
                    {
                        string input = "";
                        if (r.Attribute("input") != null)
                            input = r.Attribute("input").Value;
                        string inputEl = "";
                        if (r.Attribute("input_electrification_bonus") != null)
                            inputEl = r.Attribute("input_electrification_bonus").Value;
                        string output = "";
                        if (r.Attribute("output") != null)
                            output = r.Attribute("output").Value;
                        string outputEl = "";
                        if (r.Attribute("output_electrification_bonus") != null)
                            outputEl = r.Attribute("output_electrification_bonus").Value;
                        int reqUpgrade = 100; // 100 default, because 0 is valid upgrade
                        if (r.Attribute("require_upgrade") != null)
                            reqUpgrade = int.Parse(r.Attribute("require_upgrade").Value);
                        int timeSteps = 1;
                        if (r.Attribute("time_steps") != null && !String.IsNullOrEmpty(r.Attribute("time_steps").Value))
                            timeSteps = int.Parse(r.Attribute("time_steps").Value);
                        string outputDB = "";
                        if (r.Attribute("output_distance_bonus") != null)
                            outputDB = r.Attribute("output_distance_bonus").Value;


                        rules.Add(xmlTranslator.CreateRule(input, inputEl, output, outputEl, reqUpgrade, timeSteps, outputDB));
                    }

                    foreach (XElement u in b.Elements("Upgrade"))
                    {
                        int id = 100;
                        if (u.Attribute("id") != null)
                            id = int.Parse(u.Attribute("id").Value);
                        string uName = "";
                        if (u.Attribute("name") != null)
                        {
                            if (u.Attribute("name").Value == "91E5B675")
                            {

                            }
                            uName = ReadTextFromHash(u.Attribute("name").Value);
                        }
                        string desc = "";
                        if (u.Attribute("description") != null)
                            desc = ReadTextFromHash(u.Attribute("description").Value);
                        int[] uDimXY = new int[2];
                        if (u.Attribute("disposition") != null)
                            uDimXY = xmlTranslator.ConvDispToDim(u.Attribute("disposition").Value);
                        string effect = "";
                        if (u.Attribute("effect") != null)
                            effect = u.Attribute("effect").Value;
                        int reqEpoch = 0;
                        if (u.Attribute("require_epocha") != null)
                            reqEpoch = int.Parse(u.Attribute("require_epocha").Value);
                        string cost = "";
                        int cost1C = 0;
                        Token cost1T = invalidToken;
                        int cost2C = 0;
                        Token cost2T = invalidToken;
                        int cost3C = 0;
                        Token cost3T = invalidToken;
                        if (u.Attribute("cost") != null)
                        {
                            cost = u.Attribute("cost").Value;
                            string[] costSplit = cost.Split(';');
                            cost1C = xmlTranslator.SeparateCost(costSplit[0]);
                            cost1T = xmlTranslator.AssignToken(xmlTranslator.SeparateType(costSplit[0]));
                            if (costSplit.Length > 1)
                            {
                                cost2C = xmlTranslator.SeparateCost(costSplit[1]);
                                cost2T = xmlTranslator.AssignToken(xmlTranslator.SeparateType(costSplit[1]));
                            }
                            if (costSplit.Length > 2)
                            {
                                cost3C = xmlTranslator.SeparateCost(costSplit[2]);
                                cost3T = xmlTranslator.AssignToken(xmlTranslator.SeparateType(costSplit[2]));
                            }
                        }
                        if (cost1C == 0)
                        {
                            uName = "Quest or reserve";
                        }
                        List<ProductionRule> upgradeRules = (from r in rules                     //find rules using this upgrade, always should be one only
                                                             where (r.ReqUpgrade == id)
                                                             select r).ToList();
                        if (electricity == 0)
                            upgrades.Add(new IndustryBuildingUpgrade(upgradeRules, desc, reqEpoch, uName, uDimXY[0], uDimXY[1], cost1C, cost1T, cost2C, cost2T, cost3C, cost3T));
                        else
                        {
                            int el = xmlTranslator.GetFlagValue(u.Attribute("effect").Value, "electricity");
                            upgrades.Add(new PowerPlantUpgrade(effect, el, upgradeRules, desc, reqEpoch, uName, uDimXY[0], uDimXY[1], cost1C, cost1T, cost2C, cost2T, cost3C, cost3T));
                        }
                    }

                    if (electricity == 0)
                    {
                        if (cargoSign == "EEFDFC03" || cargoSign == "EEFDFC02" || cargoSign == "EEFDFC01") //special cargo for town upgrades
                        {
                            string cost = "";
                            int cost1C = 0;
                            Token cost1T = invalidToken;
                            int cost2C = 0;
                            Token cost2T = invalidToken;
                            int cost3C = 0;
                            Token cost3T = invalidToken;
                            if (b.Attribute("cost") != null)
                            {
                                cost = b.Attribute("cost").Value;
                                string[] costSplit = cost.Split(';');
                                cost1C = xmlTranslator.SeparateCost(costSplit[0]);
                                cost1T = xmlTranslator.AssignToken(xmlTranslator.SeparateType(costSplit[0]));
                                if (costSplit.Length > 1)
                                {
                                    cost2C = xmlTranslator.SeparateCost(costSplit[1]);
                                    cost2T = xmlTranslator.AssignToken(xmlTranslator.SeparateType(costSplit[1]));
                                }
                                if (costSplit.Length > 2)
                                {
                                    cost3C = xmlTranslator.SeparateCost(costSplit[2]);
                                    cost3T = xmlTranslator.AssignToken(xmlTranslator.SeparateType(costSplit[2]));
                                }
                                int relax = 0;
                                int amenities = 0;
                                int luxury = 0;
                                int epoch = 0;
                                if (b.Attribute("flags") != null)
                                {
                                    relax = xmlTranslator.GetFlagValue(b.Attribute("flags").Value, "town_relax");
                                    amenities = xmlTranslator.GetFlagValue(b.Attribute("flags").Value, "town_amenities");
                                    luxury = xmlTranslator.GetFlagValue(b.Attribute("flags").Value, "town_luxury");
                                }
                                if (b.Attribute("require_epocha") != null)
                                    epoch = int.Parse(b.Attribute("require_epocha").Value);
                                if (relax + amenities + luxury > 0)
                                {

                                    TownUpgrades.Add(new TownExtension(relax, amenities, luxury, epoch, name, DimXY[0], DimXY[1], cost1C, cost1T, cost2C, cost2T, cost3C, cost3T));
                                    File.AppendAllText("traceWikiGen.txt", $"\nLoaded town extension {name}");
                                }
                                else
                                {
                                    TownUpgrades.Add(new TownDecoration(epoch, name, DimXY[0], DimXY[1], cost1C, cost1T, cost2C, cost2T, cost3C, cost3T));
                                    File.AppendAllText("traceWikiGen.txt", $"\nLoaded town decoration {name}");
                                }
                            }


                        }
                        else
                        {
                            icon = ReadIcon(ReadMaterialIconHash(cargoSign), "/media/map/gui/cargo_basic_set.png", path);
                            List<ProductionRule> baseRules = (from r in rules
                                                              where (r.ReqUpgrade == 100)
                                                              select r).ToList();
                            Buildings.Add(new IndustryBuilding(upgrades, baseRules, icon, bonusDistance, pollution, ID, name, DimXY[0], DimXY[1], epochs));
                            File.AppendAllText("traceWikiGen.txt", $"\nLoaded building {name}");
                        }
                    }
                    else
                    {
                        icon = ReadIcon(ReadMaterialIconHash(cargoSign), "/media/map/gui/cargo_basic_set.png", path);
                        List<ProductionRule> baseRules = (from r in rules
                                                          where (r.ReqUpgrade == 100)
                                                          select r).ToList();
                        Buildings.Add(new PowerPlant(electricity, upgrades, baseRules, icon, bonusDistance, pollution, ID, name, DimXY[0], DimXY[1], epochs));
                        File.AppendAllText("traceWikiGen.txt", $"\nLoaded power plant {name}");
                    }
                }
            }
            catch
            {
                throw;
            }


        }

        public string ReadTokenIconHash(string ID)
        {
            string pathXML = gameFolderPath + @"\media\config\cargo_types.xml";
            XDocument Data;
            Data = xmlCorrector.CorrectXmlFile(pathXML);
            string hash = "";
            foreach (XElement e in Data.Element("root").Elements("TokenType"))
            {
                if (e.Attribute("icon") != null && e.Attribute("id").Value == ID)
                {
                    hash = e.Attribute("icon").Value;
                    return hash;
                }
            }
            return hash;
        }
        public string ReadMaterialIconHash(string ID)
        {
            string pathXML = gameFolderPath + @"\media\config\cargo_types.xml";
            XDocument Data;
            Data = xmlCorrector.CorrectXmlFile(pathXML);
            string hash = "";
            foreach (XElement e in Data.Element("root").Elements("CargoType"))
            {
                if (e.Attribute("icon") != null && e.Attribute("id").Value == ID)
                    return e.Attribute("icon").Value;

            }
            return hash;
        }
        /// <summary>
        /// reads english texts from texts.xml
        /// </summary>
        /// <param name="hash"></param>
        /// <returns>text for specified hash</returns>
        public string ReadTextFromHash(string hash)
        {
            string pathXML = gameFolderPath + @"\media\config\texts.xml";
            XDocument Data;
            Data = xmlCorrector.CorrectXmlFile(pathXML);
            XElement engTexts = (from e in Data.Element("root").Elements("resources")
                                 where e.Attribute("caption").Value == "English"
                                 select e).First();
            var texts = from e in engTexts.Elements("string")
                        where e.Attribute("name").Value.ToUpper() == hash.ToUpper()
                        select e.Value;
            string text = "NA";
            if (!texts.Any())
            {
                foreach (string path in textmodPaths)
                {
                    pathXML = path + @"\media\config\texts.xml";
                    Data = xmlCorrector.CorrectXmlFile(pathXML);
                    XElement engText = (from e in Data.Element("root").Elements("resources")
                                        where e.Attribute("caption").Value == "English"
                                        select e).First();
                    var matches = from e in engText.Elements("string")
                                  where e.Attribute("name").Value.ToUpper() == hash.ToUpper()
                                  select e.Value;
                    if (matches.Any())
                    {
                        text = matches.First();
                        break;
                    }
                }
            }
            else
                text = texts.First();
            text = text.Replace("\n", " ");
            text = text.Replace("\"", "");
            return text;
        }


    }
}
