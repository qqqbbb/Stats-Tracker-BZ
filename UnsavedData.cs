﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ErrorMessage;

namespace Stats_Tracker
{
    internal class UnsavedData
    {
        static public TimeSpan timeEscapePod;
        static public TimeSpan timeSwam;
        static public TimeSpan timeWalked;
        static public Dictionary<TechType, TimeSpan> timeVehicles = new Dictionary<TechType, TimeSpan>();
        static public TimeSpan timeBase;
        static public TimeSpan timeSlept;
        static public int playerDeaths;
        static public int healthLost;
        static public int medkitsUsed;
        static public Dictionary<TechType, float> foodEaten = new Dictionary<TechType, float>();
        static public float waterDrunk;
        static public int distanceTraveled;
        static public int maxDepth;
        static public int distanceTraveledSwim;
        static public int distanceTraveledWalk;
        static public int distanceTraveledSeaglide;

        static public Dictionary<TechType, int> distanceTraveledVehicle = new Dictionary<TechType, int>();
        static public Dictionary<TechType, int> vehiclesLost = new Dictionary<TechType, int>();
        static public Dictionary<TechType, int> itemsCrafted = new Dictionary<TechType, int>();
        static public Dictionary<TechType, int> builderToolBuilt = new Dictionary<TechType, int>();
        static public Dictionary<TechType, int> constructorBuilt = new Dictionary<TechType, int>();
        static public int objectsScanned;
        static public HashSet<TechType> blueprintsFromDatabox = new HashSet<TechType>();
        static public HashSet<TechType> blueprintsUnlocked = new HashSet<TechType>();
        static public HashSet<TechType> floraFound = new HashSet<TechType>();
        static public HashSet<TechType> faunaFound = new HashSet<TechType>();
        static public HashSet<TechType> coralFound = new HashSet<TechType>();
        static public HashSet<TechType> leviathanFound = new HashSet<TechType>();
        static public Dictionary<TechType, int> animalsKilled = new Dictionary<TechType, int>();
        static public Dictionary<TechType, int> plantsKilled = new Dictionary<TechType, int>();
        static public Dictionary<TechType, int> coralKilled = new Dictionary<TechType, int>();
        static public Dictionary<TechType, int> leviathansKilled = new Dictionary<TechType, int>();
        static public Dictionary<TechType, int> plantsGrown = new Dictionary<TechType, int>();
        static public Dictionary<TechType, int> eggsHatched = new Dictionary<TechType, int>();
        static public Dictionary<string, TimeSpan> timeBiomes = new Dictionary<string, TimeSpan>();
        static public Dictionary<TechType, int> pickedUpItems
            = new Dictionary<TechType, int>();
        public static HashSet<PowerSource> basePowerSources = new HashSet<PowerSource> { };
        public static HashSet<Base> bases = new HashSet<Base> { };
        static public int minTemp = int.MaxValue;
        static public int maxTemp = int.MinValue;
        static public int minVehicleTemp = int.MaxValue;
        static public int maxVehicleTemp = int.MinValue;

        static public void SaveDic(Dictionary<string, Dictionary<string, int>> configDic, string slot, Dictionary<TechType, int> dic)
        {
            if (dic.Count == 0)
                return;

            if (configDic.ContainsKey(slot))
            {
                foreach (var kv in dic)
                    configDic[slot].AddValue(kv.Key.ToString(), kv.Value);
            }
            else
                configDic[slot] = TechTypeDicToStringDic(dic);
        }

        static public void SaveDic(Dictionary<string, Dictionary<string, float>> configDic, string slot, Dictionary<TechType, float> dic)
        {
            if (dic.Count == 0)
                return;

            if (configDic.ContainsKey(slot))
            {
                foreach (var kv in dic)
                    configDic[slot].AddValue(kv.Key.ToString(), kv.Value);
            }
            else
                configDic[slot] = TechTypeDicToStringDic(dic);
        }

        static public void SaveDic(Dictionary<string, Dictionary<string, TimeSpan>> configDic, string slot, Dictionary<string, TimeSpan> dic)
        {
            if (dic.Count == 0)
                return;

            if (configDic.ContainsKey(slot))
            {
                foreach (var kv in dic)
                    configDic[slot].AddValue(kv.Key, kv.Value);
            }
            else
                configDic[slot] = new Dictionary<string, TimeSpan>(dic);
        }

        static public void SaveDic(Dictionary<string, Dictionary<string, TimeSpan>> configDic, string slot, Dictionary<TechType, TimeSpan> dic)
        {
            if (dic.Count == 0)
                return;

            if (configDic.ContainsKey(slot))
            {
                foreach (var kv in dic)
                    configDic[slot].AddValue(kv.Key.ToString(), kv.Value);
            }
            else
            {
                configDic[slot] = new Dictionary<string, TimeSpan>();
                foreach (var kv in dic)
                    configDic[slot][kv.Key.ToString()] = kv.Value;
            }
        }

        static public Dictionary<string, float> TechTypeDicToStringDic(Dictionary<TechType, float> sourceDic)
        {
            Dictionary<string, float> newDic = new Dictionary<string, float>();
            foreach (var pair in sourceDic)
                newDic.Add(pair.Key.ToString(), pair.Value);

            return newDic;
        }

        static public Dictionary<string, int> TechTypeDicToStringDic(Dictionary<TechType, int> sourceDic)
        {
            Dictionary<string, int> newDic = new Dictionary<string, int>();
            foreach (var pair in sourceDic)
                newDic.Add(pair.Key.ToString(), pair.Value);

            return newDic;
        }

        static public void SaveSet(Dictionary<string, HashSet<string>> dic, string slot, HashSet<TechType> set)
        {
            if (set.Count == 0)
                return;

            if (dic.ContainsKey(slot))
            {
                foreach (var kv in set)
                    dic[slot].Add(kv.ToString());
            }
            else
                dic[slot] = TechTypeSetToStringSet(set);
        }

        static public HashSet<string> TechTypeSetToStringSet(HashSet<TechType> sourceSet)
        {
            HashSet<string> newSet = new HashSet<string>();
            foreach (var tt in sourceSet)
                newSet.Add(tt.ToString());

            return newSet;
        }

        private static void UpdateDicEntryIfGreater(Dictionary<string, int> dic, string slot, int value)
        {
            if (value == int.MaxValue || value == int.MinValue)
                return;

            if (dic.ContainsKey(slot))
            {
                if (dic[slot] < value)
                    dic[slot] = value;
            }
            else
                dic[slot] = value;
        }

        private static void UpdateDicEntryIfLess(Dictionary<string, int> dic, string slot, int value)
        {
            if (value == int.MaxValue || value == int.MinValue)
                return;

            if (dic.ContainsKey(slot))
            {
                if (dic[slot] > value)
                    dic[slot] = value;
            }
            else
                dic[slot] = value;
        }

        static public void SaveData(string slot)
        {
            //AddDebug("UnsavedData SaveData " + slot);
            //Main.logger.LogDebug("UnsavedData SaveData " + slot);
            Main.config.timePlayed.AddValue(slot, Patches.GetTimePlayed());
            Main.config.timeSwam.AddValue(slot, timeSwam);
            Main.config.timeWalked.AddValue(slot, timeWalked);
            Main.config.timeBase.AddValue(slot, timeBase);
            Main.config.timeSlept.AddValue(slot, timeSlept);
            Main.config.playerDeaths.AddValue(slot, playerDeaths);
            Main.config.healthLost.AddValue(slot, healthLost);
            Main.config.medkitsUsed.AddValue(slot, medkitsUsed);
            SaveDic(Main.config.foodEaten, slot, foodEaten);
            Main.config.waterDrunk.AddValue(slot, waterDrunk);
            Main.config.distanceTraveled.AddValue(slot, distanceTraveled);
            UpdateDicEntryIfGreater(Main.config.maxDepth, slot, maxDepth);
            Main.config.distanceTraveledSwim.AddValue(slot, distanceTraveledSwim);
            Main.config.distanceTraveledWalk.AddValue(slot, distanceTraveledWalk);
            Main.config.distanceTraveledSeaglide.AddValue(slot, distanceTraveledSeaglide);
            SaveDic(Main.config.vehiclesLost, slot, vehiclesLost);
            SaveDic(Main.config.distanceTraveledVehicle, slot, distanceTraveledVehicle);
            SaveDic(Main.config.itemsCrafted, slot, itemsCrafted);
            Main.config.baseRoomsBuilt[slot] = TechTypeDicToStringDic(GetRoomsDic());
            Main.config.baseCorridorsBuilt[slot] = GetCorridorsBuilt();
            Main.config.basePower[slot] = GetBasePowerDic();
            Main.config.objectsScanned.AddValue(slot, objectsScanned);
            SaveSet(Main.config.blueprintsFromDatabox, slot, blueprintsFromDatabox);
            SaveSet(Main.config.blueprintsUnlocked, slot, blueprintsUnlocked);
            SaveSet(Main.config.floraFound, slot, floraFound);
            SaveSet(Main.config.faunaFound, slot, faunaFound);
            SaveSet(Main.config.coralFound, slot, coralFound);
            SaveSet(Main.config.leviathanFound, slot, leviathanFound);
            SaveDic(Main.config.animalsKilled, slot, animalsKilled);
            SaveDic(Main.config.plantsKilled, slot, plantsKilled);
            SaveDic(Main.config.coralKilled, slot, coralKilled);
            SaveDic(Main.config.leviathansKilled, slot, leviathansKilled);
            SaveDic(Main.config.plantsGrown, slot, plantsGrown);
            SaveDic(Main.config.eggsHatched, slot, eggsHatched);
            SaveDic(Main.config.timeBiomes, slot, timeBiomes);
            SaveDic(Main.config.timeVehicles, slot, timeVehicles);
            SaveDic(Main.config.pickedUpItems, slot, pickedUpItems);
            SaveDic(Main.config.builderToolBuilt, slot, builderToolBuilt);
            SaveDic(Main.config.constructorBuilt, slot, constructorBuilt);
            UpdateDicEntryIfLess(Main.config.minTemp, slot, minTemp);
            UpdateDicEntryIfGreater(Main.config.maxTemp, slot, maxTemp);
            UpdateDicEntryIfLess(Main.config.minVehicleTemp, slot, minVehicleTemp);
            UpdateDicEntryIfGreater(Main.config.maxVehicleTemp, slot, maxVehicleTemp);
            Main.config.Save();
        }

        private static Dictionary<string, int> GetBasePowerDic()
        {
            Dictionary<string, int> basePower = new Dictionary<string, int>();
            foreach (PowerSource ps in basePowerSources)
            {
                if (ps == null || (int)ps.power < 1)
                    continue;

                TechType tt = CraftData.GetTechType(ps.gameObject);
                if (Patches.basePowerSourceTypes.Contains(tt))
                    basePower.AddValue(tt.ToString(), (int)ps.power);
            }
            return basePower;
        }

        public static Dictionary<TechType, int> GetRoomsDic()
        {
            Dictionary<TechType, int> roomsBuilt = new Dictionary<TechType, int>();
            foreach (var b in bases)
            {
                foreach (var type in b.cells)
                {
                    if (Patches.roomTypeToTechtype.ContainsKey(type))
                        roomsBuilt.AddValue(Patches.roomTypeToTechtype[type], 1);
                }
            }
            return roomsBuilt;
        }

        public static int GetCorridorsBuilt()
        {
            int total = 0;
            foreach (var b in bases)
            {
                foreach (var type in b.cells)
                {
                    if (type == Base.CellType.Corridor)
                        total++;
                }
            }
            return total;
        }

        static public void ResetData()
        {
            //AddDebug("Reset UnsavedData  ");
            //Main.logger.LogInfo("Reset UnsavedData  ");
            timeEscapePod = TimeSpan.Zero;
            timeSwam = TimeSpan.Zero;
            timeWalked = TimeSpan.Zero;
            timeBase = TimeSpan.Zero;
            timeSlept = TimeSpan.Zero;
            playerDeaths = 0;
            healthLost = 0;
            medkitsUsed = 0;
            foodEaten.Clear();
            waterDrunk = 0;
            distanceTraveled = 0;
            maxDepth = 0;
            distanceTraveledSwim = 0;
            distanceTraveledWalk = 0;
            distanceTraveledSeaglide = 0;
            itemsCrafted.Clear();
            distanceTraveledVehicle.Clear();
            objectsScanned = 0;
            blueprintsFromDatabox.Clear();
            blueprintsUnlocked.Clear();
            floraFound.Clear();
            faunaFound.Clear();
            coralFound.Clear();
            leviathanFound.Clear();
            animalsKilled.Clear();
            plantsKilled.Clear();
            coralKilled.Clear();
            leviathansKilled.Clear();
            plantsGrown.Clear();
            eggsHatched.Clear();
            timeBiomes.Clear();
            timeVehicles.Clear();
            pickedUpItems.Clear();
            builderToolBuilt.Clear();
            constructorBuilt.Clear();
            minTemp = int.MaxValue;
            maxTemp = int.MinValue;
            minVehicleTemp = int.MaxValue;
            maxVehicleTemp = int.MinValue;
            vehiclesLost.Clear();

        }

    }
}
