
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using System.Reflection;
using System;
using System.Collections.Generic;
using static ErrorMessage;

namespace Stats_Tracker
{
    [QModCore]
    public class Main
    {
        public static Config config = new Config();
        public static bool setupDone = false;
//internal static Config config { get; } = OptionsPanelHandler.RegisterModOptions<Config>();

        public static void Log(string str, QModManager.Utility.Logger.Level lvl = QModManager.Utility.Logger.Level.Debug)
        {
            QModManager.Utility.Logger.Log(lvl, str);
        }

        public static void PrepareSaveSlot(string saveSlot)
        {
            //AddDebug("PrepareSaveSlot  " + saveSlot);
            //Log("PrepareSaveSlot  " + saveSlot);
            config.playerDeaths[saveSlot] = 0;
            config.timePlayed[saveSlot] = TimeSpan.Zero;
            config.healthLost[saveSlot] = 0;
            config.foodEaten[saveSlot] = new Dictionary<TechType, float>();
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
            config.floraFound[saveSlot] = new HashSet<TechType>();
            config.faunaFound[saveSlot] = new HashSet<TechType>();
            config.leviathanFound[saveSlot] = new HashSet<TechType>();
            config.coralFound[saveSlot] = new HashSet<TechType>();
            config.animalsKilled[saveSlot] = new Dictionary<TechType, int>();
            config.plantsKilled[saveSlot] = new Dictionary<TechType, int>();
            config.coralKilled[saveSlot] = new Dictionary<TechType, int>();
            config.leviathansKilled[saveSlot] = new Dictionary<TechType, int>();
            config.plantsRaised[saveSlot] = new Dictionary<TechType, int>();
            config.eggsHatched[saveSlot] = new Dictionary<TechType, int>();
            config.itemsCrafted[saveSlot] = new Dictionary<TechType, int>();
            config.craftingResourcesUsed[saveSlot] = new Dictionary<TechType, float>();
            config.craftingResourcesUsed_[saveSlot] = new Dictionary<TechType, int>();
            config.biomesFound[saveSlot] = new HashSet<string>();
            config.jeweledDiskFound[saveSlot] = false;
            config.storedBase[saveSlot] = new Dictionary<TechType, int>();
            config.storedSeatruck[saveSlot] = new Dictionary<TechType, int>();
            config.storedOutside[saveSlot] = new Dictionary<TechType, int>();
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

        [HarmonyPatch(typeof(uGUI_SceneLoading), "End")]
        internal class uGUI_SceneLoading_End_Patch
        { // fires 3 times after game loads
            public static void Postfix(uGUI_SceneLoading __instance)
            {
                //if (!uGUI.main.hud.active)
                //{
                //AddDebug(" is Loading");
                //return;
                //}
                //AddDebug(" uGUI_SceneLoading end");
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
            config.Load();
            Stats_Patch.powerRelays = new HashSet<PowerRelay>();
            Stats_Patch.timeLastUpdate = TimeSpan.Zero;
        }

        [QModPatch]
        public static void Load()
        {
            config.Load();
            Assembly assembly = Assembly.GetExecutingAssembly();
            new Harmony($"qqqbbb_{assembly.GetName().Name}").PatchAll(assembly);
            IngameMenuHandler.RegisterOnSaveEvent(SaveData);
            IngameMenuHandler.RegisterOnQuitEvent(CleanUp);

        }

        //[HarmonyPatch(typeof(SaveLoadManager), "ClearSlotAsync")]
        internal class SaveLoadManager_ClearSlotAsync_Patch
        {
            public static void Postfix(SaveLoadManager __instance, string slotName)
            {
                //AddDebug("ClearSlotAsync " + slotName);
                //config.escapePodSmokeOut.Remove(slotName);
                //config.openedWreckDoors.Remove(slotName);
                //config.Save();
            }
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