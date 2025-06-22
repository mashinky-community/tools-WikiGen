using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MashinkyWikiGen
{
    public class WikiScriptGen
    {
        const string newLine = "<br/>";
        const string startTable = "{| class=\"wikitable transformable \" style=text-align:center";
        const string endTable = "\n|}";
        const string newRow = "\n|-";
        const string newRowTab = "\n   |-";
        const string newColumn = "\n|";
        const string newColumnTab = "\n   |";
        const string tab = "   ";
        const string doubleTab = "      ";
        private Resources resources;


        public WikiScriptGen(Resources resources)
        {
            this.resources = resources;
        }

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
        public string CreateWagonTable(VehicleBase w)
        {
            return $"{newRow}{newColumn} {PrintImage(w.Name, 120)}{newColumn} {w.Name}{newLine}{newLine} {PrintIcon(w.Name, 80)}{newColumn} {w.Capacity} {PrintToken(w.Cargo.IconPath, 16, w.Cargo.LinkedPage)} {newColumn} {PrintPurchaseCost(w)}{newColumn} {w.Weight}" +
                $"{newColumn} {w.StartEpoch}{newColumn} {PrintBool(w.ReqDepotExtension)}";
        }
        private string engineTHeader;
        //private string engineTHeader = $"\n!Image\n!Name\n!Max speed\n!Pull up to\n!Power\n!Weight\n!Length\n!Purchase{newLine}cost\n!Operating{newLine}cost\n!Unlocked{newLine}at epoch\n!Depot extension{newLine}required?";
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
        private string CreateVehicleTable(Engine v)
        {
            vehicleHeader = $"\n!Image\n!Name\n!Max speed\n!Power\n!Weight\n!Length\n!Capacity\n!Purchase{newLine}cost\n!{LinkText($"Operating{newLine}cost", resources.LinkOperatingCost)} \n!Unlocked{newLine}at epoch";
            return $"{newRow}{newColumn} {PrintImage(v.Name, 120)}{newColumn} {v.Name}{newLine}{newLine} {PrintIcon(v.Name, 80)}{newColumn} {v.MaxSpeed} km/h{newLine}={newLine}{v.MaxSpeedMiles} mph" +
                $"{newColumn} {v.Power} hp{newColumn} {v.Weight} t{newColumn} {v.Length}{newColumn} {v.Capacity} {PrintToken(v.Cargo.IconPath, 16, v.Cargo.LinkedPage)} {newColumn} {PrintPurchaseCost(v)}{newColumn} {PrintOperatingCost(v)}{newColumn}{v.StartEpoch}" +
                $"";
        }

        private string airplaneHeader = "";
        private string CreateVehicleTable(Airplane airplane)
        {
            airplaneHeader = $"\n!Image\n!Name\n!Max speed\n!Power\n!Weight\n!Length\n!Runway{newLine}length\n!Flight{newLine}altitude\n!Capacity\n!Purchase{newLine}cost\n!{LinkText($"Operating{newLine}cost", resources.LinkOperatingCost)} \n!Unlocked{newLine}at epoch";
            return $"{newRow}{newColumn} {PrintImage(airplane.Name, 120)}{newColumn} {airplane.Name}{newLine}{newLine} {PrintIcon(airplane.Name, 80)}{newColumn} {airplane.MaxSpeed} km/h{newLine}={newLine}{airplane.MaxSpeedMiles} mph" +
                $"{newColumn} {airplane.Power} hp{newColumn} {airplane.Weight} t{newColumn} {airplane.Length} {newColumn} {airplane.RunwayLength} {newColumn} {airplane.FlightAltitude} {newColumn} {airplane.Capacity} {PrintToken(airplane.Cargo.IconPath, 16, airplane.Cargo.LinkedPage)} {newColumn} {PrintPurchaseCost(airplane)}{newColumn} {PrintOperatingCost(airplane)}{newColumn}{airplane.StartEpoch}" +
                $"";
        }

        private string PrintImage(string name, int size)
        {
            name = name.Replace(" ", "_");
            return $"[[File:{name}.png|x{size}px]]";
        }

        private string PrintImage(string name)
        {
            name = name.Replace(" ", "_");
            return $"[[File:{name}.png]]";
        }
        private string PrintToken(string path)
        {
            return $"[[File:{path}]]";
        }

        private string PrintToken(string path, int size, string link)
        {
            return $"[[File:{path}|{size}px|link={link}]]";
        }

        private string PrintToken(Token t, int size)
        {
            return $"[[File:{t.IconPath}|{size}px|link={t.LinkedPage}]]";
        }

        private string PrintIcon(string name)
        {
            name = name.Replace(" ", "_");
            return $"[[File:{name}_icon.png]]";
        }
        private string PrintIcon(string name, int size)
        {
            name = name.Replace(" ", "_");
            return $"[[File:{name}_icon.png|x{size}px]]";
        }

        private string PrintPurchaseCost(VehicleBase w)
        {
            if (w.CostAmount1 == 0)
                return "Quest reward";
            else if (w.CostAmount2 == 0)
                return $"{w.CostAmount1 * -1} {PrintToken(w.CostType1.IconPath, 16, w.CostType1.LinkedPage)}";
            else
                return $"{w.CostAmount1 * -1} {PrintToken(w.CostType1.IconPath, 16, w.CostType1.LinkedPage)}{newLine}+{newLine}{w.CostAmount2 * -1} {PrintToken(w.CostType2.IconPath, 16, w.CostType2.LinkedPage)}";
        }

        private string PrintOperatingCost(Engine e)
        {
            if (e.FuelAmount2 == 0)
                return $"{e.FuelAmount1 * -1} {PrintToken(e.FuelType1.IconPath, 16, e.FuelType1.LinkedPage)}";
            else
                return $"{e.FuelAmount1 * -1} {PrintToken(e.FuelType1.IconPath, 16, e.FuelType1.LinkedPage)}{newLine}+{newLine}{e.FuelAmount2 * -1} {PrintToken(e.FuelType2.IconPath, 16, e.FuelType2.LinkedPage)}";
        }

        private string PrintOperatingCost(Airplane airplane)
        {
            if (airplane.FuelAmount2 == 0)
                return $"{airplane.FuelAmount1 * -1} {PrintToken(airplane.FuelType1.IconPath, 16, airplane.FuelType1.LinkedPage)}";
            else
                return $"{airplane.FuelAmount1 * -1} {PrintToken(airplane.FuelType1.IconPath, 16, airplane.FuelType1.LinkedPage)}{newLine}+{newLine}{airplane.FuelAmount2 * -1} {PrintToken(airplane.FuelType2.IconPath, 16, airplane.FuelType2.LinkedPage)}";
        }
        // unify those two methods to accept common interface

        private string PrintBool(bool b)
        {
            if (b)
                return "Yes";
            else

                return "No";
        }

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

        public void CreateMultiTable(string path, List<Building> buildings)
        {

        }
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
        /// designed only from same type groups - relax or amenities or luxury
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tes"></param>
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

        public void CreateSingleTable(string path, TownExtension te)
        {
            string header = $"\n!Image\n!Name\n!Cost{RelaxHeader(te.Relax)}{AmenitiesHeader(te.Amenities)}{LuxuryHeader(te.Luxury)}\n!Unlocked{newLine}at epoch\n!Building dimension{newLine}(tiles)\n!Unlocked{newLine}at epoch";
            File.WriteAllText(path + "table.txt", $"{startTable}{header}{CreateTETable(te)}{endTable}");
        }

        private string CreateTETable(TownExtension te)
        {
            return $"{newRow}{newColumn} {PrintImage(te.Name, 120)}{newColumn} {te.Name}{newColumn} {PrintCost(te)}{RelaxP(te.Relax)}{AmenitiesP(te.Amenities)}{LuxuryP(te.Luxury)}" +
                $"{newColumn} {PrintDimensions(te.DimX, te.DimY)}{newColumn} {EpochPTE(te.Epoch)}";
        }

        public void CreateSingleTable(string path, TownDecoration td)
        {
            string header = $"\n!Image\n!Name\n!Cost\n!Building dimension{newLine}(tiles)";
            File.WriteAllText(path + "table.txt", $"{startTable}{header}{CreateTDTable(td)}{endTable}");
        }

        private string CreateTDTable(TownDecoration td)
        {
            return $"{newRow}{newColumn} {PrintImage(td.Name, 120)}{newColumn} {td.Name}{newColumn} {PrintCost(td)}{newColumn} {PrintDimensions(td.DimX, td.DimY)}";
        }
        private string PrintRuleInput(ProductionRule r)
        {
            if (r.RuleEl.Input1C == 0)
            {
                return $"{PrintRuleMultiI(r.RuleN)}";
            }
            return $"({PrintRuleMultiI(r.RuleEl)}){PrintToken(resources.Electrification, 16)} + {PrintRuleMultiI(r.RuleN)}";

        }

        private string PrintRuleOutput(ProductionRule r)
        {
            if (r.RuleEl.Output1C == 0)
            {
                return $"{PrintRuleMultiO(r.RuleN)}";
            }
            else
                return $"{PrintRuleMultiO(r.RuleN)} + ({PrintRuleMultiO(r.RuleEl)}){PrintToken(resources.Electrification, 16)}";
        }

        private string PrintRuleMultiI(IRule r)
        {
            if (r.Input1C == 0)
                return "-";
            else if (r.Input2C == 0)
                return $"{r.Input1C} {PrintToken(r.Input1T, 16)}";
            else if (r.Input3C == 0)
                return $"{r.Input1C} {PrintToken(r.Input1T, 16)} {r.Input2C} {PrintToken(r.Input2T, 16)}";
            else
                return $"{r.Input1C} {PrintToken(r.Input1T, 16)} {r.Input2C} {PrintToken(r.Input2T, 16)} {r.Input3C} {PrintToken(r.Input3T, 16)}";
        }

        private string PrintRuleMultiO(IRule r)
        {
            if (r.Output2C == 0)
                return $"{r.Output1C} {PrintToken(r.Output1T, 16)}";
            else if (r.Input3C == 0)
                return $"{r.Output1C} {PrintToken(r.Output1T, 16)} {r.Output2C} {PrintToken(r.Output2T, 16)}";
            else
                return $"{r.Input1C} {PrintToken(r.Output1T, 16)} {r.Output2C} {PrintToken(r.Output2T, 16)} {r.Output3C} {PrintToken(r.Output3T, 16)}";
        }

        private string PrintDimensions(int dimX, int dimY)
        {
            if (dimX == 0)
                return $"Random shape {newLine}({dimY} tiles total)";
            else
                return $"{dimX}x{dimY}";
        }

        private string RowSpan(int rows)
        {
            return $"rowspan=\"{rows}\"|";
        }

        private string PrintPollution(int pollution)
        {
            if (pollution > 0)
                return $"{newColumn} {pollution}";
            else
                return "";
        }

        private string LinkText(string text, string link)
        {
            return $"[[{link}|{text}]]";
        }

        private string RelaxHeader(int relax)
        {
            if (relax == 0)
                return "";
            else
                return $"\n!Relax{newLine}(Catchment area)";
        }

        private string RelaxP(int relax)
        {
            if (relax == 0)
                return "";
            else
                return $"{newColumn} {relax}";
        }

        private string AmenitiesHeader(int amenities)
        {
            if (amenities == 0)
                return "";
            else
                return $"\n!Amenities{newLine}(Catchment area)";
        }

        private string AmenitiesP(int amenities)
        {
            if (amenities == 0)
                return "";
            else
                return $"{newColumn} {amenities}";
        }

        private string LuxuryHeader(int luxury)
        {
            if (luxury == 0)
                return "";
            else
                return $"\n!Luxury{newLine}(Catchment area)";
        }

        private string LuxuryP(int luxury)
        {
            if (luxury == 0)
                return "";
            else
                return $"{newColumn} {luxury}";
        }

        private string PrintCost(BuildingUpgrade u)
        {
            string print = "NA";
            if (u.Cost2C == 0 && !(u.Cost1C == 0))
            {
                print = $"{u.Cost1C * -1} {PrintToken(u.Cost1T, 16)}";
            }
            else if (u.Cost3C == 0)
            {
                print = $"{u.Cost1C * -1} {PrintToken(u.Cost1T, 16)}{newLine}{u.Cost2C * -1} {PrintToken(u.Cost2T, 16)}";
            }
            else
            {
                print = $"{u.Cost1C * -1} {PrintToken(u.Cost1T, 16)}{newLine}{u.Cost2C * -1} {PrintToken(u.Cost2T, 16)}{newLine}{u.Cost3C * -1} {PrintToken(u.Cost3T, 16)}";
            }
            return print;
        }

        private string OutputBHeader(ProductionRule rule)
        {
            if (rule == null || rule.RuleN.OutputDistBonusC == 0)
                return "";
            else
                return $"\n!Bonus{newLine}ouput\n!Bonus distance{newLine}(tiles)";
        }

        private string OutputBD(int distance, ProductionRule rule)
        {
            if (distance == 0 || rule.RuleN.OutputDistBonusC == 0)
                return "";
            else
                return $"{newColumn} {distance}";
        }

        private string OutputBonus(ProductionRule rule)
        {
            if (rule.RuleN.OutputDistBonusC == 0)
                return "";
            return $"{newColumn} {rule.RuleN.OutputDistBonusC} {PrintToken(rule.RuleN.OutputDistBonusT, 16)}";
        }

        private string OutputBDMulti(int distance, ProductionRule rule)
        {
            if (distance == 0 || rule.RuleN.OutputDistBonusC == 0)
                return $"{newColumn} -";
            else
                return $"{newColumn} {distance}";
        }

        private string OutputBonusMulti(ProductionRule rule)
        {
            if (rule.RuleN.OutputDistBonusC == 0)
                return $"{newColumn} -";
            return $"{newColumn} {rule.RuleN.OutputDistBonusC} {PrintToken(rule.RuleN.OutputDistBonusT, 16)}";
        }


        private string OutputBonusRowspan(int distance, int rows)
        {
            if (distance == 0)
                return "";
            else
                return $"{RowSpan(rows)}";
        }

        private string EpochP(int epoch)
        {
            if (epoch == 0)
                return "-";
            else
                return $"{epoch}";
        }

        private string EpochPTE(int epoch)
        {
            if (epoch == 0)
                return "1";
            else
                return $"{epoch}";
        }
    }
}
