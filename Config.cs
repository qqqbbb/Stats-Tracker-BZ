﻿using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using System.Collections.Generic;
using System;

namespace Stats_Tracker
{
    //[Menu("Custom Spawner Settings")]
    public class Config : ConfigFile
    {
        public Dictionary<string, TimeSpan> timePlayed = new Dictionary<string, TimeSpan>();
        public Dictionary<string, TimeSpan> timeEscapePod = new Dictionary<string, TimeSpan>();
        public Dictionary<string, TimeSpan> timeSwam = new Dictionary<string, TimeSpan>();
        public Dictionary<string, TimeSpan> timeWalked = new Dictionary<string, TimeSpan>();
        public Dictionary<string, TimeSpan> timeSnowfox = new Dictionary<string, TimeSpan>();
        public Dictionary<string, TimeSpan> timeExosuit = new Dictionary<string, TimeSpan>();
        public Dictionary<string, TimeSpan> timeSeatruck = new Dictionary<string, TimeSpan>();
        public Dictionary<string, TimeSpan> timeBase = new Dictionary<string, TimeSpan>();
        public Dictionary<string, TimeSpan> timeSlept = new Dictionary<string, TimeSpan>();
        public Dictionary<string, int> playerDeaths = new Dictionary<string, int>();
        public int gamesWon = 0;
        public Dictionary<string, int> healthLost = new Dictionary<string, int>();
        public Dictionary<string, Dictionary<string, float>> foodEaten = new Dictionary<string, Dictionary<string, float>>();
        public Dictionary<string, float> foodEatenTotal = new Dictionary<string, float>();
        public float waterDrunkTotal = 0;
        public Dictionary<string, float> waterDrunk = new Dictionary<string, float>();
        public Dictionary<string, int> medkitsUsed = new Dictionary<string, int>();
        public Dictionary<string, int> distanceTraveled = new Dictionary<string, int>();
        public Dictionary<string, int> maxDepth = new Dictionary<string, int>();
        public Dictionary<string, int> distanceTraveledSwim = new Dictionary<string, int>();
        public Dictionary<string, int> distanceTraveledWalk = new Dictionary<string, int>();
        public Dictionary<string, int> distanceTraveledSeaglide = new Dictionary<string, int>();
        public Dictionary<string, int> distanceTraveledSnowfox = new Dictionary<string, int>();
        public Dictionary<string, int> distanceTraveledExosuit = new Dictionary<string, int>();
        public Dictionary<string, int> distanceTraveledSeatruck = new Dictionary<string, int>();
        public Dictionary<string, int> distanceTraveledCreature = new Dictionary<string, int>();
        public Dictionary<string, int> snowfoxesBuilt = new Dictionary<string, int>();
        public Dictionary<string, int> exosuitsBuilt = new Dictionary<string, int>();
        public Dictionary<string, int> seatrucksBuilt = new Dictionary<string, int>();
        public Dictionary<string, int> snowfoxesLost = new Dictionary<string, int>();
        public Dictionary<string, int> exosuitsLost = new Dictionary<string, int>();
        public Dictionary<string, int> seatrucksLost = new Dictionary<string, int>();
        public Dictionary<string, Dictionary<string, int>> itemsCrafted = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, int> baseRoomsBuilt = new Dictionary<string, int>();
        public Dictionary<string, int> baseCorridorsBuilt = new Dictionary<string, int>();
        public int baseCorridorsBuiltTotal = 0;
        public int baseRoomsBuiltTotal = 0;
        public Dictionary<string, int> basePower = new Dictionary<string, int>();
        public Dictionary<string, int> objectsScanned = new Dictionary<string, int>();
        public Dictionary<string, int> blueprintsUnlocked = new Dictionary<string, int>();
        public Dictionary<string, int> blueprintsFromDatabox = new Dictionary<string, int>();
        public Dictionary<string, HashSet<string>> floraFound = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> faunaFound = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> coralFound = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> leviathanFound = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, Dictionary<string, int>> animalsKilled = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, Dictionary<string, int>> plantsKilled = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, Dictionary<string, int>> coralKilled = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, Dictionary<string, int>> leviathansKilled = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, Dictionary<string, int>> plantsRaised = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, int> plantsRaisedTotal = new Dictionary<string, int>();
        public Dictionary<string, int> ghostsKilled = new Dictionary<string, int>();
        public Dictionary<string, int> repersKilled = new Dictionary<string, int>();
        public Dictionary<string, int> reefbacksKilled = new Dictionary<string, int>();
        public Dictionary<string, int> seaDragonsKilled = new Dictionary<string, int>();
        public Dictionary<string, int> seaEmperorsKilled = new Dictionary<string, int>();
        public Dictionary<string, int> seaTreadersKilled = new Dictionary<string, int>();
        public Dictionary<string, int> gulpersKilled = new Dictionary<string, int>();
        public Dictionary<string, Dictionary<string, int>> eggsHatched = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, HashSet<string>> diffEggsHatched = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, int> eggsHatchedTotal = new Dictionary<string, int>();
        public Dictionary<string, Dictionary<string, float>> craftingResourcesUsed = new Dictionary<string, Dictionary<string, float>>();
        public Dictionary<string, Dictionary<string, int>> craftingResourcesUsed_ = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, HashSet<string>> biomesFound = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, float> timeGameStarted = new Dictionary<string, float>();
        public int deathsTotal = 0;
        public int medkitsUsedTotal = 0;

        public Dictionary<string, bool> kooshFound = new Dictionary<string, bool>();
        public HashSet<string> faunaFoundTotal = new HashSet<string>();
        public HashSet<string> floraFoundTotal = new HashSet<string>();
        public HashSet<string> coralFoundTotal = new HashSet<string>();
        public HashSet<string> leviathanFoundTotal = new HashSet<string>();
        public Dictionary<string, bool> jeweledDiskFound = new Dictionary<string, bool>();
        public Dictionary<string, bool> ghostLevFound = new Dictionary<string, bool>();
        public Dictionary<string, int> animalsKilledTotal = new Dictionary<string, int>();
        public Dictionary<string, int> plantsKilledTotal = new Dictionary<string, int>();
        public Dictionary<string, int> coralKilledTotal = new Dictionary<string, int>();
        public Dictionary<string, int> leviathansKilledTotal = new Dictionary<string, int>();
        public Dictionary<string, int> itemsCraftedTotal = new Dictionary<string, int>();
        public Dictionary<string, float> craftingResourcesUsedTotal = new Dictionary<string, float>();
        public Dictionary<string, int> craftingResourcesUsedTotal_ = new Dictionary<string, int>();
        public Dictionary<string, int> storedLifePodTotal = new Dictionary<string, int>();
        public Dictionary<string, int> storedBaseTotal = new Dictionary<string, int>();
        public Dictionary<string, int> storedSeatruckTotal = new Dictionary<string, int>();
        public Dictionary<string, int> storedOutsideTotal = new Dictionary<string, int>();
        public int playerDeathsTotal = 0;
        public int healthLostTotal = 0;
        public Dictionary<string, Dictionary<string, int>> storedLifePod = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, Dictionary<string, int>> storedBase = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, Dictionary<string, int>> storedOutside = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, Dictionary<string, int>> storedSeatruck = new Dictionary<string, Dictionary<string, int>>();
        public int blueprintsFromDataboxTotal = 0;
        public int objectsScannedTotal = 0;
        public int blueprintsUnlockedTotal = 0;
        public int snowfoxesBuiltTotal = 0;
        public int exosuitsBuiltTotal = 0;
        public int seatrucksBuiltTotal = 0;
        public TimeSpan timeSleptTotal = TimeSpan.Zero;
        public int distanceTraveledTotal = 0;
        public int distanceTraveledCreatureTotal = 0;
        public int distanceTraveledSeaglideTotal = 0;
        public int distanceTraveledExosuitTotal = 0;
        public int distanceTraveledSeatruckTotal = 0;
        public int distanceTraveledSnowfoxTotal = 0;
        public int distanceTraveledWalkTotal = 0;
        public int distanceTraveledSwimTotal = 0;
        public TimeSpan timeSeatruckTotal = TimeSpan.Zero;
        public TimeSpan timeExosuitTotal = TimeSpan.Zero;
        public TimeSpan timeSnowfoxTotal = TimeSpan.Zero;
        public TimeSpan timeSwamTotal = TimeSpan.Zero;
        public TimeSpan timeWalkedTotal = TimeSpan.Zero;
        public TimeSpan timeEscapePodTotal = TimeSpan.Zero;
        public TimeSpan timeBaseTotal = TimeSpan.Zero;
        public int exosuitsLostTotal = 0;
        public int maxDepthGlobal = 0;
        public Dictionary<string, int> seatrucksModulesBuilt = new Dictionary<string, int>();
        public int seatruckModulesBuiltTotal = 0;
        public int snowfoxesLostTotal = 0;
        public int seatrucksLostTotal = 0;
        public int seatruckModulesLostTotal = 0;
        public Dictionary<string, int> seatruckModulesLost = new Dictionary<string, int>();
        public HashSet<string> biomesFoundGlobal = new HashSet<string>();
    }
}