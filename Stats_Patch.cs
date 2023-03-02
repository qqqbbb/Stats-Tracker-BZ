using HarmonyLib;
//using QModManager.Utility;
using System;
//using System.Linq;
//using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using SMLHelper.V2.Handlers;
using static ErrorMessage;

namespace Stats_Tracker
{
    internal class Stats_Patch
    {
        static SeaTruckSegment destroyedSTS = null;
        static bool removeIngredient = false;
        static bool constructing = false;
        static bool finishedCrafting = false;
        static LiveMixin killedLM;
        public static string saveSlot;
        public static CraftNode lastEncNode;
        public static Dictionary<string, PDAEncyclopedia.EntryData> mapping;
        public static List <TechType> roomTypes = new List<TechType> { TechType.BaseRoom, TechType.BaseLargeRoom, TechType.BaseMapRoom, TechType.BaseMoonpool, TechType.BaseObservatory, TechType.BaseControlRoom, TechType.BaseLargeGlassDome, TechType.BaseGlassDome, TechType.BaseMoonpoolExpansion};
        public static List<TechType> corridorTypes = new List<TechType> { TechType.BaseCorridorI, TechType.BaseCorridorL, TechType.BaseCorridorT, TechType.BaseCorridorX, TechType.BaseCorridorGlassI, TechType.BaseCorridorGlassL, TechType.BaseCorridor, TechType.BaseCorridorGlass, TechType.BaseCorridor};
        public static List<TechType> fauna = new List<TechType> { TechType.Brinewing, TechType.BruteShark, TechType.Cryptosuchus, TechType.NootFish, TechType.Penguin, TechType.PenguinBaby, TechType.Crash, TechType.Pinnacarid, TechType.RockPuncher, TechType.SnowStalker, TechType.SnowStalkerBaby, TechType.SpikeyTrap, TechType.Bladderfish, TechType.Boomerang, TechType.SquidShark, TechType.Symbiote, TechType.ArcticPeeper, TechType.ArcticRay, TechType.ArrowRay, TechType.DiscusFish, TechType.FeatherFish, TechType.FeatherFishRed, TechType.Hoopfish, TechType.Jellyfish, TechType.HivePlant, TechType.LilyPaddler, TechType.SeaMonkey, TechType.SeaMonkeyBaby, TechType.SpinnerFish, TechType.TitanHolefish, TechType.Skyray, TechType.Triops, TechType.Spinefish, TechType.BlueAmoeba, TechType.TrivalveBlue, TechType.TrivalveYellow };
        public static List<TechType> leviathans = new List<TechType>
        {TechType.GlowWhale, TechType.Chelicerate, TechType.IceWorm, TechType.LargeVentGarden, TechType.SmallVentGarden, TechType.SeaEmperorJuvenile, TechType.ShadowLeviathan };
        //public static string[] moddedCreatureTechtypes = new string[] { "StellarThalassacean", "JasperThalassacean", "GrandGlider", "Axetail", "AmberClownPincher", "CitrineClownPincher", "EmeraldClownPincher", "RubyClownPincher", "SapphireClownPincher", "GulperLeviathan", "RibbonRay", "Twisteel", "Filtorb", "JellySpinner", "Trianglefish" };
        public static List<TechType> coral = new List<TechType> { TechType.CoralShellPlate, TechType.BrownTubes, TechType.BigCoralTubes, TechType.BlueCoralTubes, TechType.RedTipRockThings, TechType.GenericJeweledDisk, TechType.BlueJeweledDisk, TechType.GreenJeweledDisk, TechType.RedJeweledDisk, TechType.PurpleJeweledDisk, TechType.TreeMushroom, TechType.BrainCoral, TechType.TwistyBridgesCoralShelf };
        // Mace Plant, Blooming Raindrops, Cliff Lantern, Paddle Fern, Deep Lily Pads Flower 03, Spotted Reeds Relative, Tongue Plant, Hanging Plant, Hardy Cave Bush, 
        public static List<TechType> flora = new List<TechType> { TechType.BloodRoot, TechType.BloodVine, TechType.SmallMaroonPlant, TechType.GenericArmored, TechType.LilyPadMature, TechType.LilyPadRoot, TechType.GenericBigPlant1, TechType.GenericShellSingle, TechType.PurpleBranches, TechType.PurpleVegetablePlant, TechType.Creepvine, TechType.GenericShellDouble, TechType.EyesPlant, TechType.FernPalm, TechType.BlueFurPlant, TechType.DeepTwistyBridgesLargePlant, TechType.JellyPlant, TechType.RedGreenTentacle, TechType.OrangePetalsPlant, TechType.GenericBulbStalk, TechType.TwistyBridgesMushroom, TechType.HangingFruitTree, TechType.CavePlant, TechType.MelonPlant, TechType.DeepLilyPadsLanternPlant, TechType.TwistyBridgeCliffPlant, TechType.OxygenPlant, TechType.RedBush, TechType.TwistyBridgeCoralLong, TechType.RedBasketPlant, TechType.GenericCage, TechType.GlacialPouchBulb, TechType.ShellGrass, TechType.SpottedLeavesPlant, TechType.CrashHome, TechType.SnowStalkerPlant, TechType.TapePlant, TechType.PurpleStalk, TechType.PinkFlower, TechType.PurpleTentacle, TechType.BloodGrass, TechType.RedGrass, TechType.RedSeaweed, TechType.BlueBarnacle, TechType.BlueBarnacleCluster, TechType.BlueLostRiverLilly, TechType.BlueTipLostRiverPlant, TechType.HangingStinger, TechType.CoveTree, TechType.BlueCluster, TechType.GreenReeds, TechType.BarnacleSuckers, TechType.BallClusters, TechType.GenericCrystal, TechType.GenericBowl, TechType.TallShootsPlant, TechType.TreeSpireMushroom, TechType.GenericRibbon, TechType.HeatFruitPlant, TechType.GlacialTree, TechType.IceFruitPlant, TechType.FrozenRiverPlant2, TechType.FrozenRiverPlant1, TechType.SnowPlant, TechType.Mohawk, TechType.PurpleRattle, TechType.GenericSpiral, TechType.CaveFlower, TechType.TrianglePlant, TechType.OrangePetalsPlant, TechType.ThermalLily, TechType.TwistyBridgesLargePlant, TechType.ThermalSpireBarnacle, TechType.TornadoPlates, TechType.HoneyCombPlant, TechType.LeafyFruitPlant, TechType.GlacialBulb, TechType.PinkFlower, TechType.GenericBigPlant2, TechType.DeepLilyShroom, TechType.KelpRoot };
        static TimeSpan bedTimeStart = TimeSpan.Zero;
        public static TimeSpan timeLastUpdate = TimeSpan.Zero;
        public static Dictionary<string, string> myStrings = new Dictionary<string, string>();
        public static Dictionary<string, string> descs = new Dictionary<string, string>();
        public static HashSet<PowerRelay> powerRelays = new HashSet<PowerRelay>();
        static string timePlayed { get { return "Time since landing on the planet: " + GetTimePlayed().Days + " days, " + GetTimePlayed().Hours + " hours, " + GetTimePlayed().Minutes + " minutes"; } }
        static string timePlayedTotal
        {
            get
            {
                TimeSpan total = TimeSpan.Zero;
                foreach (var item in Main.config.timePlayed)
                {
                    //if (item.Key != saveSlot)
                        total += item.Value;
                }
                total += GetTimePlayed();
                return "Time since landing on the planet: " + total.Days + " days, " + total.Hours + " hours, " + total.Minutes + " minutes";
            }
        }
        static TravelMode travelMode;
        static int itemsCraftedTotal
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.itemsCraftedTotal)
                    total += kv.Value;

                return total;
            }
        }
        public static int basePower
        {
            get
            {
                int total = 0;
                foreach (PowerRelay pr in powerRelays)
                {
                    if (pr)
                        total += (int)pr.GetMaxPower();
                }
                return total;
            }
        }
        static int basePowerTotal
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.basePower)
                {
                    //if (kv.Key != saveSlot)
                    total += kv.Value;
                }
                total += basePower;

                return total;
            }
        }
        static int objectsScannedTotal
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.objectsScanned)
                {
                    //if (kv.Key != saveSlot)
                        total += kv.Value;
                }
                total += Main.config.objectsScanned[saveSlot];

                return total;
            }
        }
        static int blueprintsUnlockedTotal
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.blueprintsUnlocked)
                {
                    //if (kv.Key != saveSlot)
                        total += kv.Value;
                }
                total += Main.config.blueprintsUnlocked[saveSlot];

                return total;
            }
        }
        static int blueprintsFromDataboxesTotal
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.blueprintsFromDatabox)
                {
                    //if (kv.Key != saveSlot)
                        total += kv.Value;
                }
                total += Main.config.blueprintsFromDatabox[saveSlot];

                return total;
            }
        }
        static int plantsKilledTotal
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.plantsKilledTotal)
                    total += kv.Value;

                return total;
            }
        }
        static int animalsKilledTotal
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.animalsKilledTotal)
                    total += kv.Value;

                return total;
            }
        }
        static int coralKilledTotal
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.coralKilledTotal)
                    total += kv.Value;

                return total;
            }
        }
        static int leviathansKilledTotal
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.leviathansKilledTotal)
                    total += kv.Value;

                return total;
            }
        }
        static int plantsRaisedTotal
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.plantsRaisedTotal)
                    total += kv.Value;

                return total;
            }
        }
        static int eggsHatchedTotal
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.eggsHatchedTotal)
                    total += kv.Value;

                return total;
            }
        }
        static float foodEaten
        {
            get
            {
                float total = 0;
                foreach (var kv in Main.config.foodEaten[saveSlot])
                    total += kv.Value;

                return total;
            }
        }
        static float foodEatenTotal
        {
            get
            {
                float total = 0;
                foreach (var kv in Main.config.foodEatenTotal)
                    total += kv.Value;

                return total;
            }
        }
        static int itemsCrafted
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.itemsCrafted[saveSlot])
                    total += kv.Value;

                return total;
            }
        }
        static int plantsKilled
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.plantsKilled[saveSlot])
                    total += kv.Value;

                return total;
            }
        }
        static int coralKilled
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.coralKilled[saveSlot])
                    total += kv.Value;

                return total;
            }
        }
        static int animalsKilled
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.animalsKilled[saveSlot])
                    total += kv.Value;

                return total;
            }
        }
        static int leviathansKilled
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.leviathansKilled[saveSlot])
                    total += kv.Value;

                return total;
            }
        }
        static int craftingResourcesUsed_
        {
            get
            {
                int total = 0;
                foreach (var crafted in Main.config.craftingResourcesUsed_[saveSlot])
                    total += crafted.Value;

                return total;
            }
        }
        static float craftingResourcesUsed
        {
            get
            {
                float total = 0;
                foreach (var crafted in Main.config.craftingResourcesUsed[saveSlot])
                    total += crafted.Value;

                return total;
            }
        }
        static int craftingResourcesUsedTotal_
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.craftingResourcesUsedTotal_)
                    total += kv.Value;

                return total;
            }
        }
        static float craftingResourcesUsedTotal
        {
            get
            {
                float total = 0;
                foreach (var kv in Main.config.craftingResourcesUsedTotal)
                    total += kv.Value;

                return total;
            }
        }
        static int plantsRaised
        {
            get
            {
                int total = 0;
                foreach (var r in Main.config.plantsRaised[saveSlot])
                    total += r.Value;

                return total;
            }
        }
        static int eggsHatched
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.eggsHatched[saveSlot])
                    total += kv.Value;

                return total;
            }
        }
        static bool introCinRunning = false;
        static int storedLifePod
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.storedLifePod[saveSlot])
                    total += kv.Value;

                return total;
            }
        }
        static int storedBase
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.storedBase[saveSlot])
                    total += kv.Value;

                return total;
            }
        }
        static int storedOutside
        {
            get
            {
                int total = 0;
                foreach (var kv in Main.config.storedOutside[saveSlot])
                    total += kv.Value;

                return total;
            }
        }

        public static string GetCraftingResourcesUsedTotal(string str)
        {
            if (Main.config.craftingResourcesUsedTotal.ContainsKey(str))
                return Language.main.Get(str) + " " + Main.config.craftingResourcesUsedTotal_[str] + ", " + Main.config.craftingResourcesUsedTotal[str].ToString("0.0") + " kg";
            else
                return Language.main.Get(str) + " " + Main.config.craftingResourcesUsedTotal_[str];
        }

        public static string GetCraftingResourcesUsed(string str)
        {
            if (Main.config.craftingResourcesUsed[saveSlot].ContainsKey(str))
                return Language.main.Get(str) + " " + Main.config.craftingResourcesUsed_[saveSlot][str] + ", " + Main.config.craftingResourcesUsed[saveSlot][str].ToString("0.0") + " kg";
            else
                return Language.main.Get(str) + " " + Main.config.craftingResourcesUsed_[saveSlot][str];
        }

        public static string GetBiomeName(string name)
        { //  Fissure IceSheet
            name = name.ToLower();
            if (name == "twistybridges_shallow")
                return "Shallow Twisty Bridges";
            else if (name == "twistybridges_deep")
                return "Deep Twisty Bridges";
            else if (name == "arctickelp")
                return "Arctic Kelp Forest";
            else if (name == "thermalspires" || name == "rocketarea")
                return "Thermal Spires";
            else if (name == "twistybridges")
                return "Twisty Bridges";
            else if (name == "sparsearctic" || name == "startzone")
                return "Sparse Arctic";
            else if (name == "tundravoid" || name == "worldedge")
                return "World Edge";
            else if (name == "purplevents")
                return "Purple Vents";
            else if (name == "purplevents_deep")
                return "Deep Purple Vents";
            else if (name.Contains("crystalcave")) // CrystalCave_Inner
                return "Crystal Caves";
            else if (name == "lilypads")
                return "Lilypad Islands";
            else if (name == "lilypads_deep")
                return "Deep Lilypads Cave";
            else if (name == "arcticspires")
                return "Arctic Spires";
            else if (name == "eastarctic" || name == "westarctic")
                return "Arctic";
            else if (name == "fabricatorcaverns")
                return "Fabricator Caverns";
            else if (name == "treespires")
                return "Tree Spires";
            else if (name == "glacialbay")
                return "Glacial Bay";
            else if (name == "glacialbasin" || name == "glacialbasin_underwater")
                return "Glacial Basin";

            return "unknown";
        }

        public static void AddPDAentry(string key, string name, string desc, string path)
        {
            //newPediaEntries.Add(key);
            string[] nodes = path.Split('/');
            PDAEncyclopedia.EntryData entry = new PDAEncyclopedia.EntryData()
            {
                //path = path,
                key = key,
                nodes = nodes
            };
            PDAEncyclopediaHandler.AddCustomEntry(entry);
            //mapping[key] = entry;
            myStrings[key] = desc;
            descs["EncyDesc_" + key] = desc;
            LanguageHandler.SetLanguageLine("Ency_" + key, name);
            LanguageHandler.SetLanguageLine("EncyDesc_" + key, desc);
            //PDAEncyclopedia.Add(entryData.encyclopedia, false);
        }

        public static TimeSpan GetTimePlayed()
        {
            if (!Main.setupDone || DayNightCycle.main.timePassedSinceOrigin < 350f)
                return new TimeSpan(0, 0, 0);

            if (!Main.config.timeGameStarted.ContainsKey(saveSlot))
            {
                Main.config.timeGameStarted[saveSlot] = DayNightCycle.main.timePassedSinceOrigin;
                //AddDebug("save timeGameStarted " + Main.config.timeGameStarted[saveSlot]);
                //Main.Log("save timeGameStarted " + Main.config.timeGameStarted[saveSlot]);
            }
            //else if(Main.config.timeGameStarted.ContainsKey(saveSlot))
            //    Main.Log("timePassedSinceOrigin " + DayNightCycle.main.timePassedSinceOrigin);

            return new TimeSpan(0, 0, Mathf.FloorToInt((DayNightCycle.main.timePassedSinceOrigin - Main.config.timeGameStarted[saveSlot]) * 72f));
        }

        public static string GetTraveledString(int meters)
        {
            //AddDebug(" metersToKM " + meters);
            int km = 0;
            string s = "";
            for (int i = meters; i > 1000; i -= 1000)
            {
                km++;
                meters -= 1000;
            }
            //for (int i = 0; i < length; i++)
            if (km > 0)
                s += km + " km, ";

            s += meters + " meters";
            //AddDebug(" metersToKM " + km + " " + meters);
            return s;
        }

        public static void AddEntries()
        {
            //LanguageHandler.SetLanguageLine("EncyPath_", "Statistics");
            //AddPDAentry("Stats", "Statistics", "", "Stats");
            AddPDAentry("StatsGlobal", "Global statistics", "", "Stats");
            AddPDAentry("StatsThisGame", "Current game statistics", "_qwe_", "Stats");
            LanguageHandler.SetLanguageLine("EncyPath_Stats", "Statistics");
        }

        [HarmonyPatch(typeof(Language), "TryGet")]
        internal class Language_TryGet_Patch
        {
            public static void Postfix(Language __instance, string key, ref string result)
            {
                if (!Main.setupDone || descs == null || key == null || !descs.ContainsKey(key))
                    return;

                if (key == "EncyDesc_StatsThisGame")
                {
                    string biomeName = GetBiomeName(LargeWorld.main.GetBiome(Player.main.transform.position));
                    result = "Current biome: " + biomeName + "\n\n";
                    //AddDebug(" " + biomeName);
                    result += timePlayed;
                    if (GameModeManager.GetOption<float>(GameOption.PlayerDamageTakenModifier) > 0 && !GameModeManager.GetOption<bool>(GameOption.PermanentDeath))
                    {
                        if (Main.config.playerDeaths[saveSlot] > 0)
                            result += "\nDeaths: " + Main.config.playerDeaths[saveSlot];
                    }

                    result += "\n\nTime spent on feet: " + Main.config.timeWalked[saveSlot].Days + " days, " + Main.config.timeWalked[saveSlot].Hours + " hours, " + Main.config.timeWalked[saveSlot].Minutes + " minutes.";
                    result += "\nTime spent swimming: " + Main.config.timeSwam[saveSlot].Days + " days, " + Main.config.timeSwam[saveSlot].Hours + " hours, " + Main.config.timeSwam[saveSlot].Minutes + " minutes.";
                    result += "\nTime spent sleeping: " + Main.config.timeSlept[saveSlot].Days + " days, " + Main.config.timeSlept[saveSlot].Hours + " hours, " + Main.config.timeSlept[saveSlot].Minutes + " minutes.";
                    result += "\nTime spent in your drop pod: " + Main.config.timeEscapePod[saveSlot].Days + " days, " + Main.config.timeEscapePod[saveSlot].Hours + " hours, " + Main.config.timeEscapePod[saveSlot].Minutes + " minutes.";
                    if (Main.config.baseRoomsBuilt[saveSlot] > 0 || Main.config.baseCorridorsBuilt[saveSlot] > 0)
                        result += "\nTime spent in your base: " + Main.config.timeBase[saveSlot].Days + " days, " + Main.config.timeBase[saveSlot].Hours + " hours, " + Main.config.timeBase[saveSlot].Minutes + " minutes.";
                    if (Main.config.snowfoxesBuilt[saveSlot] > 0)
                        result += "\nTime spent riding snowfox: " + Main.config.timeSnowfox[saveSlot].Days + " days, " + Main.config.timeSnowfox[saveSlot].Hours + " hours, " + Main.config.timeSnowfox[saveSlot].Minutes + " minutes.";
                    if (Main.config.exosuitsBuilt[saveSlot] > 0)
                        result += "\nTime spent piloting prawn suit: " + Main.config.timeExosuit[saveSlot].Days + " days, " + Main.config.timeExosuit[saveSlot].Hours + " hours, " + Main.config.timeExosuit[saveSlot].Minutes + " minutes.";
                    if (Main.config.seatrucksBuilt[saveSlot] > 0)
                        result += "\nTime spent piloting seatruck: " + Main.config.timeSeatruck[saveSlot].Days + " days, " + Main.config.timeSeatruck[saveSlot].Hours + " hours, " + Main.config.timeSeatruck[saveSlot].Minutes + " minutes.";
                  
                    result += "\n\nDistance traveled: " + Main.config.distanceTraveled[saveSlot] + " meters.";
                    result += "\nDistance traveled by swimming: " + Main.config.distanceTraveledSwim[saveSlot] + " meters.";
                    result += "\nDistance traveled on foot: " + Main.config.distanceTraveledWalk[saveSlot] + " meters.";
                    result += "\nDistance traveled by seaglide: " + Main.config.distanceTraveledSeaglide[saveSlot] + " meters.";

                    if (Main.config.snowfoxesBuilt[saveSlot] > 0)
                        result += "\nDistance traveled by snowfox: " + Main.config.distanceTraveledSnowfox[saveSlot] + " meters.";
                    if (Main.config.exosuitsBuilt[saveSlot] > 0)
                        result += "\nDistance traveled in prawn suit: " + Main.config.distanceTraveledExosuit[saveSlot] + " meters.";
                    if (Main.config.seatrucksBuilt[saveSlot] > 0)
                        result += "\nDistance traveled in seatruck: " + Main.config.distanceTraveledSeatruck[saveSlot] + " meters.";
                    if (Main.config.distanceTraveledCreature[saveSlot] > 0)
                        result += "\nDistance traveled by riding creatures: " + Main.config.distanceTraveledCreature[saveSlot] + " meters.";
                    result += "\nMax depth reached: " + Main.config.maxDepth[saveSlot] + " meters.";

                    if (Main.config.exosuitsBuilt[saveSlot] > 0 || Main.config.seatrucksBuilt[saveSlot] > 0)
                        result += "\n";
                    if (Main.config.seatrucksBuilt[saveSlot] > 0)
                        result += "\nSeatrucks constructed: " + Main.config.seatrucksBuilt[saveSlot];
                    if (Main.config.seatrucksModulesBuilt[saveSlot] > 0)
                        result += "\nSeatruck modules constructed: " + Main.config.seatrucksModulesBuilt[saveSlot];
                    if (Main.config.exosuitsBuilt[saveSlot] > 0)
                        result += "\nPrawn suits constructed: " + Main.config.exosuitsBuilt[saveSlot];
                    if (Main.config.snowfoxesBuilt[saveSlot] > 0)
                        result += "\nSnofoxes constructed: " + Main.config.snowfoxesBuilt[saveSlot];
                    if (Main.config.seatrucksLost[saveSlot] > 0)
                        result += "\nSeatrucks lost: " + Main.config.seatrucksLost[saveSlot];
                    if (Main.config.seatruckModulesLost[saveSlot] > 0)
                        result += "\nSeatruck modules lost: " + Main.config.seatruckModulesLost[saveSlot];
                    if (Main.config.exosuitsLost[saveSlot] > 0)
                        result += "\nPrawn suits lost: " + Main.config.exosuitsLost[saveSlot];
                    if (Main.config.snowfoxesLost[saveSlot] > 0)
                        result += "\nSnofoxes lost: " + Main.config.snowfoxesLost[saveSlot];

                    if(GameModeManager.GetOption<float>(GameOption.PlayerDamageTakenModifier) > 0)
                    {
                        result += "\n\nHealth lost: " + Main.config.healthLost[saveSlot];
                        if (Main.config.medkitsUsed[saveSlot] > 0)
                            result += "\nFirst aid kits used: " + Main.config.medkitsUsed[saveSlot];
                    }

                    if (GameModeManager.GetOption<bool>(GameOption.Thirst))
                    {
                        result += "\n\nWater drunk: " + Main.config.waterDrunk[saveSlot] + " liters.";
                    }
                    if (GameModeManager.GetOption<bool>(GameOption.Hunger))
                    {
                        result += "\nFood eaten: " + foodEaten + " kg.";
                        foreach (var kv in Main.config.foodEaten[saveSlot])
                            result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value + " kg.";
                    }

                    if (Main.config.baseRoomsBuilt[saveSlot] > 0 || Main.config.baseCorridorsBuilt[saveSlot] > 0)
                        result += "\n\nTotal power generated for your bases: " + basePower;
                    if (Main.config.baseCorridorsBuilt[saveSlot] > 0)
                        result += "\nBase corridor segments built: " + Main.config.baseCorridorsBuilt[saveSlot];
                    if (Main.config.baseRoomsBuilt[saveSlot] > 0)
                        result += "\nBase rooms built: " + Main.config.baseRoomsBuilt[saveSlot];
                    //foreach (var kv in Main.config.baseRoomsBuilt[saveSlot])
                    //    result += "\n      " + Language.main.Get(kv.Key.AsString()) + " " + kv.Value;
              
                    if (Main.config.objectsScanned[saveSlot] > 0 || Main.config.blueprintsFromDatabox[saveSlot] > 0 || Main.config.blueprintsUnlocked[saveSlot] > 0)
                        result += "\n";
                    if (Main.config.objectsScanned[saveSlot] > 0)
                        result += "\nObjects scanned: " + Main.config.objectsScanned[saveSlot];
                    if (Main.config.blueprintsUnlocked[saveSlot] > 0)
                        result += "\nBlueprints unlocked by scanning: " + Main.config.blueprintsUnlocked[saveSlot];
                    if (Main.config.blueprintsFromDatabox[saveSlot] > 0)
                        result += "\nBlueprints found in databoxes: " + Main.config.blueprintsFromDatabox[saveSlot];
              
                    if (plantsKilled > 0)
                        result += "\n\nPlants killed: " + plantsKilled;
                    foreach (var kv in Main.config.plantsKilled[saveSlot])
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
                
                    if (animalsKilled > 0)
                        result += "\n\nAnimals killed: " + animalsKilled;
                    foreach (var kv in Main.config.animalsKilled[saveSlot])
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;

                    if (coralKilled > 0)
                        result += "\n\nCorals killed: " + coralKilled;
                    foreach (var kv in Main.config.coralKilled[saveSlot])
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;

                    if (leviathansKilled > 0)
                        result += "\n\nLeviathans killed: " + leviathansKilled;
                    foreach (var kv in Main.config.leviathansKilled[saveSlot])
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
   
                    result += "\n\nItems crafted: " + itemsCrafted;
                    foreach (var kv in Main.config.itemsCrafted[saveSlot])
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;

                    result += "\n\nResources used for crafting and constructing: " + craftingResourcesUsed.ToString("0.0") + " kg";
                    foreach (var kv in Main.config.craftingResourcesUsed_[saveSlot])
                        result += "\n      " + GetCraftingResourcesUsed(kv.Key);

                    if (plantsRaised > 0)
                        result += "\n\nPlants raised: " + plantsRaised;
                    foreach (var kv in Main.config.plantsRaised[saveSlot])
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
      
                    if (eggsHatched > 0)
                    {
                        result += "\n\nEggs hatched in AC: " + eggsHatched;
                        foreach (var kv in Main.config.eggsHatched[saveSlot])
                            result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
                    }

                    if (storedLifePod > 0)
                    {
                        result += "\n\nThings stored in your drop pod: ";
                        foreach (var kv in Main.config.storedLifePod[saveSlot])
                        {
                            if (kv.Value > 0)
                                result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
                        }
                    }
                    if (storedBase > 0)
                    {
                        result += "\n\nThings stored in your bases: ";
                        foreach (var kv in Main.config.storedBase[saveSlot])
                        {
                            if (kv.Value > 0)
                                result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
                        }
                    }
                    if (Main.config.seatrucksBuilt[saveSlot] > 0)
                    {
                        result += "\n\nThings stored in seatruck: ";
                        foreach (var kv in Main.config.storedSeatruck[saveSlot])
                        {
                            if (kv.Value > 0)
                                result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
                        }
                    }
                    if (storedOutside > 0)
                    {
                        result += "\n\nThings stored outside your bases: ";
                        foreach (var kv in Main.config.storedOutside[saveSlot])
                        {
                            if (kv.Value > 0)
                                result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
                        }
                    }

                    result += "\n\nBiomes discovered:";
                    foreach (string biome in Main.config.biomesFound[saveSlot])
                        result += "\n      " + biome;

                    if (Main.config.faunaFound[saveSlot].Count > 0)
                        result += "\n\nFauna species discovered: ";
                    foreach (string name in Main.config.faunaFound[saveSlot])
                        result += "\n      " + Language.main.Get(name);
        
                    if (Main.config.floraFound[saveSlot].Count > 0)
                        result += "\n\nFlora species discovered: ";
                    foreach (string name in Main.config.floraFound[saveSlot])
                        result += "\n      " + Language.main.Get(name);

                    if (Main.config.coralFound[saveSlot].Count > 0)
                        result += "\n\nCoral species discovered: ";
                    foreach (string name in Main.config.coralFound[saveSlot])
                        result += "\n      " + Language.main.Get(name);

                    if (Main.config.leviathanFound[saveSlot].Count > 0)
                        result += "\n\nLeviathan species discovered: ";
                    foreach (string name in Main.config.leviathanFound[saveSlot])
                        result += "\n      " + Language.main.Get(name);
                }

                else if (key == "EncyDesc_StatsGlobal")
                {
                    result = timePlayedTotal;
                    if (Main.config.gamesWon > 0)
                        result += "\nGames completed " + Main.config.gamesWon;
                    result += "\nDeaths: " + Main.config.playerDeathsTotal;
                    //result += "\nHealth lost: " + healthLostTotal;

                    result += "\n\nTime spent on feet: " + Main.config.timeWalkedTotal.Days + " days, " + Main.config.timeWalkedTotal.Hours + " hours, " + Main.config.timeWalkedTotal.Minutes + " minutes.";
                    result += "\nTime spent swimming: " + Main.config.timeSwamTotal.Days + " days, " + Main.config.timeSwamTotal.Hours + " hours, " + Main.config.timeSwamTotal.Minutes + " minutes.";
                    result += "\nTime spent sleeping: " + Main.config.timeSleptTotal.Days + " days, " + Main.config.timeSleptTotal.Hours + " hours, " + Main.config.timeSleptTotal.Minutes + " minutes.";
                    result += "\nTime spent in your drop pod: " + Main.config.timeEscapePodTotal.Days + " days, " + Main.config.timeEscapePodTotal.Hours + " hours, " + Main.config.timeEscapePodTotal.Minutes + " minutes.";
                    result += "\nTime spent in your base: " + Main.config.timeBaseTotal.Days + " days, " + Main.config.timeBaseTotal.Hours + " hours, " + Main.config.timeBaseTotal.Minutes + " minutes.";
                    result += "\nTime spent riding snowfox: " + Main.config.timeSnowfoxTotal.Days + " days, " + Main.config.timeSnowfoxTotal.Hours + " hours, " + Main.config.timeSnowfoxTotal.Minutes + " minutes.";
                    result += "\nTime spent piloting prawn suit: " + Main.config.timeExosuitTotal.Days + " days, " + Main.config.timeExosuitTotal.Hours + " hours, " + Main.config.timeExosuitTotal.Minutes + " minutes.";
                    result += "\nTime spent piloting seatruck: " + Main.config.timeSeatruckTotal.Days + " days, " + Main.config.timeSeatruckTotal.Hours + " hours, " + Main.config.timeSeatruckTotal.Minutes + " minutes.";

                    result += "\n\nDistance traveled: " + GetTraveledString(Main.config.distanceTraveledTotal);
                    result += "\nDistance traveled on foot: " + Main.config.distanceTraveledWalkTotal + " meters.";
                    result += "\nDistance traveled by swimming: " + Main.config.distanceTraveledSwimTotal + " meters.";
                    result += "\nDistance traveled by seaglide: " + Main.config.distanceTraveledSeaglideTotal + " meters.";
                    result += "\nDistance traveled by snowfox: " + Main.config.distanceTraveledSnowfoxTotal + " meters.";
                    result += "\nDistance traveled in prawn suit: " + Main.config.distanceTraveledExosuitTotal + " meters.";
                    result += "\nDistance traveled in seatruck: " + Main.config.distanceTraveledSeatruckTotal + " meters.";
                    result += "\nDistance traveled by riding creatures: " + Main.config.distanceTraveledCreatureTotal + " meters.";
                    result += "\nMax depth reached: " + Main.config.maxDepthGlobal + " meters.";

                    result += "\n\nSeatrucks constructed: " + Main.config.seatrucksBuiltTotal;
                    result += "\nSeatrucks lost: " + Main.config.seatrucksLostTotal;
                    result += "\nSeatruck modules constructed: " + Main.config.seatruckModulesBuiltTotal;
                    result += "\nSeatruck modules lost: " + Main.config.seatruckModulesLostTotal;
                    result += "\nSnowfoxes constructed: " + Main.config.snowfoxesBuiltTotal;
                    result += "\nSnowfoxes lost: " + Main.config.snowfoxesLostTotal;
                    result += "\nPrawn suits constructed: " + Main.config.exosuitsBuiltTotal;
                    result += "\nPrawn suits lost: " + Main.config.exosuitsLostTotal;
                 
                    result += "\n\nHealth lost: " + Main.config.healthLostTotal;
                    result += "\nFirst aid kits used: " + Main.config.medkitsUsedTotal;
               
                    result += "\n\nWater drunk: " + Main.config.waterDrunkTotal + " liters.";
                    result += "\nFood eaten: " + foodEatenTotal + " kg.";
                    foreach (var kv in Main.config.foodEatenTotal)
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value + " kg.";
              
                    result += "\n\nTotal power generated for your bases: " + basePowerTotal;
                    result += "\nBase corridor segments built: " + Main.config.baseCorridorsBuiltTotal;
                    result += "\nBase rooms built: " + Main.config.baseRoomsBuiltTotal;
                    //foreach (var kv in Main.config.baseRoomsBuiltTotal)
                    //    result += "\n      " + Language.main.Get(kv.Key.AsString()) + " " + kv.Value;

                    result += "\n\nThings scanned: " + objectsScannedTotal;
                    result += "\nBlueprints unlocked by scanning: " + blueprintsUnlockedTotal;
                    result += "\nBlueprints found in databoxes: " + blueprintsFromDataboxesTotal;
               
                    result += "\n\nPlants killed: " + plantsKilledTotal;
                    foreach (var kv in Main.config.plantsKilledTotal)
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;

                    result += "\n\nAnimals killed: " + animalsKilledTotal;
                    foreach (var kv in Main.config.animalsKilledTotal)
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;

                    result += "\n\nCorals killed: " + coralKilledTotal;
                    foreach (var kv in Main.config.coralKilledTotal)
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;

                    result += "\n\nLeviathans killed: " + leviathansKilledTotal;
                    foreach (var kv in Main.config.leviathansKilledTotal)
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;

                    result += "\n\nItems crafted: " + itemsCraftedTotal;
                    foreach (var kv in Main.config.itemsCraftedTotal)
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;

                    result += "\n\nResources used for crafting and constructing: " + craftingResourcesUsedTotal.ToString("0.0") + " kg";
                    foreach (var kv in Main.config.craftingResourcesUsedTotal_)
                        result += "\n      " + GetCraftingResourcesUsedTotal(kv.Key);

                    result += "\n\nPlants raised: " + plantsRaisedTotal;
                    foreach (var kv in Main.config.plantsRaisedTotal)
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;

                    result += "\n\nEggs hatched in AC: " + eggsHatchedTotal;
                    foreach (var kv in Main.config.eggsHatchedTotal)
                        result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;

                    result += "\n\nThings stored in your drop pods: ";
                    foreach (var kv in Main.config.storedLifePodTotal)
                    {
                        if (kv.Value > 0)
                            result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
                    }
                    result += "\n\nThings stored in your bases: ";
                    foreach (var kv in Main.config.storedBaseTotal)
                    {
                        if (kv.Value > 0)
                            result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
                    }
                    result += "\n\nThings stored in seatrucks: ";
                    foreach (var kv in Main.config.storedSeatruckTotal)
                    {
                        if (kv.Value > 0)
                            result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
                    }
                    result += "\n\nThings stored outside your bases: ";
                    foreach (var kv in Main.config.storedOutsideTotal)
                    {
                        if (kv.Value > 0)
                            result += "\n      " + Language.main.Get(kv.Key) + " " + kv.Value;
                    }
                    result += "\n\nBiomes discovered:";
                    foreach (string biome in Main.config.biomesFoundGlobal)
                        result += "\n      " + biome;

                    result += "\n\nFauna species discovered: ";
                    foreach (string name in Main.config.faunaFoundTotal)
                        result += "\n      " + Language.main.Get(name);
    
                    result += "\n\nFlora species discovered: ";
                    foreach (string name in Main.config.floraFoundTotal)
                        result += "\n      " + Language.main.Get(name);

                    result += "\n\nCoral species discovered: ";
                    foreach (string name in Main.config.coralFoundTotal)
                        result += "\n      " + Language.main.Get(name);

                    result += "\n\nLeviathan species discovered: ";
                    foreach (string name in Main.config.leviathanFoundTotal)
                        result += "\n      " + Language.main.Get(name);
                }
            }
        }

        [HarmonyPatch(typeof(ExpansionIntroManagerV2))]
        internal class ExpansionIntroManagerV2_Patch
        {
            [HarmonyPatch("Start")]
            [HarmonyPostfix]
            public static void CinematicStartPostfix(ExpansionIntroManagerV2 __instance)
            {
                //AddDebug("Start");
                introCinRunning = true;
            }
            [HarmonyPatch("OnCinematicEnded")]
            [HarmonyPostfix]
            public static void OnCinematicEndedPostfix(ExpansionIntroManagerV2 __instance)
            {
                //AddDebug("OnCinematicEnded");
                introCinRunning = false;
            }
        }

        [HarmonyPatch(typeof(BelowZeroEndGame), "ShowCredits")]
        internal class BelowZeroEndGame_ShowCredits_Patch
        {
            public static void Postfix(BelowZeroEndGame __instance)
            {
                //AddDebug("BelowZeroEndGame ShowCredits ");
                Main.config.gamesWon++;
                Main.config.Save();
            }
        }

        [HarmonyPatch(typeof(Player), "OnKill")]
        internal class Player_OnKill_Patch
        {
            public static void Postfix(Player __instance)
            {
                if(!GameModeManager.GetOption<bool>(GameOption.PermanentDeath))
                    Main.config.playerDeaths[saveSlot]++;

                Main.config.deathsTotal++;
            }
        }

        [HarmonyPatch(typeof(DamageSystem), "CalculateDamage", new Type[] { typeof(TechType), typeof(float), typeof(float), typeof(DamageType), typeof(GameObject), typeof(GameObject) })]
        class DamageSystem_CalculateDamage_Patch
        {
            public static void Postfix(DamageSystem __instance, float damage, DamageType type, GameObject target, GameObject dealer, ref float __result, TechType techType)
            {
                if (__result > 0f && techType == TechType.Player)
                {
                    int dam = Mathf.RoundToInt(__result);
                    //AddDebug("Player takes damage " + dam);   
                    Main.config.healthLost[saveSlot] += dam;
                    Main.config.healthLostTotal += dam;
                }
            }
        }

        [HarmonyPatch(typeof(Survival))]
        class Survival_Eat_Patch
        {
            [HarmonyPostfix]
            [HarmonyPatch("Eat")]
            public static void EatPostfix(Survival __instance, GameObject useObj, bool __result)
            {
                if (__result)
                {
                    Eatable eatable = useObj.GetComponent<Eatable>();
                    Rigidbody rb = useObj.GetComponent<Rigidbody>();
                    if (eatable && rb)
                    {
                        float foodValue = eatable.GetFoodValue();
                        float waterValue = eatable.GetWaterValue();
                        TechType tt = CraftData.GetTechType(useObj);
                        string name = tt.AsString();

                        if (foodValue >= waterValue)
                        {
                            if (Main.config.foodEaten[saveSlot].ContainsKey(name))
                                Main.config.foodEaten[saveSlot][name] += rb.mass;
                            else
                                Main.config.foodEaten[saveSlot][name] = rb.mass;

                            if (Main.config.foodEatenTotal.ContainsKey(name))
                                Main.config.foodEatenTotal[name] += rb.mass;
                            else
                                Main.config.foodEatenTotal[name] = rb.mass;
                        }
                        else if (waterValue > foodValue)
                        {
                            Main.config.waterDrunk[saveSlot] += rb.mass;
                            Main.config.waterDrunkTotal += rb.mass;
                        }
                        if (fauna.Contains(tt))
                        {
                            //AddDebug(tt + " animal killed by player");
                            LiveMixin lm = useObj.GetComponent<LiveMixin>();
                            if (lm && lm.IsAlive())
                            {
                                if (Main.config.animalsKilledTotal.ContainsKey(name))
                                    Main.config.animalsKilledTotal[name]++;
                                else
                                    Main.config.animalsKilledTotal[name] = 1;

                                if (Main.config.animalsKilled[saveSlot].ContainsKey(name))
                                    Main.config.animalsKilled[saveSlot][name]++;
                                else
                                    Main.config.animalsKilled[saveSlot][name] = 1;
                            }
                        }
                    }
                }
            }
            [HarmonyPostfix]
            [HarmonyPatch("Use")]
            public static void UsePostfix(Survival __instance, GameObject useObj, bool __result)
            {
                if (!__result)
                    return;

                TechType tt = CraftData.GetTechType(useObj);
                if (tt == TechType.FirstAidKit)
                {
                    //AddDebug("medkit used");
                    Main.config.medkitsUsed[saveSlot]++;
                    Main.config.medkitsUsedTotal++;
                }
            }
        }

        [HarmonyPatch(typeof(Player), "TrackTravelStats")]
        internal class Player_TrackTravelStats_Patch
        {
            public static bool Prefix(Player __instance)
            {
                //AddDebug("motorMode " + __instance.motorMode);
                //AddDebug("IsUnderwaterForSwimming " + __instance.IsUnderwaterForSwimming());
                //AddDebug("inHovercraft " + __instance.inHovercraft);
                //AddDebug("mode " + __instance.mode);
                //AddDebug("GetTimePlayed " + GetTimePlayed());
                if (!Main.setupDone || introCinRunning)
                    return false;
                // snowfox MotorMode.Vehicle mode.LockedPiloting
                Vector3 position = __instance.transform.position;

                __instance.maxDepth = Mathf.Max(__instance.maxDepth, -position.y);
                Main.config.maxDepth[saveSlot] = (int)__instance.maxDepth;
                Main.config.maxDepthGlobal = (int)__instance.maxDepth;
                string biomeName = GetBiomeName(LargeWorld.main.GetBiome(Player.main.transform.position));
                if (biomeName != "unknown")
                {
                    Main.config.biomesFound[saveSlot].Add(biomeName);
                    Main.config.biomesFoundGlobal.Add(biomeName);
                }
                __instance.distanceTraveled += Vector3.Distance(position, __instance.lastPosition);
                int distanceTraveled = Mathf.RoundToInt((__instance.lastPosition - position).magnitude);
                Main.config.distanceTraveled[saveSlot] += distanceTraveled;
                Main.config.distanceTraveledTotal += distanceTraveled;

                if (travelMode == TravelMode.CreatureRide && __instance.motorMode == Player.MotorMode.CreatureRide)
                {
                    Main.config.distanceTraveledCreature[saveSlot] += distanceTraveled;
                    Main.config.distanceTraveledCreatureTotal += distanceTraveled;
                }
                else if (travelMode == TravelMode.Seaglide && __instance.motorMode == Player.MotorMode.Seaglide)
                {
                    Main.config.distanceTraveledSeaglide[saveSlot] += distanceTraveled;
                    Main.config.distanceTraveledSeaglideTotal += distanceTraveled;
                }
                else if (travelMode == TravelMode.Exosuit && __instance.inExosuit)
                {
                    Main.config.distanceTraveledExosuit[saveSlot] += distanceTraveled;
                    Main.config.distanceTraveledExosuitTotal += distanceTraveled;
                }
                else if (travelMode == TravelMode.Seatruck && __instance.inSeatruckPilotingChair)
                {
                    Main.config.distanceTraveledSeatruck[saveSlot] += distanceTraveled;
                    Main.config.distanceTraveledSeatruckTotal += distanceTraveled;
                }
                else if (travelMode == TravelMode.Snowfox && __instance.inHovercraft)
                {
                    Main.config.distanceTraveledSnowfox[saveSlot] += distanceTraveled;
                    Main.config.distanceTraveledSnowfoxTotal += distanceTraveled;
                }
                else if (travelMode == TravelMode.Walk && __instance.motorMode == Player.MotorMode.Run)
                {
                    Main.config.distanceTraveledWalk[saveSlot] += distanceTraveled;
                    Main.config.distanceTraveledWalkTotal += distanceTraveled;
                }
                else if (travelMode == TravelMode.Swim && __instance.IsUnderwaterForSwimming())
                {
                    Main.config.distanceTraveledSwim[saveSlot] += distanceTraveled;
                    Main.config.distanceTraveledSwimTotal += distanceTraveled;
                }
                __instance.lastPosition = position;
                TimeSpan updatePeriod = GetTimePlayed() - timeLastUpdate;

                if (__instance.motorMode == Player.MotorMode.CreatureRide)
                    travelMode = TravelMode.CreatureRide;
                else if(__instance.motorMode == Player.MotorMode.Seaglide)
                    travelMode = TravelMode.Seaglide;
                else if (__instance.inSeatruckPilotingChair)
                {
                    travelMode = TravelMode.Seatruck;
                    Main.config.timeSeatruck[saveSlot] += updatePeriod;
                    Main.config.timeSeatruckTotal += updatePeriod;
                }
                else if (__instance.inExosuit)
                {
                    travelMode = TravelMode.Exosuit;
                    Main.config.timeExosuit[saveSlot] += updatePeriod;
                    Main.config.timeExosuitTotal += updatePeriod;
                }
                else if (__instance.inHovercraft)
                {
                    travelMode = TravelMode.Snowfox;
                    Main.config.timeSnowfox[saveSlot] += updatePeriod;
                    Main.config.timeSnowfoxTotal += updatePeriod;
                }
                else if (Player.main.IsUnderwaterForSwimming())
                { // MotorMode.Run when swimming on surface
                    travelMode = TravelMode.Swim;
                    Main.config.timeSwam[saveSlot] += updatePeriod;
                    Main.config.timeSwamTotal += updatePeriod;
                }
                else if (__instance.motorMode == Player.MotorMode.Run)
                {
                    travelMode = TravelMode.Walk;
                    Main.config.timeWalked[saveSlot] += updatePeriod;
                    Main.config.timeWalkedTotal += updatePeriod;
                }
                if (__instance.currentInterior is LifepodDrop)
                {
                    Main.config.timeEscapePod[saveSlot] += updatePeriod;
                    Main.config.timeEscapePodTotal += updatePeriod;
                }
                else if(__instance.IsInBase())
                {
                    Main.config.timeBase[saveSlot] += updatePeriod;
                    Main.config.timeBaseTotal += updatePeriod;
                }
                timeLastUpdate = GetTimePlayed();
                return false;
            }
        }

        [HarmonyPatch(typeof(CreatureEgg), "Hatch")]
        internal class CreatureEgg_Hatch_Patch
        {
            public static void Postfix(CreatureEgg __instance)
            {
                if (__instance.creatureType == TechType.None)
                    return;

                string name = __instance.creatureType.AsString();

                if (Main.config.eggsHatchedTotal.ContainsKey(name))
                    Main.config.eggsHatchedTotal[name]++;
                else
                    Main.config.eggsHatchedTotal[name] = 1;

                if (Main.config.eggsHatched[saveSlot].ContainsKey(name))
                    Main.config.eggsHatched[saveSlot][name]++;
                else
                    Main.config.eggsHatched[saveSlot][name] = 1;
            }
        }

        [HarmonyPatch(typeof(GrowingPlant), "SpawnGrownModel")]
        internal class GrowingPlant_SpawnGrownModel_Patch
        {
            public static void Postfix(GrowingPlant __instance)
            {
                if (string.IsNullOrEmpty(saveSlot))
                    return;

                TechType tt = __instance.plantTechType;
                //AddDebug("SpawnGrownModel " + tt);
                if (tt == TechType.None)
                    return;

                string name = tt.AsString();

                if (Main.config.plantsRaisedTotal.ContainsKey(name))
                    Main.config.plantsRaisedTotal[name]++;
                else
                    Main.config.plantsRaisedTotal[name] = 1;

                if (Main.config.plantsRaised[saveSlot].ContainsKey(name))
                    Main.config.plantsRaised[saveSlot][name]++;
                else
                    Main.config.plantsRaised[saveSlot][name] = 1;
            }
        }

        [HarmonyPatch(typeof(LiveMixin), "Kill")]
        internal class LiveMixin_Kill_Patch
        {
            public static void Prefix(LiveMixin __instance)
            {
                //TechType tt = CraftData.GetTechType(__instance.gameObject);
                //AddDebug("Kill " + tt);
                killedLM = __instance;
            }
        }

        [HarmonyPatch(typeof(LiveMixin), "TakeDamage")]
        internal class LiveMixin_TakeDamage_Patch
        { // Tweaks&Fixes overwrites TakeDamage so cant use Prefix
            public static void Postfix(LiveMixin __instance, float originalDamage, Vector3 position, DamageType type, GameObject dealer)
            {
                if (dealer && killedLM && __instance.Equals(killedLM))
                {
                    if (dealer.Equals(Player.main.gameObject) || dealer.GetComponent<SeaTruckSegment>())
                    {
                        TechType tt = CraftData.GetTechType(__instance.gameObject);
                        if (tt == TechType.None)
                            return;
                        //AddDebug(tt + " killed by player");
                        string name = tt.AsString();

                        if (fauna.Contains(tt))
                        {
                            //AddDebug(tt + " animal killed by player");
                            if (Main.config.animalsKilledTotal.ContainsKey(name))
                                Main.config.animalsKilledTotal[name]++;
                            else
                                Main.config.animalsKilledTotal[name] = 1;

                            if (Main.config.animalsKilled[saveSlot].ContainsKey(name))
                                Main.config.animalsKilled[saveSlot][name]++;
                            else
                                Main.config.animalsKilled[saveSlot][name] = 1;
                        }
                        else if (flora.Contains(tt))
                        {
                            //AddDebug(tt + " plant killed by player");
                            if (Main.config.plantsKilledTotal.ContainsKey(name))
                                Main.config.plantsKilledTotal[name]++;
                            else
                                Main.config.plantsKilledTotal[name] = 1;

                            if (Main.config.plantsKilled[saveSlot].ContainsKey(name))
                                Main.config.plantsKilled[saveSlot][name]++;
                            else
                                Main.config.plantsKilled[saveSlot][name] = 1;
                        }
                        else if (coral.Contains(tt))
                        {
                            //AddDebug(tt + " coral killed by player");
                            if (Main.config.coralKilledTotal.ContainsKey(name))
                                Main.config.coralKilledTotal[name]++;
                            else
                                Main.config.coralKilledTotal[name] = 1;

                            if (Main.config.coralKilled[saveSlot].ContainsKey(name))
                                Main.config.coralKilled[saveSlot][name]++;
                            else
                                Main.config.coralKilled[saveSlot][name] = 1;
                        }
                        else if (leviathans.Contains(tt))
                        {
                            //AddDebug(tt + " leviathan killed by player");
                            if (Main.config.leviathansKilled[saveSlot].ContainsKey(name))
                                Main.config.leviathansKilled[saveSlot][name]++;
                            else
                                Main.config.leviathansKilled[saveSlot][name] = 1;

                            if (Main.config.leviathansKilledTotal.ContainsKey(name))
                                Main.config.leviathansKilledTotal[name]++;
                            else
                                Main.config.leviathansKilledTotal[name] = 1;
                        }
                    }
                }
                killedLM = null;
            }
        }

        [HarmonyPatch(typeof(Inventory))]
        internal class Inventory_Patch
        { // runs when you have enough ingredients
            [HarmonyPrefix]
            [HarmonyPatch("ConsumeResourcesForRecipe")]
            public static void ConsumeResourcesForRecipePrefix(Inventory __instance, TechType techType)
            {
                ReadOnlyCollection<Ingredient> ingredients = TechData.GetIngredients(techType);
                if (ingredients == null)
                    return;
                removeIngredient = true;
            }
            [HarmonyPostfix]
            [HarmonyPatch("ConsumeResourcesForRecipe")]
            public static void ConsumeResourcesForRecipePostfix(Inventory __instance, TechType techType)
            {
                removeIngredient = false;
            }
            [HarmonyPostfix]
            [HarmonyPatch("OnRemoveItem")]
            public static void OnRemoveItemPostfix(InventoryItem item)
            {
                if (removeIngredient || constructing)
                { 
                    LiveMixin liveMixin = item.item.GetComponent<LiveMixin>();
                    Rigidbody rb = item.item.GetComponent<Rigidbody>();
                    TechType tt = item.item.GetTechType();
                    string name = tt.AsString();
                    bool alive = liveMixin && liveMixin.IsAlive();

                    if (alive || liveMixin == null)
                    { // cooking fish
                        if (fauna.Contains(tt))
                        {
                            //AddDebug(tt + " animal killed by player");

                            if (Main.config.animalsKilledTotal.ContainsKey(name))
                                Main.config.animalsKilledTotal[name]++;
                            else
                                Main.config.animalsKilledTotal[name] = 1;

                            if (Main.config.animalsKilled[saveSlot].ContainsKey(name))
                                Main.config.animalsKilled[saveSlot][name]++;
                            else
                                Main.config.animalsKilled[saveSlot][name] = 1;
                        }
                        else if (flora.Contains(tt))
                        {
                            //AddDebug(tt + " plant killed by player");
                            if (Main.config.plantsKilledTotal.ContainsKey(name))
                                Main.config.plantsKilledTotal[name]++;
                            else
                                Main.config.plantsKilledTotal[name] = 1;

                            if (Main.config.plantsKilled[saveSlot].ContainsKey(name))
                                Main.config.plantsKilled[saveSlot][name]++;
                            else
                                Main.config.plantsKilled[saveSlot][name] = 1;
                        }
                    }
                    if (item.item.GetComponent<Eatable>())
                        return;

                    if (Main.config.craftingResourcesUsed_[saveSlot].ContainsKey(name))
                        Main.config.craftingResourcesUsed_[saveSlot][name]++;
                    else
                        Main.config.craftingResourcesUsed_[saveSlot][name] = 1;

                    if (Main.config.craftingResourcesUsedTotal_.ContainsKey(name))
                        Main.config.craftingResourcesUsedTotal_[name]++;
                    else
                        Main.config.craftingResourcesUsedTotal_[name] = 1;

                    if (rb)
                    {
                        if (Main.config.craftingResourcesUsed[saveSlot].ContainsKey(name))
                            Main.config.craftingResourcesUsed[saveSlot][name] += rb.mass;
                        else
                            Main.config.craftingResourcesUsed[saveSlot][name] = rb.mass;

                        if (Main.config.craftingResourcesUsedTotal.ContainsKey(name))
                            Main.config.craftingResourcesUsedTotal[name] += rb.mass;
                        else
                            Main.config.craftingResourcesUsedTotal[name] = rb.mass;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(LifepodDrop), "OnSettled")]
        class LifepodDrop_OnGroundCollision_Patch
        { // TweaksAndFixes uses OnWaterCollision to add items
            public static void Postfix(LifepodDrop __instance)
            {
                //AddDebug("LifepodDrop OnSettled");
                StorageContainer sc = __instance.GetComponentInChildren<StorageContainer>();
                foreach (var itemsDic in sc.container._items)
                {
                    //AddDebug("start loot " + itemsDic.Key + " " + itemsDic.Value.items.Count);
                    string name = itemsDic.Key.AsString();
                    Main.config.storedLifePod[saveSlot][name] = itemsDic.Value.items.Count;
                    Main.config.storedLifePodTotal[name] = itemsDic.Value.items.Count;
                }
            }
        }

        [HarmonyPatch(typeof(ItemsContainer))]
        internal class ItemsContainer_Patch
        {
            [HarmonyPostfix]
            [HarmonyPatch("NotifyAddItem")]
            public static void NotifyAddItemPostfix(ItemsContainer __instance, InventoryItem item)
            {
                if (!Main.setupDone || Inventory.main.usedStorage.Count == 0 || Inventory.main._container.Equals(__instance) || __instance.tr.parent.GetComponent<Trashcan>())
                    return;
                //AddDebug("NotifyAddItem " + __instance.tr.name);
                TechType tt = item.item.GetTechType();
                //AddDebug("NotifyAddItem " + tt);
                Rigidbody rb = item.item.GetComponent<Rigidbody>();
                if (tt == TechType.None || rb == null)
                    return;

                string name = tt.AsString();

                if (Player.main.currentSub)
                {
                    //AddDebug("currentSub " );
                    if (Main.config.storedBase[saveSlot].ContainsKey(name))
                        Main.config.storedBase[saveSlot][name]++;
                    else
                        Main.config.storedBase[saveSlot][name] = 1;

                    if (Main.config.storedBaseTotal.ContainsKey(name))
                        Main.config.storedBaseTotal[name]++;
                    else
                        Main.config.storedBaseTotal[name] = 1;
                }
                else if (Player.main.currentInterior is LifepodDrop)
                {
                    //AddDebug("NotifyRemoveItem LifepodDrop " + tt); 
                    if (Main.config.storedLifePod[saveSlot].ContainsKey(name))
                        Main.config.storedLifePod[saveSlot][name]++;
                    else
                        Main.config.storedLifePod[saveSlot][name] = 1;

                    if (Main.config.storedLifePodTotal.ContainsKey(name))
                        Main.config.storedLifePodTotal[name]++;
                    else
                        Main.config.storedLifePodTotal[name] = 1;
                }
                else if(Player.main.currentInterior is SeaTruckSegment)
                {
                    //AddDebug("SeaTruckSegment ");
                    if (Main.config.storedSeatruck[saveSlot].ContainsKey(name))
                        Main.config.storedSeatruck[saveSlot][name]++;
                    else
                        Main.config.storedSeatruck[saveSlot][name] = 1;

                    if (Main.config.storedSeatruckTotal.ContainsKey(name))
                        Main.config.storedSeatruckTotal[name]++;
                    else
                        Main.config.storedSeatruckTotal[name] = 1;
                }
                else
                {
                    //AddDebug("Outside ");
                    if (Main.config.storedOutside[saveSlot].ContainsKey(name))
                        Main.config.storedOutside[saveSlot][name]++;
                    else
                        Main.config.storedOutside[saveSlot][name] = 1;

                    if (Main.config.storedOutsideTotal.ContainsKey(name))
                        Main.config.storedOutsideTotal[name]++;
                    else
                        Main.config.storedOutsideTotal[name] = 1;
                }
            }
            [HarmonyPostfix]
            [HarmonyPatch("NotifyRemoveItem")]
            public static void NotifyRemoveItemPostfix(ItemsContainer __instance, InventoryItem item)
            {
                if (!Main.setupDone || Inventory.main.usedStorage.Count == 0 || Inventory.main._container.Equals(__instance) || __instance.tr.parent.GetComponent<Trashcan>())
                    return;

                //AddDebug("NotifyRemoveItem " + __instance.tr.name);
                TechType tt = item.item.GetTechType();
                //AddDebug("NotifyRemoveItem " + tt);
                string name = tt.AsString();
                Rigidbody rb = item.item.GetComponent<Rigidbody>();
                if (tt == TechType.None || rb == null)
                    return;

                if (Player.main.currentSub)
                {
                    //AddDebug("NotifyRemoveItem IsInBase " + tt);
                    if (Main.config.storedBase[saveSlot].ContainsKey(name) && Main.config.storedBase[saveSlot][name] > 0)
                        Main.config.storedBase[saveSlot][name]--;

                    if (Main.config.storedBaseTotal.ContainsKey(name) && Main.config.storedBaseTotal[name] > 0)
                        Main.config.storedBaseTotal[name]--;
                }
                else if (Player.main.currentInterior is LifepodDrop)
                {
                    //AddDebug("NotifyRemoveItem LifepodDrop " + tt); 
                    if (Main.config.storedLifePod[saveSlot].ContainsKey(name) && Main.config.storedLifePod[saveSlot][name] > 0)
                        Main.config.storedLifePod[saveSlot][name]--;

                    if (Main.config.storedLifePodTotal.ContainsKey(name) && Main.config.storedLifePodTotal[name] > 0)
                        Main.config.storedLifePodTotal[name]--;
                }
                else if (Player.main.currentInterior is SeaTruckSegment)
                {
                    //AddDebug("NotifyRemoveItem SeaTruck " + tt); 
                    if (Main.config.storedSeatruck[saveSlot].ContainsKey(name) && Main.config.storedSeatruck[saveSlot][name] > 0)
                        Main.config.storedSeatruck[saveSlot][name]--;

                    if (Main.config.storedSeatruckTotal.ContainsKey(name) && Main.config.storedSeatruckTotal[name] > 0)
                        Main.config.storedSeatruckTotal[name]--;
                }
                else
                {
                    //AddDebug("NotifyRemoveItem " + tt);
                    if (Main.config.storedOutside[saveSlot].ContainsKey(name) && Main.config.storedOutside[saveSlot][name] > 0)
                        Main.config.storedOutside[saveSlot][name]--;

                    if (Main.config.storedOutsideTotal.ContainsKey(name) && Main.config.storedOutsideTotal[name] > 0)
                        Main.config.storedOutsideTotal[name]--;
                }
            }
        }

        [HarmonyPatch(typeof(BlueprintHandTarget), "UnlockBlueprint")]
        internal class BlueprintHandTarget_UnlockBlueprint_Patch
        {
            public static void Postfix(BlueprintHandTarget __instance)
            {
                if (string.IsNullOrEmpty(saveSlot) || __instance.used || KnownTech.Contains(__instance.unlockTechType))
                    return;

                //AddDebug("unlock  " + __instance.unlockTechType);
                Main.config.blueprintsFromDatabox[saveSlot]++;
                Main.config.blueprintsFromDataboxTotal++;
            }
        }

        [HarmonyPatch(typeof(ScannerTool), "Scan")]
        internal class ScannerTool_Scan_Patch
        {
            public static void Postfix(ScannerTool __instance, PDAScanner.Result __result)
            {
                //TechType techType = PDAScanner.scanTarget.techType;
                if (__result == PDAScanner.Result.None || __result == PDAScanner.Result.Scan || __result == PDAScanner.Result.Known || __result == PDAScanner.Result.Processed) { }
                else
                {
                    //AddDebug("Scan result " + __result);
                    Main.config.objectsScanned[saveSlot]++;
                    Main.config.objectsScannedTotal++;
                    TechType tt = PDAScanner.scanTarget.techType;
                    string name = tt.AsString();

                    if (fauna.Contains(tt))
                    {
                        //AddDebug("scanned creature");
                        Main.config.faunaFound[saveSlot].Add(name);
                        Main.config.faunaFoundTotal.Add(name);
                    }
                    else if (flora.Contains(tt))
                    {
                        //AddDebug("scanned flora"); 
                        Main.config.floraFound[saveSlot].Add(name);
                        Main.config.floraFoundTotal.Add(name);
                    }
                    else if (coral.Contains(tt))
                    {
                        //AddDebug("scanned coral");
                        if (tt == TechType.BlueJeweledDisk || tt == TechType.GreenJeweledDisk || tt == TechType.PurpleJeweledDisk || tt == TechType.RedJeweledDisk || tt == TechType.GenericJeweledDisk)
                        {
                            if (Main.config.jeweledDiskFound[saveSlot])
                                return;
                            Main.config.jeweledDiskFound[saveSlot] = true;
                        }
                        Main.config.coralFound[saveSlot].Add(name);
                        Main.config.coralFoundTotal.Add(name);
                    }
                    else if (leviathans.Contains(tt))
                    {
                        //AddDebug("scanned leviathan");
                        Main.config.leviathanFound[saveSlot].Add(name);
                        Main.config.leviathanFoundTotal.Add(name);
                    }

                }

            }
        }

        [HarmonyPatch(typeof(PDAScanner), "Unlock")]
        internal class PDAScanner_Unlock_Patch
        {
            public static void Postfix(PDAScanner.EntryData entryData, bool unlockBlueprint, bool unlockEncyclopedia,
    bool verbose)
            {
                if (entryData == null || string.IsNullOrEmpty(saveSlot) || !verbose)
                    return;

                if (unlockBlueprint && entryData.blueprint != TechType.None)
                { // scanning bladderfish unlocks bladderfish blueprint
                    //AddDebug("PDAScanner unlock  " + entryData.key);
                    //AddDebug("PDAScanner unlock blueprint " + entryData.blueprint);
                    if (fauna.Contains(entryData.blueprint) || flora.Contains(entryData.blueprint) || coral.Contains(entryData.blueprint) || leviathans.Contains(entryData.blueprint))
                        return;
                    Main.config.blueprintsUnlocked[saveSlot]++;
                    Main.config.blueprintsUnlockedTotal++;
                }
                //if (!PDAEncyclopedia.ContainsEntry(entryData.encyclopedia))
                //{
                //    AddDebug("unlock Encyclopedia " + entryData.encyclopedia);
                //}
                //if (!string.IsNullOrEmpty( entryData.encyclopedia))
                //    AddDebug("unlock Encyclopedia ");
            }
        }

        [HarmonyPatch(typeof(uGUI_EncyclopediaTab), "Activate")]
        internal class uGUI_EncyclopediaTab_Activate_Patch
        {
            public static void Postfix(uGUI_EncyclopediaTab __instance, CraftNode node)
            {
                lastEncNode = node;
                //if (strings.ContainsKey(node.id))
                //{
                //    AddDebug("Activate " + __instance.activeEntry.key);
                //}
            }
        }

        [HarmonyPatch(typeof(Constructable))]
        class Constructable_NotifyConstructedChanged_Patch
        {
            [HarmonyPatch(nameof(Constructable.Construct))]
            [HarmonyPrefix]
            static void ConstructPretfix(Constructable __instance)
            {
                constructing = true;
            }
            [HarmonyPatch(nameof(Constructable.Construct))]
            [HarmonyPostfix]
            static void ConstructPostfix(Constructable __instance)
            {
                constructing = false;
            }
            [HarmonyPatch(nameof(Constructable.NotifyConstructedChanged))]
            [HarmonyPostfix]
            static void NotifyConstructedChangedPostfix(Constructable __instance, bool constructed)
            {
                //AddDebug("NotifyConstructedChanged " + __instance.techType + "  " + constructed + " " + __instance.constructedAmount);
                if (__instance.constructedAmount < 1f)
                    return;

                if (constructed)
                {
                    if (corridorTypes.Contains(__instance.techType))
                    {
                        Main.config.baseCorridorsBuilt[saveSlot]++;
                        Main.config.baseCorridorsBuiltTotal++;
                    }
                    else if (roomTypes.Contains(__instance.techType))
                    {
                        Main.config.baseRoomsBuilt[saveSlot]++;
                        Main.config.baseRoomsBuiltTotal++;
                    }
                }
                else if(!constructed)
                {
                    if (corridorTypes.Contains(__instance.techType))
                    {
                        if(Main.config.baseCorridorsBuilt[saveSlot] > 0)
                            Main.config.baseCorridorsBuilt[saveSlot]--;
                        if (Main.config.baseCorridorsBuiltTotal > 0)
                            Main.config.baseCorridorsBuiltTotal--;
                    }
                    else if (roomTypes.Contains(__instance.techType))
                    {
                        if (Main.config.baseRoomsBuilt[saveSlot] > 0)
                            Main.config.baseRoomsBuilt[saveSlot]--;
                        if (Main.config.baseRoomsBuiltTotal > 0)
                            Main.config.baseRoomsBuiltTotal--;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(SubRoot), "Start")]
        class SubRoot_UpdatePower_Patch
        {
            static void Postfix(SubRoot __instance)
            {
                if (__instance.powerRelay)
                    powerRelays.Add(__instance.powerRelay);
            }
        }

        //[HarmonyPatch(typeof(PowerRelay), "Start")]
        internal class PowerRelay_Start_Patch
        {
            public static void Postfix(PowerRelay __instance)
            {

                if (__instance.GetComponent<LifepodDrop>() || __instance.GetComponent<SeaTruckSegment>() || __instance.GetComponent<Hoverpad>())
                { }
                else
                {
                    powerRelays.Add(__instance);
                    //AddDebug(" PowerRelay Start " + __instance.name + " " + __instance.GetMaxPower());
                }
            }
        }

        [HarmonyPatch(typeof(uGUI_EncyclopediaTab), "Open")]
        internal class uGUI_EncyclopediaTab_Close_Patch
        {
            public static void Prefix(uGUI_EncyclopediaTab __instance)
            {
                if (lastEncNode != null && myStrings.ContainsKey(lastEncNode.id))
                { // update stats 
                    //AddDebug("update tab");
                    __instance.activeEntry = null;
                    __instance.Activate(lastEncNode);
                }
            }
        }

        [HarmonyPatch(typeof(Vehicle), "OnKill")]
        internal class Vehicle_OnKill_Patch
        {
            public static void Postfix(Vehicle __instance)
            {
                if (__instance is Exosuit)
                {
                    //AddDebug("Exosuit lost");
                    Main.config.exosuitsLost[saveSlot]++;
                    Main.config.exosuitsLostTotal++;
                }
            }
        }

        [HarmonyPatch(typeof(Hoverbike), "OnKill")]
        internal class Hoverbike_OnKill_Patch
        {
            public static void Postfix(Hoverbike __instance)
            {
                //AddDebug("Hoverbike OnKill");
                Main.config.snowfoxesLost[saveSlot]++;
                Main.config.snowfoxesLostTotal++;
            }
        }

        [HarmonyPatch(typeof(SeaTruckSegment), "OnKill")]
        internal class SeaTruckSegment_OnKill_Patch
        {
            public static void Postfix(SeaTruckSegment __instance)
            { // fires twice
                if (destroyedSTS && destroyedSTS.Equals(__instance))
                    return; 

                destroyedSTS = __instance;
                if (__instance.isMainCab)
                {
                    //AddDebug("seatruck lost");
                    Main.config.seatrucksLost[saveSlot]++;
                    Main.config.seatrucksLostTotal++;
                }
                else
                {
                    //AddDebug("seatruck module lost");
                    Main.config.seatruckModulesLost[saveSlot]++;
                    Main.config.seatruckModulesLostTotal++;
                }
            }
        }

        [HarmonyPatch(typeof(HoverpadConstructor), "SpawnHoverbikeAsync")]
        internal class HoverpadConstructor_Patch
        {
            public static void Postfix(HoverpadConstructor __instance)
            {
                //TechType tt = CraftData.GetTechType(constructedObject);
                //AddDebug("SpawnHoverbikeAsync " + __instance.timeToConstruct);
                Main.config.snowfoxesBuilt[saveSlot]++;
                Main.config.snowfoxesBuiltTotal++;
            }
        }

        [HarmonyPatch(typeof(Constructor), "OnConstructionDone")]
        internal class Constructor_OnConstructionDone_Patch
        {
            public static void Postfix(Constructor __instance, GameObject constructedObject)
            {
                TechType tt = CraftData.GetTechType(constructedObject);
                //AddDebug("built " + tt);
                //AddDebug("built " + constructedObject.name);
                //if (tt == TechType.Seamoth)
                //if (constructedObject.GetComponent<Hoverbike>())
                //{
                //    Main.config.snowfoxesBuilt[saveSlot]++;
                //    Main.config.snowfoxesBuiltTotal++;
                //}
                //else if (tt == TechType.Exosuit)
                if (tt == TechType.Exosuit)
                {
                    Main.config.exosuitsBuilt[saveSlot]++;
                    Main.config.exosuitsBuiltTotal++;
                }
                else if (tt == TechType.SeaTruck)
                { // tt is none
                    Main.config.seatrucksBuilt[saveSlot]++;
                    Main.config.seatrucksBuiltTotal++;
                }
                else if (tt == TechType.SeaTruckAquariumModule || tt == TechType.SeaTruckDockingModule || tt == TechType.SeaTruckFabricatorModule || tt == TechType.SeaTruckSleeperModule || tt == TechType.SeaTruckStorageModule || tt == TechType.SeaTruckTeleportationModule)
                {
                    Main.config.seatrucksModulesBuilt[saveSlot]++;
                    Main.config.seatruckModulesBuiltTotal++;
                }
            }
        }

        [HarmonyPatch(typeof(CrafterLogic), "TryPickupSingleAsync")]
        internal class CrafterLogic_TryPickupSingleAsync_Patch
        {
            public static void Postfix(TechType techType)
            {
                //AddDebug("TryPickupSingleAsync " + techType);
                finishedCrafting = true;
            }
        }
             
        [HarmonyPatch(typeof(CrafterLogic), "NotifyCraftEnd")]
        internal class CrafterLogic_NotifyCraftEnd_Patch
        {
            public static void Postfix(GameObject target, TechType techType)
            {
                //AddDebug("NotifyCraftEnd " + techType);
                if (!finishedCrafting)
                    return;

                finishedCrafting = false;

                if (target && target.GetComponent<Eatable>())
                    return;

                string name = techType.AsString();

                if (Main.config.itemsCrafted[saveSlot].ContainsKey(name))
                    Main.config.itemsCrafted[saveSlot][name]++;
                else
                    Main.config.itemsCrafted[saveSlot][name] = 1;

                if (Main.config.itemsCraftedTotal.ContainsKey(name))
                    Main.config.itemsCraftedTotal[name]++;
                else
                    Main.config.itemsCraftedTotal[name] = 1;
            }
        }

        [HarmonyPatch(typeof(Bed))]
        internal class Bed_Patch
        {
            [HarmonyPatch("EnterInUseMode")]
            [HarmonyPostfix]
            public static void EnterInUseModePostfix(Bed __instance)
            {
                bedTimeStart = GetTimePlayed();
                //AddDebug("EnterInUseMode ");
            }
            [HarmonyPatch("ExitInUseMode")]
            [HarmonyPostfix]
            public static void ExitInUseModePostfix(Bed __instance)
            {
                TimeSpan ts = GetTimePlayed() - bedTimeStart;
                Main.config.timeSlept[saveSlot] += ts;
                Main.config.timeSleptTotal += ts;
                //AddDebug("ExitInUseMode ");
            }
        }

        //[HarmonyPatch(typeof(PDAEncyclopedia), "Add", new Type[] { typeof(string), typeof(PDAEncyclopedia.Entry), typeof})]
        internal class PDAEncyclopedia_Add_Patch
        {
            public static void Postfix(string key, PDAEncyclopedia.Entry entry)
            {
                //uGUI_ListEntry uGuiListEntry = item as uGUI_ListEntry;
                //AddDebug("Add " + key);
                //Main.Log("Add " + key);
            }
        }

        //[HarmonyPatch(typeof(PDAEncyclopedia), "OnAdd")]
        internal class PDAEncyclopedia_OnAdd_Patch
        {
            public static void Postfix(CraftNode node, bool verbose)
            {
                //uGUI_ListEntry uGuiListEntry = item as uGUI_ListEntry;
                //AddDebug("Add " + node.id);
            }
        }

        //[HarmonyPatch(typeof(PDAEncyclopedia), "Initialize")]
        internal class PDAEncyclopedia_Initialize_Patch
        {
            public static void Postfix(PDAData pdaData)
            {
                mapping = PDAEncyclopedia.mapping;
                //Log("mapping count " + mapping.Count);
                //foreach (var item in mapping)
                //{
                //Log(item.Key + " " + item.Value);
                //}
            }
        }

        //[HarmonyPatch(typeof(PDAScanner), "NotifyRemove")]
        internal class PDAScanner_NotifyRemove_Patch
        {
            public static void Postfix(PDAScanner.Entry entry)
            {
                if (PDAScanner.onRemove == null)
                    return;

                //AddDebug("NotifyRemove " + entry.techType);
            }
        }

        public enum TravelMode
        {
            Walk,
            Swim,
            Seaglide,
            Exosuit,
            Seatruck,
            Snowfox,
            CreatureRide
        }
        /*
 public static void ModCompat()
 { // not used
     foreach (string tt in moddedCreatureTechtypes)
     {
         TechType newTT = TechType.None;
         TechTypeExtensions.FromString(tt, out newTT, false);
         if (newTT != TechType.None)
         {
             if (tt == "GulperLeviathan")
             {
                 gulperTT = newTT;
                 leviathans.Add(newTT);
             }
             else
                 fauna.Add(newTT);
         }
     }
 }
 */
    }
}