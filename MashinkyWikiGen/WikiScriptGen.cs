using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Generates MediaWiki-formatted tables and markup for game entities.
    /// Handles creation of wikitables for vehicles, buildings, and upgrades with proper formatting and links.
    /// </summary>
    public class WikiScriptGen
    {
        /// <summary>
        /// MediaWiki line break markup.
        /// </summary>
        const string newLine = "<br/>";
        
        /// <summary>
        /// MediaWiki table start markup with wikitable styling.
        /// </summary>
        const string startTable = "{| class=\"wikitable transformable \" style=text-align:center";
        
        /// <summary>
        /// MediaWiki table end markup.
        /// </summary>
        const string endTable = "\n|}";
        
        /// <summary>
        /// MediaWiki table row separator.
        /// </summary>
        const string newRow = "\n|-";
        
        /// <summary>
        /// MediaWiki table row separator with indentation for nested tables.
        /// </summary>
        const string newRowTab = "\n   |-";
        
        /// <summary>
        /// MediaWiki table column separator.
        /// </summary>
        const string newColumn = "\n|";
        
        /// <summary>
        /// MediaWiki table column separator with indentation for nested tables.
        /// </summary>
        const string newColumnTab = "\n   |";
        
        /// <summary>
        /// Standard indentation string.
        /// </summary>
        const string tab = "   ";
        
        /// <summary>
        /// Double indentation string.
        /// </summary>
        const string doubleTab = "      ";
        
        /// <summary>
        /// The resources manager providing access to tokens and links.
        /// </summary>
        private Resources resources;


        /// <summary>
        /// Initializes a new instance of the WikiScriptGen class.
        /// </summary>
        /// <param name="resources">The resources manager providing access to tokens and links</param>
        public WikiScriptGen(Resources resources)
        {
            this.resources = resources;
        }

        /// <summary>
        /// Creates a multi-row table for a list of vehicles of the same type.
        /// Generates appropriate headers and formatting based on vehicle type.
        /// </summary>
        /// <param name="path">The output path where the table file will be saved</param>
        /// <param name="w">The list of vehicles to include in the table</param>
        public void CreateMultiTable(string path, List<VehicleBase> w)
        {
            if (!w.Any())
                return;
            string header = "";
            bool electric = false;
            if (w.First() is Engine && w.First().VehicleType == 0) // engine
            {
                foreach (Engine e in w)
                {
                    if (e.StartEpoch >= 5)
                        electric = true;
                    break;
                }
                if (electric)
                {
                    header = engineTHeader + $"\n!Electrification{newLine}required?";
                }
                else
                    header = engineTHeader;
                string print = $"{startTable}{header}";
                foreach (Engine e in w)
                {
                    print += CreateEngineTable(e);
                }
                print += endTable;

                File.WriteAllText(path + "parameters table.txt", print);
            }

            else if (w.First() is Engine && w.First().VehicleType == 1) // vehicle
            {
                string print = $"{startTable}{vehicleHeader}";
                foreach (Engine v in w)
                {
                    print += CreateVehicleTable(v);
                }
                print += endTable;
                File.WriteAllText(path + "parameters table.txt", print);
            }
            else if (w.First() is Airplane)
            {
                string print = $"{startTable}{airplaneHeader}";
                foreach (Airplane airplane in w)
                {
                    print += CreateVehicleTable(airplane);
                }
                print += endTable;
                File.WriteAllText(path + "parameters table.txt", print);
            }

            else // wagon
            {
                string print = $"{startTable}{wagonHeader}";
                foreach (VehicleBase wagon in w)
                {
                    print += CreateWagonTable(wagon);
                }
                print += endTable;
                File.WriteAllText(path + "parameters table.txt", print);
            }


        }

        /// <summary>
        /// Creates a single-row table for one vehicle with appropriate headers based on vehicle type.
        /// Generates different table formats for engines, vehicles, airplanes, and wagons.
        /// </summary>
        /// <param name="path">The output path where the table file will be saved</param>
        /// <param name="w">The vehicle to create a table for</param>
        public void CreateSingleTable(string path, VehicleBase w)
        {
            string header = "";
            if (w is Engine && w.VehicleType == 0)
            {
                if (w.StartEpoch >= 5)
                    header = engineTHeader + $"\n!Electrification{newLine}required?";
                else
                    header = engineTHeader;
                File.WriteAllText(path + "parameters table.txt", $"{startTable}{header}{CreateEngineTable(w as Engine)}{endTable}");
            }
            else if (w is Engine && w.VehicleType == 1) // vehicle
            {
                File.WriteAllText(path + "parameters table.txt", $"{startTable}{vehicleHeader}{CreateVehicleTable(w as Engine)}{endTable}");
            }
            else if (w is Airplane)
            {
                File.WriteAllText(path + "parameters table.txt", $"{startTable}{vehicleHeader}{CreateVehicleTable(w as Airplane)}{endTable}");
            }

            else // wagon
            {
                File.WriteAllText(path + "parameters table.txt", $"{startTable}{wagonHeader}{CreateWagonTable(w)}{endTable}");
            }


        }
        private string wagonHeader = $"\n!Image\n!Name\n!Capacity\n!Purchase{newLine}cost\n!Weight{newLine}(Fully loaded)\n!Unlocked{newLine}at epoch\n!Depot extension{newLine}required?";
        /// <summary>
        /// Creates a MediaWiki table row for a wagon vehicle.
        /// Includes image, name, capacity, purchase cost, weight, epoch, and depot extension requirement.
        /// </summary>
        /// <param name="w">The wagon vehicle to create a table row for</param>
        /// <returns>A MediaWiki-formatted table row string</returns>
        public string CreateWagonTable(VehicleBase w)
        {
            return $"{newRow}{newColumn} {PrintImage(w.Name, 120)}{newColumn} {w.Name}{newLine}{newLine} {PrintIcon(w.Name, 80)}{newColumn} {w.Capacity} {PrintToken(w.Cargo.IconPath, 16, w.Cargo.LinkedPage)} {newColumn} {PrintPurchaseCost(w)}{newColumn} {w.Weight}" +
                $"{newColumn} {w.StartEpoch}{newColumn} {PrintBool(w.ReqDepotExtension)}";
        }
        private string engineTHeader;
        //private string engineTHeader = $"\n!Image\n!Name\n!Max speed\n!Pull up to\n!Power\n!Weight\n!Length\n!Purchase{newLine}cost\n!Operating{newLine}cost\n!Unlocked{newLine}at epoch\n!Depot extension{newLine}required?";
        /// <summary>
        /// Creates a MediaWiki table row for an engine vehicle.
        /// Includes image, specifications, costs, and electrification requirements.
        /// </summary>
        /// <param name="e">The engine to create a table row for</param>
        /// <returns>A MediaWiki-formatted table row string</returns>
        private string CreateEngineTable(Engine e)
        {
            engineTHeader = $"\n!Image\n!Name\n!Max speed\n!Pull up to\n!Power\n!Weight\n!Length\n!Purchase{newLine}cost\n!{LinkText($"Operating{newLine}cost", resources.LinkOperatingCost)}\n!Depot extension{newLine}required?";
            string elColumn = "";

            if (e.TrackType == 2)
                elColumn = "\n| Yes";
            else if (e.StartEpoch >= 5)
                elColumn = "\n| No";

            /* return $"{newRow}{newColumn} {PrintImage(e.Name, 120)}{newColumn} {e.Name}{newLine}{newLine}{PrintIcon(e.Name, 80)}{newColumn} {e.MaxSpeed} kph ({e.MaxSpeedMiles} mph)" +
                 $"{newColumn} {e.Pull} t{newColumn} {e.Power} hp{newColumn} {e.Weight} t{newColumn} {e.Length}{newColumn} {PrintPurchaseCost(e)}{newColumn} {PrintOperatingCost(e)}" +
                 $"{newColumn}{e.StartEpoch}{newColumn} {PrintBool(e.ReqDepotExtension)}{elColumn}";*/

            return $"{newRow}{newColumn} {PrintImage(e.Name, 120)}{newColumn} {e.Name}{newLine}{newLine}{PrintIcon(e.Name, 80)}{newColumn} {e.MaxSpeed} km/h{newLine}={newLine}{e.MaxSpeedMiles} mph" +
                 $"{newColumn} {e.Pull} t{newColumn} {e.Power} hp{newColumn} {e.Weight} t{newColumn} {e.Length}{newColumn} {PrintPurchaseCost(e)}{newColumn} {PrintOperatingCost(e)}" +
                 $"{newColumn} {PrintBool(e.ReqDepotExtension)}{elColumn}";
        }
        private string vehicleHeader = "";
        /// <summary>
        /// Creates a MediaWiki table row for a road vehicle (car).
        /// Includes image, specifications, capacity, and operating costs.
        /// </summary>
        /// <param name="v">The vehicle to create a table row for</param>
        /// <returns>A MediaWiki-formatted table row string</returns>
        private string CreateVehicleTable(Engine v)
        {
            vehicleHeader = $"\n!Image\n!Name\n!Max speed\n!Power\n!Weight\n!Length\n!Capacity\n!Purchase{newLine}cost\n!{LinkText($"Operating{newLine}cost", resources.LinkOperatingCost)} \n!Unlocked{newLine}at epoch";
            return $"{newRow}{newColumn} {PrintImage(v.Name, 120)}{newColumn} {v.Name}{newLine}{newLine} {PrintIcon(v.Name, 80)}{newColumn} {v.MaxSpeed} km/h{newLine}={newLine}{v.MaxSpeedMiles} mph" +
                $"{newColumn} {v.Power} hp{newColumn} {v.Weight} t{newColumn} {v.Length}{newColumn} {v.Capacity} {PrintToken(v.Cargo.IconPath, 16, v.Cargo.LinkedPage)} {newColumn} {PrintPurchaseCost(v)}{newColumn} {PrintOperatingCost(v)}{newColumn}{v.StartEpoch}" +
                $"";
        }

        private string airplaneHeader = "";
        /// <summary>
        /// Creates a MediaWiki table row for an airplane vehicle.
        /// Includes image, specifications, runway requirements, and flight altitude.
        /// </summary>
        /// <param name="airplane">The airplane to create a table row for</param>
        /// <returns>A MediaWiki-formatted table row string</returns>
        private string CreateVehicleTable(Airplane airplane)
        {
            airplaneHeader = $"\n!Image\n!Name\n!Max speed\n!Power\n!Weight\n!Length\n!Runway{newLine}length\n!Flight{newLine}altitude\n!Capacity\n!Purchase{newLine}cost\n!{LinkText($"Operating{newLine}cost", resources.LinkOperatingCost)} \n!Unlocked{newLine}at epoch";
            return $"{newRow}{newColumn} {PrintImage(airplane.Name, 120)}{newColumn} {airplane.Name}{newLine}{newLine} {PrintIcon(airplane.Name, 80)}{newColumn} {airplane.MaxSpeed} km/h{newLine}={newLine}{airplane.MaxSpeedMiles} mph" +
                $"{newColumn} {airplane.Power} hp{newColumn} {airplane.Weight} t{newColumn} {airplane.Length} {newColumn} {airplane.RunwayLength} {newColumn} {airplane.FlightAltitude} {newColumn} {airplane.Capacity} {PrintToken(airplane.Cargo.IconPath, 16, airplane.Cargo.LinkedPage)} {newColumn} {PrintPurchaseCost(airplane)}{newColumn} {PrintOperatingCost(airplane)}{newColumn}{airplane.StartEpoch}" +
                $"";
        }

        /// <summary>
        /// Creates a MediaWiki image link with specified size.
        /// Replaces spaces with underscores in the filename.
        /// </summary>
        /// <param name="name">The image name</param>
        /// <param name="size">The image height in pixels</param>
        /// <returns>A MediaWiki image link with size specification</returns>
        private string PrintImage(string name, int size)
        {
            name = name.Replace(" ", "_");
            return $"[[File:{name}.png|x{size}px]]";
        }

        /// <summary>
        /// Creates a MediaWiki image link without size specification.
        /// Replaces spaces with underscores in the filename.
        /// </summary>
        /// <param name="name">The image name</param>
        /// <returns>A MediaWiki image link</returns>
        private string PrintImage(string name)
        {
            name = name.Replace(" ", "_");
            return $"[[File:{name}.png]]";
        }
        /// <summary>
        /// Creates a MediaWiki token/icon link.
        /// </summary>
        /// <param name="path">The path to the token icon file</param>
        /// <returns>A MediaWiki file link</returns>
        private string PrintToken(string path)
        {
            return $"[[File:{path}]]";
        }

        /// <summary>
        /// Creates a MediaWiki token/icon link with size and hyperlink.
        /// </summary>
        /// <param name="path">The path to the token icon file</param>
        /// <param name="size">The icon size in pixels</param>
        /// <param name="link">The target page for the hyperlink</param>
        /// <returns>A MediaWiki file link with size and hyperlink</returns>
        private string PrintToken(string path, int size, string link)
        {
            return $"[[File:{path}|{size}px|link={link}]]";
        }

        /// <summary>
        /// Creates a MediaWiki token/icon link using a Token object.
        /// </summary>
        /// <param name="t">The token object containing icon path and linked page</param>
        /// <param name="size">The icon size in pixels</param>
        /// <returns>A MediaWiki file link with size and hyperlink</returns>
        private string PrintToken(Token t, int size)
        {
            return $"[[File:{t.IconPath}|{size}px|link={t.LinkedPage}]]";
        }

        /// <summary>
        /// Creates a MediaWiki icon link for vehicle/building icons.
        /// Replaces spaces with underscores and adds '_icon' suffix.
        /// </summary>
        /// <param name="name">The name of the item</param>
        /// <returns>A MediaWiki icon file link</returns>
        private string PrintIcon(string name)
        {
            name = name.Replace(" ", "_");
            return $"[[File:{name}_icon.png]]";
        }
        /// <summary>
        /// Creates a MediaWiki icon link with specified size for vehicle/building icons.
        /// Replaces spaces with underscores and adds '_icon' suffix.
        /// </summary>
        /// <param name="name">The name of the item</param>
        /// <param name="size">The icon height in pixels</param>
        /// <returns>A MediaWiki icon file link with size specification</returns>
        private string PrintIcon(string name, int size)
        {
            name = name.Replace(" ", "_");
            return $"[[File:{name}_icon.png|x{size}px]]";
        }

        /// <summary>
        /// Formats the purchase cost for a vehicle as MediaWiki markup.
        /// Handles single cost, dual cost, and quest reward scenarios.
        /// </summary>
        /// <param name="w">The vehicle to format the purchase cost for</param>
        /// <returns>A formatted cost string with token icons</returns>
        private string PrintPurchaseCost(VehicleBase w)
        {
            if (w.CostAmount1 == 0)
                return "Quest reward";
            else if (w.CostAmount2 == 0)
                return $"{w.CostAmount1 * -1} {PrintToken(w.CostType1.IconPath, 16, w.CostType1.LinkedPage)}";
            else
                return $"{w.CostAmount1 * -1} {PrintToken(w.CostType1.IconPath, 16, w.CostType1.LinkedPage)}{newLine}+{newLine}{w.CostAmount2 * -1} {PrintToken(w.CostType2.IconPath, 16, w.CostType2.LinkedPage)}";
        }

        /// <summary>
        /// Formats the operating cost (fuel consumption) for an engine as MediaWiki markup.
        /// Handles single fuel type and dual fuel type scenarios.
        /// </summary>
        /// <param name="e">The engine to format the operating cost for</param>
        /// <returns>A formatted fuel cost string with token icons</returns>
        private string PrintOperatingCost(Engine e)
        {
            if (e.FuelAmount2 == 0)
                return $"{e.FuelAmount1 * -1} {PrintToken(e.FuelType1.IconPath, 16, e.FuelType1.LinkedPage)}";
            else
                return $"{e.FuelAmount1 * -1} {PrintToken(e.FuelType1.IconPath, 16, e.FuelType1.LinkedPage)}{newLine}+{newLine}{e.FuelAmount2 * -1} {PrintToken(e.FuelType2.IconPath, 16, e.FuelType2.LinkedPage)}";
        }

        /// <summary>
        /// Formats the operating cost (fuel consumption) for an airplane as MediaWiki markup.
        /// Handles single fuel type and dual fuel type scenarios.
        /// </summary>
        /// <param name="airplane">The airplane to format the operating cost for</param>
        /// <returns>A formatted fuel cost string with token icons</returns>
        private string PrintOperatingCost(Airplane airplane)
        {
            if (airplane.FuelAmount2 == 0)
                return $"{airplane.FuelAmount1 * -1} {PrintToken(airplane.FuelType1.IconPath, 16, airplane.FuelType1.LinkedPage)}";
            else
                return $"{airplane.FuelAmount1 * -1} {PrintToken(airplane.FuelType1.IconPath, 16, airplane.FuelType1.LinkedPage)}{newLine}+{newLine}{airplane.FuelAmount2 * -1} {PrintToken(airplane.FuelType2.IconPath, 16, airplane.FuelType2.LinkedPage)}";
        }
        // unify those two methods to accept common interface

        /// <summary>
        /// Converts a boolean value to "Yes" or "No" text.
        /// </summary>
        /// <param name="b">The boolean value to convert</param>
        /// <returns>"Yes" if true, "No" if false</returns>
        private string PrintBool(bool b)
        {
            if (b)
                return "Yes";
            else

                return "No";
        }

        /// <summary>
        /// Formats the end epoch for display, incrementing by 1 or showing "Never".
        /// </summary>
        /// <param name="epoch">The epoch number</param>
        /// <returns>The incremented epoch number as string, or "Never" if epoch >= 7</returns>
        private string PrintEndEpoch(int epoch)
        {
            if (epoch < 7)
            {
                epoch++;
                return $"{epoch}";
            }
            else
                return "Never";
        }

        /// <summary>
        /// Creates a single building table with appropriate headers based on building type.
        /// Generates different formats for power plants and industry buildings.
        /// </summary>
        /// <param name="path">The output path where the table file will be saved</param>
        /// <param name="b">The building to create a table for</param>
        public void CreateSingleTable(string path, Building b)
        {
            string header = "";
            if (b is PowerPlant)
            {
                header = $"\n!Image\n!Name\n!Consumes\n!Produces\n!{LinkText("Ticks", resources.LinkTick)} to{newLine}complete\n!{LinkText("Electricity", resources.Electrification.LinkedPage)}{newLine}generation\n{OutputBHeader(((IndustryBuilding)b).Rules.First())}\n!Building dimension{newLine}(tiles)";
                if ((b as PowerPlant).PollutionRange > 0)
                    header += $"\n!{LinkText("Pollution", resources.LinkPollution)}{newLine}range";
                File.WriteAllText(path + "building table.txt", $"{startTable}{header}{CreateBuildingTable(b as PowerPlant)}{endTable}");
            }
            else if (b is IndustryBuilding)
            {
                if (((IndustryBuilding)b).Rules.Any())
                {
                    header = $"\n!Image\n!Name\n!Consumes\n!Produces\n!{LinkText("Ticks", resources.LinkTick)} to{newLine}complete{OutputBHeader(((IndustryBuilding)b).Rules.First())}\n!Building{newLine}dimension (tiles)";
                    File.WriteAllText(path + "building table.txt", $"{startTable}{header}{CreateBuildingTable(b as IndustryBuilding)}{endTable}");
                }
                else
                {
                    header = $"\n!Image\n!Name\n!Consumes\n!Produces\n!{LinkText("Ticks", resources.LinkTick)} to{newLine}complete\n!Building{newLine}dimension (tiles)";
                    File.WriteAllText(path + "building table.txt", $"{startTable}{header}{CreateBuildingTable(b as IndustryBuilding)}{endTable}");
                }
            }
        }

        /// <summary>
        /// Creates a multi-row table for a list of buildings.
        /// Currently not implemented - method body is empty.
        /// </summary>
        /// <param name="path">The output path where the table file will be saved</param>
        /// <param name="buildings">The list of buildings to include in the table</param>
        public void CreateMultiTable(string path, List<Building> buildings)
        {

        }
        /// <summary>
        /// Creates a single upgrade table with appropriate headers based on upgrade type.
        /// Generates different formats for power plant upgrades and industry building upgrades.
        /// </summary>
        /// <param name="path">The output path where the table file will be saved</param>
        /// <param name="bu">The building upgrade to create a table for</param>
        public void CreateSingleTable(string path, IndustryBuildingUpgrade bu)
        {
            string header = "";
            if (bu is PowerPlantUpgrade)
            {
                header = $"\n!Image\n!Name\n!Description\n!Cost\n!Consumes\n!Produces\n!{LinkText("Ticks", resources.LinkTick)} to{newLine}complete\n!{LinkText("Electricity", resources.Electrification.LinkedPage)}{newLine}generation\n!Building dimension{newLine}(tiles)\n!Unlocked{newLine}at epoch";
                File.WriteAllText(path + "upgrades table.txt", $"{startTable}{header}{CreateBUTable(bu as PowerPlantUpgrade)}{endTable}");
            }
            else if (bu is IndustryBuildingUpgrade)
            {
                header = $"\n!Image\n!Name\n!Description\n!Cost\n!Consumes\n!Produces\n!{LinkText("Ticks", resources.LinkTick)} to{newLine}complete\n!Building dimension{newLine}(tiles)\n!Unlocked{newLine}at epoch";
                File.WriteAllText(path + "upgrades table.txt", $"{startTable}{header}{CreateBUTable(bu)}{endTable}");
            }
        }


        /// <summary>
        /// Creates a multi-row table for a list of building upgrades.
        /// Generates different headers for power plant upgrades vs industry building upgrades.
        /// </summary>
        /// <param name="path">The output path where the table file will be saved</param>
        /// <param name="upgrades">The list of building upgrades to include in the table</param>
        public void CreateMultiTable(string path, List<IndustryBuildingUpgrade> upgrades)
        {
            if (!upgrades.Any())
                return;
            string header = "";

            if (upgrades.First() is PowerPlantUpgrade)
            {
                header = $"\n!Image\n!Name\n!Cost\n!Consumes\n!Produces\n!{LinkText("Ticks", resources.LinkTick)} to{newLine}complete\n!{LinkText("Electricity", resources.Electrification.LinkedPage)}{newLine}generation\n!Building dimension{newLine}(tiles)\n!Unlocked{newLine}at epoch";
                string print = $"{startTable}{header}";
                foreach (PowerPlantUpgrade bu in upgrades)
                {
                    print += CreateBUTable(bu);
                }
                File.WriteAllText(path + "upgrades table.txt", $"{print}{endTable}");
            }
            else if (upgrades.First() is IndustryBuildingUpgrade)
            {
                header = $"\n!Image\n!Name\n!Cost\n!Consumes\n!Produces\n!{LinkText("Ticks", resources.LinkTick)} to{newLine}complete\n!Building dimension{newLine}(tiles)\n!Unlocked{newLine}at epoch";
                string print = $"{startTable}{header}";
                foreach (IndustryBuildingUpgrade bu in upgrades)
                {
                    print += CreateBUTable(bu);
                }
                File.WriteAllText(path + "upgrades table.txt", $"{print}{endTable}");
            }
        }

        /// <summary>
        /// Creates a MediaWiki table row for a power plant building.
        /// Handles single and multiple production rules with electricity generation.
        /// </summary>
        /// <param name="b">The power plant to create a table row for</param>
        /// <returns>A MediaWiki-formatted table row string</returns>
        private string CreateBuildingTable(PowerPlant b)
        {
            int rows = b.Rules.Count();
            if (rows == 1)
            {
                ProductionRule rule = b.Rules.First();
                return $"{newRow}{newColumn} {PrintImage(b.Name, 120)}{newColumn} {b.Name}{newColumn} {PrintRuleInput(rule)}{newColumn} {PrintRuleOutput(rule)}{newColumn} {rule.TimeSteps}" +
                    $"{newColumn} {b.ElGenerated}{OutputBonus(rule)}{OutputBD(b.BonusDistance, rule)}{newColumn} {PrintDimensions(b.DimX, b.DimY)}{PrintPollution(b.PollutionRange)}";
            }
            else
            {
                string[] rulePrint = new string[rows - 1];
                for (int i = 1; i < rows; i++)
                {
                    ProductionRule rule = b.Rules[i];
                    rulePrint[i - 1] += $"{newRowTab} {PrintRuleInput(rule)}{newColumnTab} {PrintRuleOutput(rule)}{newColumnTab} {rule.TimeSteps}";
                }
                ProductionRule firstRule = b.Rules.First();
                string result = $"{newRow}{newColumn}{RowSpan(rows)} {PrintImage(b.Name, 120)}{newColumn}{RowSpan(rows)} {b.Name}{newColumnTab} {PrintRuleInput(firstRule)}{newColumnTab} {PrintRuleOutput(firstRule)}" +
                    $"{newColumnTab} {firstRule.TimeSteps}{newColumn}{RowSpan(rows)} {b.ElGenerated}{newColumn}{RowSpan(rows)} {PrintDimensions(b.DimX, b.DimY)}{PrintPollution(b.PollutionRange)}";
                for (int i = 0; i < rulePrint.Length; i++)
                {
                    result += rulePrint[i];
                }
                return result;
            }
        }
        /// <summary>
        /// Creates a MediaWiki table row for an industry building.
        /// Handles buildings with no rules, single rule, or multiple production rules.
        /// </summary>
        /// <param name="b">The industry building to create a table row for</param>
        /// <returns>A MediaWiki-formatted table row string</returns>
        private string CreateBuildingTable(IndustryBuilding b)
        {
            int rows = b.Rules.Count();
            if (rows < 2)
            {
                if (rows == 0)
                    return $"{newRow}{newColumn} {PrintImage(b.Name, 120)}{newColumn} {b.Name}{newColumn} NA{newColumn} NA{newColumn} NA" +
                    $"{newColumn} {PrintDimensions(b.DimX, b.DimY)}";
                else
                {
                    ProductionRule rule = b.Rules.First();
                    return $"{newRow}{newColumn} {PrintImage(b.Name, 120)}{newColumn} {b.Name}{newColumn} {PrintRuleInput(rule)}{newColumn} {PrintRuleOutput(rule)}{newColumn} {rule.TimeSteps}" +
                        $"{OutputBonus(rule)}{OutputBD(b.BonusDistance, rule)}{newColumn} {PrintDimensions(b.DimX, b.DimY)}";
                }
            }
            else
            {
                string[] rulePrint = new string[rows - 1];
                for (int i = 1; i < rows; i++)
                {
                    ProductionRule rule = b.Rules[i];
                    rulePrint[i - 1] += $"{newRowTab}{newColumnTab} {PrintRuleInput(rule)}{newColumnTab} {PrintRuleOutput(rule)}{newColumnTab} {rule.TimeSteps}{OutputBonusMulti(rule)}{OutputBDMulti(b.BonusDistance, rule)}";
                }
                ProductionRule firstRule = b.Rules.First();
                string result = $"{newRow}{newColumn}{RowSpan(rows)} {PrintImage(b.Name, 120)}{newColumn}{RowSpan(rows)} {b.Name}{newColumnTab} {PrintRuleInput(firstRule)}{newColumnTab} {PrintRuleOutput(firstRule)}" +
                    $"{newColumnTab} {firstRule.TimeSteps}{OutputBonus(b.Rules.First())}{OutputBD(b.BonusDistance, firstRule)}{newColumn}{RowSpan(rows)} {PrintDimensions(b.DimX, b.DimY)}";
                for (int i = 0; i < rulePrint.Length; i++)
                {
                    result += rulePrint[i];
                }
                return result;
            }
        }
        /// <summary>
        /// Creates a MediaWiki table row for a power plant upgrade.
        /// Includes upgrade costs, production rules, and electricity generation.
        /// </summary>
        /// <param name="bu">The power plant upgrade to create a table row for</param>
        /// <returns>A MediaWiki-formatted table row string</returns>
        private string CreateBUTable(PowerPlantUpgrade bu)
        {
            int rows = bu.Rules.Count();
            if (rows < 2)
            {
                if (rows == 0)
                    return $"{newRow}{newColumn} {PrintImage(bu.Name, 120)}{newColumn} {bu.Name}{newColumn} {PrintCost(bu)} {newColumn} NA{newColumn} NA{newColumn} NA" +
                    $"{newColumn} {PrintDimensions(bu.DimX, bu.DimY)}";
                else
                {
                    ProductionRule rule = bu.Rules.First();
                    return $"{newRow}{newColumn} {PrintImage(bu.Name, 120)}{newColumn} {bu.Name}{newColumn} {PrintCost(bu)}{newColumn} {PrintRuleInput(rule)}{newColumn} {PrintRuleOutput(rule)}" +
                        $"{newColumn} {rule.TimeSteps} {newColumn} {bu.ElGeneration}{newColumn} {PrintDimensions(bu.DimX, bu.DimY)}{newColumn} {EpochP(bu.Epoch)}";
                }
            }
            else
            {
                string[] rulePrint = new string[rows - 1];
                for (int i = 1; i < rows; i++)
                {
                    ProductionRule rule = bu.Rules[i];
                    rulePrint[i - 1] += $"{newRowTab}{newColumnTab} {PrintRuleInput(rule)}{newColumnTab} {PrintRuleOutput(rule)}{newColumnTab} {rule.TimeSteps}";
                }
                ProductionRule firstRule = bu.Rules.First();
                string result = $"{newRow}{newColumn}{RowSpan(rows)} {PrintImage(bu.Name, 120)}{newColumn}{RowSpan(rows)} {bu.Name}{newColumn} {PrintCost(bu)}{newColumnTab} {PrintRuleInput(firstRule)}{newColumnTab} {PrintRuleOutput(firstRule)}" +
                    $"{newColumnTab} {firstRule.TimeSteps}{newColumn}{RowSpan(rows)} {bu.ElGeneration}{newColumn}{RowSpan(rows)} {PrintDimensions(bu.DimX, bu.DimY)}{newColumn} {EpochP(bu.Epoch)}";
                for (int i = 0; i < rulePrint.Length; i++)
                {
                    result += rulePrint[i];
                }
                return result;
            }
        }
        /// <summary>
        /// Creates a MediaWiki table row for an industry building upgrade.
        /// Includes upgrade costs, production rules, and building specifications.
        /// </summary>
        /// <param name="bu">The industry building upgrade to create a table row for</param>
        /// <returns>A MediaWiki-formatted table row string</returns>
        private string CreateBUTable(IndustryBuildingUpgrade bu)
        {
            int rows = bu.Rules.Count();
            if (rows < 2)
            {
                if (rows == 0)
                    return $"{newRow}{newColumn} {PrintImage(bu.Name, 120)}{newColumn} {bu.Name}{newColumn} {PrintCost(bu)}{newColumn} NA{newColumn} NA{newColumn} NA" +
                    $"{newColumn} {PrintDimensions(bu.DimX, bu.DimY)}{newColumn} {EpochP(bu.Epoch)}";
                else
                {
                    ProductionRule rule = bu.Rules.First();
                    return $"{newRow}{newColumn} {PrintImage(bu.Name, 120)}{newColumn} {bu.Name}{newColumn} {PrintCost(bu)}{newColumn} {PrintRuleInput(rule)}{newColumn} {PrintRuleOutput(rule)}{newColumn} {rule.TimeSteps}" +
                        $"{newColumn} {PrintDimensions(bu.DimX, bu.DimY)}{newColumn} {EpochP(bu.Epoch)}";
                }
            }
            else
            {
                string[] rulePrint = new string[rows - 1];
                for (int i = 1; i < rows; i++)
                {
                    ProductionRule rule = bu.Rules[i];
                    rulePrint[i - 1] += $"{newRowTab}{newColumnTab} {PrintRuleInput(rule)}{newColumnTab} {PrintRuleOutput(rule)}{newColumnTab} {rule.TimeSteps}";
                }
                ProductionRule firstRule = bu.Rules.First();
                string result = $"{newRow}{newColumn}{RowSpan(rows)} {PrintImage(bu.Name, 120)}{newColumn}{RowSpan(rows)} {bu.Name}{newColumn} {PrintCost(bu)}{newColumnTab} {PrintRuleInput(firstRule)}{newColumnTab} {PrintRuleOutput(firstRule)}" +
                    $"{newColumnTab} {firstRule.TimeSteps}{newColumn}{RowSpan(rows)} {PrintDimensions(bu.DimX, bu.DimY)}{newColumn}{RowSpan(rows)} {EpochP(bu.Epoch)}";
                for (int i = 0; i < rulePrint.Length; i++)
                {
                    result += rulePrint[i];
                }
                return result;
            }
        }
        /// <summary>
        /// Creates a multi-row table for town extensions of the same type.
        /// Designed only for same type groups - relax, amenities, or luxury extensions.
        /// </summary>
        /// <param name="path">The output path where the table file will be saved</param>
        /// <param name="tes">The list of town extensions to include in the table</param>
        public void CreateMultiTable(string path, List<TownExtension> tes)
        {
            if (!tes.Any())
                return;
            string header = "";
            header = $"\n!Image\n!Name\n!Cost{RelaxHeader(tes.First().Relax)}{AmenitiesHeader(tes.First().Amenities)}{LuxuryHeader(tes.First().Luxury)}\n!Building dimension{newLine}(tiles)\n!Unlocked{newLine}at epoch";
            string print = $"{startTable}{header}";
            foreach (TownExtension te in tes)
            {
                print += CreateTETable(te);
            }
            File.WriteAllText(path + "table.txt", $"{print}{endTable}");
        }

        /// <summary>
        /// Creates a single town extension table with headers for the specific bonus type.
        /// Includes appropriate columns for relax, amenities, or luxury bonuses.
        /// </summary>
        /// <param name="path">The output path where the table file will be saved</param>
        /// <param name="te">The town extension to create a table for</param>
        public void CreateSingleTable(string path, TownExtension te)
        {
            string header = $"\n!Image\n!Name\n!Cost{RelaxHeader(te.Relax)}{AmenitiesHeader(te.Amenities)}{LuxuryHeader(te.Luxury)}\n!Unlocked{newLine}at epoch\n!Building dimension{newLine}(tiles)\n!Unlocked{newLine}at epoch";
            File.WriteAllText(path + "table.txt", $"{startTable}{header}{CreateTETable(te)}{endTable}");
        }

        /// <summary>
        /// Creates a MediaWiki table row for a town extension.
        /// Includes cost, happiness bonuses (relax/amenities/luxury), and dimensions.
        /// </summary>
        /// <param name="te">The town extension to create a table row for</param>
        /// <returns>A MediaWiki-formatted table row string</returns>
        private string CreateTETable(TownExtension te)
        {
            return $"{newRow}{newColumn} {PrintImage(te.Name, 120)}{newColumn} {te.Name}{newColumn} {PrintCost(te)}{RelaxP(te.Relax)}{AmenitiesP(te.Amenities)}{LuxuryP(te.Luxury)}" +
                $"{newColumn} {PrintDimensions(te.DimX, te.DimY)}{newColumn} {EpochPTE(te.Epoch)}";
        }

        /// <summary>
        /// Creates a single town decoration table with basic building information.
        /// Includes image, name, cost, and building dimensions.
        /// </summary>
        /// <param name="path">The output path where the table file will be saved</param>
        /// <param name="td">The town decoration to create a table for</param>
        public void CreateSingleTable(string path, TownDecoration td)
        {
            string header = $"\n!Image\n!Name\n!Cost\n!Building dimension{newLine}(tiles)";
            File.WriteAllText(path + "table.txt", $"{startTable}{header}{CreateTDTable(td)}{endTable}");
        }

        /// <summary>
        /// Creates a MediaWiki table row for a town decoration.
        /// Includes basic information like cost and building dimensions.
        /// </summary>
        /// <param name="td">The town decoration to create a table row for</param>
        /// <returns>A MediaWiki-formatted table row string</returns>
        private string CreateTDTable(TownDecoration td)
        {
            return $"{newRow}{newColumn} {PrintImage(td.Name, 120)}{newColumn} {td.Name}{newColumn} {PrintCost(td)}{newColumn} {PrintDimensions(td.DimX, td.DimY)}";
        }
        /// <summary>
        /// Formats production rule input for display in MediaWiki tables.
        /// Combines normal rule input with electrification bonus input if applicable.
        /// </summary>
        /// <param name="r">The production rule to format</param>
        /// <returns>A formatted input string with token icons</returns>
        private string PrintRuleInput(ProductionRule r)
        {
            if (r.RuleEl.Input1Count == 0)
            {
                return $"{PrintRuleMultiI(r.RuleN)}";
            }
            return $"({PrintRuleMultiI(r.RuleEl)}){PrintToken(resources.Electrification, 16)} + {PrintRuleMultiI(r.RuleN)}";

        }

        /// <summary>
        /// Formats production rule output for display in MediaWiki tables.
        /// Combines normal rule output with electrification bonus output if applicable.
        /// </summary>
        /// <param name="r">The production rule to format</param>
        /// <returns>A formatted output string with token icons</returns>
        private string PrintRuleOutput(ProductionRule r)
        {
            if (r.RuleEl.Output1Count == 0)
            {
                return $"{PrintRuleMultiO(r.RuleN)}";
            }
            else
                return $"{PrintRuleMultiO(r.RuleN)} + ({PrintRuleMultiO(r.RuleEl)}){PrintToken(resources.Electrification, 16)}";
        }

        /// <summary>
        /// Formats multiple input resources for a production rule.
        /// Handles 1-3 input types with their counts and token icons.
        /// </summary>
        /// <param name="r">The rule to format inputs for</param>
        /// <returns>A formatted input string with token icons, or "-" if no inputs</returns>
        private string PrintRuleMultiI(IRule r)
        {
            if (r.Input1Count == 0)
                return "-";
            else if (r.Input2Count == 0)
                return $"{r.Input1Count} {PrintToken(r.Input1Type, 16)}";
            else if (r.Input3Count == 0)
                return $"{r.Input1Count} {PrintToken(r.Input1Type, 16)} {r.Input2Count} {PrintToken(r.Input2Type, 16)}";
            else
                return $"{r.Input1Count} {PrintToken(r.Input1Type, 16)} {r.Input2Count} {PrintToken(r.Input2Type, 16)} {r.Input3Count} {PrintToken(r.Input3Type, 16)}";
        }

        /// <summary>
        /// Formats multiple output resources for a production rule.
        /// Handles 1-3 output types with their counts and token icons.
        /// </summary>
        /// <param name="r">The rule to format outputs for</param>
        /// <returns>A formatted output string with token icons</returns>
        private string PrintRuleMultiO(IRule r)
        {
            if (r.Output2Count == 0)
                return $"{r.Output1Count} {PrintToken(r.Output1Type, 16)}";
            else if (r.Input3Count == 0)
                return $"{r.Output1Count} {PrintToken(r.Output1Type, 16)} {r.Output2Count} {PrintToken(r.Output2Type, 16)}";
            else
                return $"{r.Input1Count} {PrintToken(r.Output1Type, 16)} {r.Output2Count} {PrintToken(r.Output2Type, 16)} {r.Output3Count} {PrintToken(r.Output3Type, 16)}";
        }

        /// <summary>
        /// Formats building dimensions for display.
        /// Shows "Random shape" for irregular buildings or "XxY" for rectangular ones.
        /// </summary>
        /// <param name="dimX">The X dimension (0 indicates random/irregular shape)</param>
        /// <param name="dimY">The Y dimension (total tiles for irregular shapes)</param>
        /// <returns>A formatted dimension string</returns>
        private string PrintDimensions(int dimX, int dimY)
        {
            if (dimX == 0)
                return $"Random shape {newLine}({dimY} tiles total)";
            else
                return $"{dimX}x{dimY}";
        }

        /// <summary>
        /// Creates a MediaWiki rowspan attribute for table cells.
        /// </summary>
        /// <param name="rows">The number of rows to span</param>
        /// <returns>A MediaWiki rowspan attribute string</returns>
        private string RowSpan(int rows)
        {
            return $"rowspan=\"{rows}\"|";
        }

        /// <summary>
        /// Formats pollution range for display in building tables.
        /// Only shows pollution column if the building actually pollutes.
        /// </summary>
        /// <param name="pollution">The pollution range value</param>
        /// <returns>A formatted pollution column or empty string if no pollution</returns>
        private string PrintPollution(int pollution)
        {
            if (pollution > 0)
                return $"{newColumn} {pollution}";
            else
                return "";
        }

        /// <summary>
        /// Creates a MediaWiki link with display text.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="link">The target page to link to</param>
        /// <returns>A MediaWiki link with custom display text</returns>
        private string LinkText(string text, string link)
        {
            return $"[[{link}|{text}]]";
        }

        /// <summary>
        /// Creates a table header for relax bonus if the value is greater than 0.
        /// </summary>
        /// <param name="relax">The relax bonus value</param>
        /// <returns>A formatted header string or empty string if no relax bonus</returns>
        private string RelaxHeader(int relax)
        {
            if (relax == 0)
                return "";
            else
                return $"\n!Relax{newLine}(Catchment area)";
        }

        /// <summary>
        /// Creates a table cell for relax bonus value if greater than 0.
        /// </summary>
        /// <param name="relax">The relax bonus value</param>
        /// <returns>A formatted table cell or empty string if no relax bonus</returns>
        private string RelaxP(int relax)
        {
            if (relax == 0)
                return "";
            else
                return $"{newColumn} {relax}";
        }

        /// <summary>
        /// Creates a table header for amenities bonus if the value is greater than 0.
        /// </summary>
        /// <param name="amenities">The amenities bonus value</param>
        /// <returns>A formatted header string or empty string if no amenities bonus</returns>
        private string AmenitiesHeader(int amenities)
        {
            if (amenities == 0)
                return "";
            else
                return $"\n!Amenities{newLine}(Catchment area)";
        }

        /// <summary>
        /// Creates a table cell for amenities bonus value if greater than 0.
        /// </summary>
        /// <param name="amenities">The amenities bonus value</param>
        /// <returns>A formatted table cell or empty string if no amenities bonus</returns>
        private string AmenitiesP(int amenities)
        {
            if (amenities == 0)
                return "";
            else
                return $"{newColumn} {amenities}";
        }

        /// <summary>
        /// Creates a table header for luxury bonus if the value is greater than 0.
        /// </summary>
        /// <param name="luxury">The luxury bonus value</param>
        /// <returns>A formatted header string or empty string if no luxury bonus</returns>
        private string LuxuryHeader(int luxury)
        {
            if (luxury == 0)
                return "";
            else
                return $"\n!Luxury{newLine}(Catchment area)";
        }

        /// <summary>
        /// Creates a table cell for luxury bonus value if greater than 0.
        /// </summary>
        /// <param name="luxury">The luxury bonus value</param>
        /// <returns>A formatted table cell or empty string if no luxury bonus</returns>
        private string LuxuryP(int luxury)
        {
            if (luxury == 0)
                return "";
            else
                return $"{newColumn} {luxury}";
        }

        /// <summary>
        /// Formats the cost for a building upgrade as MediaWiki markup.
        /// Handles 1-3 different cost types and displays "NA" for free upgrades.
        /// </summary>
        /// <param name="u">The building upgrade to format the cost for</param>
        /// <returns>A formatted cost string with token icons</returns>
        private string PrintCost(BuildingUpgrade u)
        {
            string print = "NA";
            if (u.Cost2Count == 0 && !(u.Cost1Count == 0))
            {
                print = $"{u.Cost1Count * -1} {PrintToken(u.Cost1Type, 16)}";
            }
            else if (u.Cost3Count == 0)
            {
                print = $"{u.Cost1Count * -1} {PrintToken(u.Cost1Type, 16)}{newLine}{u.Cost2Count * -1} {PrintToken(u.Cost2Type, 16)}";
            }
            else
            {
                print = $"{u.Cost1Count * -1} {PrintToken(u.Cost1Type, 16)}{newLine}{u.Cost2Count * -1} {PrintToken(u.Cost2Type, 16)}{newLine}{u.Cost3Count * -1} {PrintToken(u.Cost3Type, 16)}";
            }
            return print;
        }

        /// <summary>
        /// Creates table headers for output bonus columns if the rule has distance bonuses.
        /// </summary>
        /// <param name="rule">The production rule to check for output bonuses</param>
        /// <returns>Header columns for bonus output and distance, or empty string if no bonuses</returns>
        private string OutputBHeader(ProductionRule rule)
        {
            if (rule == null || rule.RuleN.OutputDistBonusCount == 0)
                return "";
            else
                return $"\n!Bonus{newLine}ouput\n!Bonus distance{newLine}(tiles)";
        }

        /// <summary>
        /// Creates a table cell for bonus distance if the rule has distance bonuses.
        /// </summary>
        /// <param name="distance">The bonus distance value</param>
        /// <param name="rule">The production rule to check for output bonuses</param>
        /// <returns>A formatted distance cell or empty string if no distance bonus</returns>
        private string OutputBD(int distance, ProductionRule rule)
        {
            if (distance == 0 || rule.RuleN.OutputDistBonusCount == 0)
                return "";
            else
                return $"{newColumn} {distance}";
        }

        /// <summary>
        /// Creates a table cell for output bonus resources if the rule has distance bonuses.
        /// </summary>
        /// <param name="rule">The production rule to format the output bonus for</param>
        /// <returns>A formatted bonus output cell or empty string if no bonus</returns>
        private string OutputBonus(ProductionRule rule)
        {
            if (rule.RuleN.OutputDistBonusCount == 0)
                return "";
            return $"{newColumn} {rule.RuleN.OutputDistBonusCount} {PrintToken(rule.RuleN.OutputDistBonusType, 16)}";
        }

        /// <summary>
        /// Creates a table cell for bonus distance in multi-rule tables, showing "-" when no bonus.
        /// </summary>
        /// <param name="distance">The bonus distance value</param>
        /// <param name="rule">The production rule to check for output bonuses</param>
        /// <returns>A formatted distance cell or "-" if no distance bonus</returns>
        private string OutputBDMulti(int distance, ProductionRule rule)
        {
            if (distance == 0 || rule.RuleN.OutputDistBonusCount == 0)
                return $"{newColumn} -";
            else
                return $"{newColumn} {distance}";
        }

        /// <summary>
        /// Creates a table cell for output bonus in multi-rule tables, showing "-" when no bonus.
        /// </summary>
        /// <param name="rule">The production rule to format the output bonus for</param>
        /// <returns>A formatted bonus output cell or "-" if no bonus</returns>
        private string OutputBonusMulti(ProductionRule rule)
        {
            if (rule.RuleN.OutputDistBonusCount == 0)
                return $"{newColumn} -";
            return $"{newColumn} {rule.RuleN.OutputDistBonusCount} {PrintToken(rule.RuleN.OutputDistBonusType, 16)}";
        }


        /// <summary>
        /// Creates a rowspan attribute for output bonus columns in multi-rule tables.
        /// </summary>
        /// <param name="distance">The bonus distance value</param>
        /// <param name="rows">The number of rows to span</param>
        /// <returns>A rowspan attribute or empty string if no distance bonus</returns>
        private string OutputBonusRowspan(int distance, int rows)
        {
            if (distance == 0)
                return "";
            else
                return $"{RowSpan(rows)}";
        }

        /// <summary>
        /// Formats epoch numbers for display, showing "-" for epoch 0.
        /// </summary>
        /// <param name="epoch">The epoch number</param>
        /// <returns>The epoch number as string, or "-" if epoch is 0</returns>
        private string EpochP(int epoch)
        {
            if (epoch == 0)
                return "-";
            else
                return $"{epoch}";
        }

        /// <summary>
        /// Formats epoch numbers for town extensions, showing "1" for epoch 0.
        /// Town extensions default to epoch 1 when not specified.
        /// </summary>
        /// <param name="epoch">The epoch number</param>
        /// <returns>The epoch number as string, or "1" if epoch is 0</returns>
        private string EpochPTE(int epoch)
        {
            if (epoch == 0)
                return "1";
            else
                return $"{epoch}";
        }
    }
}
