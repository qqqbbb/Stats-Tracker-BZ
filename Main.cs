
using HarmonyLib;
using System.Reflection;
using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Bootstrap;
using Nautilus.Handlers;
using Nautilus.Assets;
using Nautilus.Utility;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets.Gadgets;
using static ErrorMessage;
using static OVRPlugin;
using Steamworks;

namespace Stats_Tracker
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        private const string
            MODNAME = "Stats Tracker",
            GUID = "qqqbbb.subnauticaBZ.statsTracker",
            VERSION = "2.0.0";
        public static Config config { get; } = OptionsPanelHandler.RegisterModOptions<Config>();
        public static bool setupDone = false;

        public static void PrepareSaveSlot(string saveSlot)
        {
            //AddDebug("PrepareSaveSlot  " + saveSlot);
            //Log("PrepareSaveSlot  " + saveSlot);
            config.playerDeaths[saveSlot] = 0;
            config.timePlayed[saveSlot] = TimeSpan.Zero;
            config.healthLost[saveSlot] = 0;
            config.foodEaten[saveSlot] = new Dictionary<string, float>();
            config.waterDrunk[saveSlot] = 0;
            config.distanceTraveled[saveSlot] = 0;
            config.maxDepth[saveSlot] = 0;
            config.distanceTraveledSwim[saveSlot] = 0;
            config.distanceTraveledCreature[saveSlot] = 0;
            config.distanceTraveledWalk[saveSlot] = 0;
            config.distanceTraveledSeaglide[saveSlot] = 0;
            config.distanceTraveledSnowfox[saveSlot] = 0;
            config.distanceTraveledExosuit[saveSlot] = 0;
            config.distanceTraveledSeatruck[saveSlot] = 0;
            config.snowfoxesBuilt[saveSlot] = 0;
            config.exosuitsBuilt[saveSlot] = 0;
            config.seatrucksBuilt[saveSlot] = 0;
            config.snowfoxesLost[saveSlot] = 0;
            config.exosuitsLost[saveSlot] = 0;
            config.seatrucksLost[saveSlot] = 0;
            config.timeSlept[saveSlot] = TimeSpan.Zero;
            config.timeSwam[saveSlot] = TimeSpan.Zero;
            config.timeWalked[saveSlot] = TimeSpan.Zero;
            config.timeSnowfox[saveSlot] = TimeSpan.Zero;
            config.timeExosuit[saveSlot] = TimeSpan.Zero;
            config.timeSeatruck[saveSlot] = TimeSpan.Zero;
            config.timeBase[saveSlot] = TimeSpan.Zero;
            config.timeEscapePod[saveSlot] = TimeSpan.Zero;
            config.baseRoomsBuilt[saveSlot] = 0;
            config.baseCorridorsBuilt[saveSlot] = 0;
            config.basePower[saveSlot] = 0;
            config.objectsScanned[saveSlot] = 0;
            config.blueprintsUnlocked[saveSlot] = 0;
            config.blueprintsFromDatabox[saveSlot] = 0;
            config.floraFound[saveSlot] = new HashSet<string>();
            config.faunaFound[saveSlot] = new HashSet<string>();
            config.leviathanFound[saveSlot] = new HashSet<string>();
            config.coralFound[saveSlot] = new HashSet<string>();
            config.animalsKilled[saveSlot] = new Dictionary<string, int>();
            config.plantsKilled[saveSlot] = new Dictionary<string, int>();
            config.coralKilled[saveSlot] = new Dictionary<string, int>();
            config.leviathansKilled[saveSlot] = new Dictionary<string, int>();
            config.plantsRaised[saveSlot] = new Dictionary<string, int>();
            config.eggsHatched[saveSlot] = new Dictionary<string, int>();
            config.itemsCrafted[saveSlot] = new Dictionary<string, int>();
            config.craftingResourcesUsed[saveSlot] = new Dictionary<string, float>();
            config.craftingResourcesUsed_[saveSlot] = new Dictionary<string, int>();
            config.biomesFound[saveSlot] = new HashSet<string>();
            config.jeweledDiskFound[saveSlot] = false;
            config.storedBase[saveSlot] = new Dictionary<string, int>();
            config.storedLifePod[saveSlot] = new Dictionary<string, int>();
            config.storedSeatruck[saveSlot] = new Dictionary<string, int>();
            config.storedOutside[saveSlot] = new Dictionary<string, int>();
            config.seatrucksModulesBuilt[saveSlot] = 0;
            config.seatruckModulesLost[saveSlot] = 0;
            config.medkitsUsed[saveSlot] = 0;
            config.Save();
        }

        public static void Setup()
        {
            string saveSlot = SaveLoadManager.main.currentSlot;
            Stats_Patch.saveSlot = saveSlot;
            Stats_Patch.timeLastUpdate = Stats_Patch.GetTimePlayed();

            if (!config.timePlayed.ContainsKey(saveSlot))
                PrepareSaveSlot(saveSlot);
            //if (!config.timeGameStarted.ContainsKey(saveSlot))
            //    config.timeGameStarted[saveSlot] = 0;

            //AddDebug("SETUP " + saveSlot);
            //Stats_Patch.ModCompat();
            foreach (var s in Stats_Patch.myStrings)
                PDAEncyclopedia.Add(s.Key, false, false);

            setupDone = true;
        }

        [HarmonyPatch(typeof(WaitScreen), "Hide")]
        internal class WaitScreen_Hide_Patch
        {
            public static void Postfix(WaitScreen __instance)
            {
                //AddDebug(" WaitScreen Hide  !!!");
                Setup();
            }
        }

        static void SaveData()
        {
            //config.baseRoomsBuiltTotal = Stats_Patch.baseRoomsBuiltTotal;
            config.timePlayed[SaveLoadManager.main.currentSlot] = Stats_Patch.GetTimePlayed();
            config.basePower[SaveLoadManager.main.currentSlot] = Stats_Patch.basePower;
            config.Save();
        }

        public static void CleanUp()
        {
            //AddDebug("CleanUp");
            setupDone = false;
            Stats_Patch.powerRelays = new HashSet<PowerRelay>();
            Stats_Patch.timeLastUpdate = TimeSpan.Zero;
            config.Load();
        }

        private void Start()
        {
            Harmony harmony = new Harmony(GUID);
            harmony.PatchAll();
            SaveUtils.RegisterOnSaveEvent(SaveData);
            SaveUtils.RegisterOnQuitEvent(CleanUp);
        }

        [HarmonyPatch(typeof(Player), "Awake")]
        internal class PlayerAwakePatcher
        {
            public static void Prefix()
            { // must be prefix
                Stats_Patch.AddEntries();
            }
        }

    }
}