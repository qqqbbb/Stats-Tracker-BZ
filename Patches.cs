using HarmonyLib;
using mset;
using Nautilus.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;
using static ErrorMessage;

namespace Stats_Tracker
{
    internal class Patches
    {
        static LiveMixin killedLM = null;
        public static string saveSlot;
        static TimeSpan timeLastUpdate = default;
        public static HashSet<TechType> roomTypes = new HashSet<TechType> { TechType.BaseRoom, TechType.BaseMapRoom, TechType.BaseMoonpool, TechType.BaseObservatory, TechType.BaseLargeRoom, TechType.BaseControlRoom, TechType.BaseMoonpoolExpansion };
        public static Dictionary<Base.CellType, TechType> roomTypeToTechtype = new Dictionary<Base.CellType, TechType>
        {
            { Base.CellType.Room,  TechType.BaseRoom },
            {  Base.CellType.MapRoom, TechType.BaseMapRoom },
            {  Base.CellType.MapRoomRotated, TechType.BaseMapRoom },
            {  Base.CellType.Moonpool, TechType.BaseMoonpool },
            {  Base.CellType.MoonpoolRotated, TechType.BaseMoonpool },
            {  Base.CellType.Observatory, TechType.BaseObservatory },
            {  Base.CellType.ControlRoom, TechType.BaseControlRoom },
            {  Base.CellType.ControlRoomRotated, TechType.BaseControlRoom },
            {  Base.CellType.LargeRoom, TechType.BaseLargeRoom },
            {  Base.CellType.LargeRoomRotated, TechType.BaseLargeRoom },
            {  Base.CellType.MoonpoolExpansionE, TechType.BaseMoonpoolExpansion },
            {  Base.CellType.MoonpoolExpansionN, TechType.BaseMoonpoolExpansion },
            {  Base.CellType.MoonpoolExpansionS, TechType.BaseMoonpoolExpansion },
            {  Base.CellType.MoonpoolExpansionW, TechType.BaseMoonpoolExpansion },
        };
        public static HashSet<TechType> corridorTypes = new HashSet<TechType> { TechType.BaseCorridorI, TechType.BaseCorridorL, TechType.BaseCorridorT, TechType.BaseCorridorX, TechType.BaseCorridorGlassI, TechType.BaseCorridorGlassL, TechType.BaseCorridor, TechType.BaseCorridorGlass };
        public static HashSet<TechType> basePowerSourceTypes = new HashSet<TechType> { TechType.SolarPanel, TechType.ThermalPlant, TechType.BaseNuclearReactor, TechType.BaseBioReactor };

        //public static HashSet<TechType> faunaVanilla = new HashSet<TechType> { TechType.Brinewing, TechType.BruteShark, TechType.Cryptosuchus, TechType.NootFish, TechType.Penguin, TechType.PenguinBaby, TechType.Crash, TechType.Pinnacarid, TechType.RockPuncher, TechType.SnowStalker, TechType.SnowStalkerBaby, TechType.SpikeyTrap, TechType.Bladderfish, TechType.Boomerang, TechType.SquidShark, TechType.Symbiote, TechType.ArcticPeeper, TechType.ArcticRay, TechType.ArrowRay, TechType.DiscusFish, TechType.FeatherFish, TechType.FeatherFishRed, TechType.Hoopfish, TechType.Jellyfish, TechType.HivePlant, TechType.LilyPaddler, TechType.SeaMonkey, TechType.SeaMonkeyBaby, TechType.SpinnerFish, TechType.TitanHolefish, TechType.Skyray, TechType.Triops, TechType.Spinefish, TechType.BlueAmoeba, TechType.TrivalveBlue, TechType.TrivalveYellow };
        public static HashSet<TechType> leviathans = new HashSet<TechType>();
        public static HashSet<TechType> creatures = new HashSet<TechType>();
        //public static HashSet<TechType> leviathansVanilla = new HashSet<TechType>
        //{TechType.GlowWhale, TechType.Chelicerate, TechType.IceWorm, TechType.LargeVentGarden, TechType.SmallVentGarden, TechType.SeaEmperorJuvenile, TechType.ShadowLeviathan };
        public static HashSet<TechType> coral = new HashSet<TechType> { TechType.CoralShellPlate, TechType.BrownTubes, TechType.BigCoralTubes, TechType.BlueCoralTubes, TechType.RedTipRockThings, TechType.GenericJeweledDisk, TechType.BlueJeweledDisk, TechType.GreenJeweledDisk, TechType.RedJeweledDisk, TechType.PurpleJeweledDisk, TechType.TreeMushroom, TechType.BrainCoral, TechType.TwistyBridgesCoralShelf };
        public static HashSet<TechType> flora = new HashSet<TechType> { TechType.BloodRoot, TechType.BloodVine, TechType.SmallMaroonPlant, TechType.GenericArmored, TechType.LilyPadMature, TechType.LilyPadRoot, TechType.GenericBigPlant1, TechType.GenericShellSingle, TechType.PurpleBranches, TechType.PurpleVegetablePlant, TechType.Creepvine, TechType.GenericShellDouble, TechType.EyesPlant, TechType.FernPalm, TechType.BlueFurPlant, TechType.DeepTwistyBridgesLargePlant, TechType.JellyPlant, TechType.RedGreenTentacle, TechType.OrangePetalsPlant, TechType.GenericBulbStalk, TechType.TwistyBridgesMushroom, TechType.HangingFruitTree, TechType.CavePlant, TechType.MelonPlant, TechType.DeepLilyPadsLanternPlant, TechType.TwistyBridgeCliffPlant, TechType.OxygenPlant, TechType.RedBush, TechType.TwistyBridgeCoralLong, TechType.RedBasketPlant, TechType.GenericCage, TechType.GlacialPouchBulb, TechType.ShellGrass, TechType.SpottedLeavesPlant, TechType.CrashHome, TechType.SnowStalkerPlant, TechType.TapePlant, TechType.PurpleStalk, TechType.PinkFlower, TechType.PurpleTentacle, TechType.BloodGrass, TechType.RedGrass, TechType.RedSeaweed, TechType.BlueBarnacle, TechType.BlueBarnacleCluster, TechType.BlueLostRiverLilly, TechType.BlueTipLostRiverPlant, TechType.HangingStinger, TechType.CoveTree, TechType.BlueCluster, TechType.GreenReeds, TechType.BarnacleSuckers, TechType.BallClusters, TechType.GenericCrystal, TechType.GenericBowl, TechType.TallShootsPlant, TechType.TreeSpireMushroom, TechType.GenericRibbon, TechType.HeatFruitPlant, TechType.GlacialTree, TechType.IceFruitPlant, TechType.FrozenRiverPlant2, TechType.FrozenRiverPlant1, TechType.SnowPlant, TechType.Mohawk, TechType.PurpleRattle, TechType.GenericSpiral, TechType.CaveFlower, TechType.TrianglePlant, TechType.OrangePetalsPlant, TechType.ThermalLily, TechType.TwistyBridgesLargePlant, TechType.ThermalSpireBarnacle, TechType.TornadoPlates, TechType.HoneyCombPlant, TechType.LeafyFruitPlant, TechType.GlacialBulb, TechType.PinkFlower, TechType.GenericBigPlant2, TechType.DeepLilyShroom, TechType.KelpRoot };
        public static HashSet<TechType> constructorBuilt = new HashSet<TechType>();
        public static TechType currentVehicleTT;
        static Dictionary<TechType, float> itemMass = new Dictionary<TechType, float>();
        const int leviathanMinHealth = 4000;
        static BodyTemperature bodyTemperature;
        static bool playerLanded = false;
        static TimeSpan timePlayedAtGameStart = new TimeSpan(8, 27, 0);
        static bool playerTakesDamage = false;
        static bool teleporting = false;
        private static TechType hatchedTT;


        public static void CleanUp()
        {
            timeLastUpdate = TimeSpan.Zero;
            playerLanded = false;
            teleporting = false;
        }

        public static TimeSpan GetTimeSpanPlayed()
        {
            return new TimeSpan(0, 0, Mathf.FloorToInt(DayNightCycle.main.timePassedAsFloat * DayNightCycle.main.gameSecondMultiplier)) - timePlayedAtGameStart;
        }

        static float GetItemMass(TechType techType)
        {
            ItemsContainer.ItemGroup itemGroup;
            if (!Inventory.main._container._items.TryGetValue(techType, out itemGroup))
                return 0f;

            List<InventoryItem> items = itemGroup.items;
            int index1 = items.Count - 1;
            InventoryItem inventoryItem1 = items[index1];
            Rigidbody rb = inventoryItem1.item.GetComponent<Rigidbody>();
            if (rb)
                return rb.mass;

            return 0f;
        }

        static float GetItemMass(TechType techType, GameObject gameObject)
        {
            if (itemMass.ContainsKey(techType))
                return itemMass[techType];

            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if (rb)
            {
                itemMass[techType] = rb.mass;
                return rb.mass;
            }
            return 0f;
        }


        [HarmonyPatch(typeof(Player))]
        internal class Player_Patch
        {
            static BasicText currentBiome = new BasicText(0, 250);

            private static void SaveTravelStats(Player player)
            {
                Vector3 position = player.transform.position;
                player.maxDepth = Mathf.Max(player.maxDepth, -position.y);
                UnsavedData.maxDepth = (int)player.maxDepth;
                //if (biomeName != "ST_unknown_biome")
                //    UnsavedData.biomesFound.Add(biomeName);

                if (player.lastPosition != Vector3.zero) // first run after game loads it's Vector3.zero
                    player.distanceTraveled += Vector3.Distance(position, player.lastPosition);

                int distanceTraveledSinceLastUpdate = Mathf.RoundToInt((player.lastPosition - position).magnitude);
                if (player.lastPosition == Vector3.zero)
                    distanceTraveledSinceLastUpdate = 0;

                player.lastPosition = position;
                if (distanceTraveledSinceLastUpdate == 0)
                    return;

                UnsavedData.distanceTraveled += distanceTraveledSinceLastUpdate;

                if (player.motorMode == Player.MotorMode.Seaglide)
                {
                    UnsavedData.distanceTraveledSeaglide += distanceTraveledSinceLastUpdate;
                }
                else if (Player.main.IsUnderwaterForSwimming())
                {
                    UnsavedData.distanceTraveledSwim += distanceTraveledSinceLastUpdate;
                }
                else if (Player.main.inHovercraft)
                {
                    UnsavedData.distanceTraveledVehicle.AddValue(TechType.Hoverbike, distanceTraveledSinceLastUpdate);
                }
                else if (player.currentMountedVehicle && currentVehicleTT != TechType.None)
                {
                    UnsavedData.distanceTraveledVehicle.AddValue(currentVehicleTT, distanceTraveledSinceLastUpdate);
                }
                else if (player.inSeatruckPilotingChair)
                {
                    UnsavedData.distanceTraveledVehicle.AddValue(TechType.SeaTruck, distanceTraveledSinceLastUpdate);
                }
                else if (player.motorMode == Player.MotorMode.Walk || player.motorMode == Player.MotorMode.Run)
                {
                    UnsavedData.distanceTraveledWalk += distanceTraveledSinceLastUpdate;
                }
            }

            private static void SaveTimeStats(Player player)
            {
                string biomeName = Util.GetBiomeName();
                TimeSpan timeSinceLastUpdate = GetTimeSpanPlayed() - timeLastUpdate;
                //AddDebug("timeSinceLastUpdate " + timeSinceLastUpdate);
                //AddDebug($"GetTimePlayed {GetTimePlayed()} timeLastUpdate {timeLastUpdate} ");
                if (biomeName != "ST_unknown_biome")
                {
                    UnsavedData.timeBiomes.AddValue(biomeName, timeSinceLastUpdate);
                }
                //if (player.currentInterior != null)
                //    AddDebug("currentInterior " + player.currentInterior.ToString());

                if (player.IsUnderwaterForSwimming())
                {
                    UnsavedData.timeSwam += timeSinceLastUpdate;
                }
                else if (player.mode == Player.Mode.Sitting)
                {
                    UnsavedData.timeSat += timeSinceLastUpdate;
                }
                else if (player.motorMode == Player.MotorMode.Walk || player.motorMode == Player.MotorMode.Run)
                { // MotorMode.Run when swimming on surface
                    UnsavedData.timeWalked += timeSinceLastUpdate;
                }
                else if (Player.main.inHovercraft)
                {
                    UnsavedData.timeVehicles.AddValue(TechType.Hoverbike, timeSinceLastUpdate);
                }
                else if (player.currentMountedVehicle && currentVehicleTT != TechType.None)
                {
                    //AddDebug("SAVE Vehicle time " + currentVehicleTT);
                    UnsavedData.timeVehicles.AddValue(currentVehicleTT, timeSinceLastUpdate);
                }

                if (player.currentSub)
                {
                    UnsavedData.timeBase += timeSinceLastUpdate;
                }
                else if (player.currentInterior is LifepodDrop)
                {
                    UnsavedData.timeEscapePod += timeSinceLastUpdate;
                }
                else if (player.currentInterior is SeaTruckSegment)
                {
                    UnsavedData.timeVehicles.AddValue(TechType.SeaTruck, timeSinceLastUpdate);
                }
                else if (player.currentInterior != null && player.currentInterior.ToString().StartsWith("Precursor"))
                {
                    //AddDebug("Precursor");
                    UnsavedData.timePrecursor += timeSinceLastUpdate;
                }
                //AddDebug("timeSwam " + UnsavedData.timeSwam[saveSlot]);
                timeLastUpdate = GetTimeSpanPlayed();
            }

            private static void SaveTempStats(Player player)
            {
                if (bodyTemperature == null)
                    bodyTemperature = player.GetComponent<BodyTemperature>();

                float temp = bodyTemperature.effectiveAmbientTemperature;

                if (Util.IsPlayerInVehicle())
                {
                    if (Player.main.transform.position.y < Ocean.GetOceanLevel())
                        temp = WaterTemperatureSimulation.main.GetTemperature(Player.main.transform.position);
                    else
                        temp = WeatherManager.main.GetFeelsLikeTemperature();

                    //AddDebug("In Vehicle temp " + (int)temp);
                    if (UnsavedData.minVehicleTemp > temp)
                        UnsavedData.minVehicleTemp = Mathf.RoundToInt(temp);

                    if (UnsavedData.maxVehicleTemp < temp)
                        UnsavedData.maxVehicleTemp = Mathf.RoundToInt(temp);
                }
                else
                {
                    //AddDebug(" temp " + (int)temp);
                    if (UnsavedData.minTemp > temp)
                        UnsavedData.minTemp = Mathf.RoundToInt(temp);

                    if (UnsavedData.maxTemp < temp)
                        UnsavedData.maxTemp = Mathf.RoundToInt(temp);
                }
            }

            public static bool ShouldBeTracking()
            {
                if (Player.main.isNewBorn)
                    return playerLanded;

                return Main.setupDone;
            }

            [HarmonyPrefix, HarmonyPatch("TrackTravelStats")]
            public static bool TrackTravelStatsPrefix(Player __instance)
            { // runs during opening cinematic
                if (ConfigMenu.modEnabled.Value == false)
                    return true;

                if (ShouldBeTracking() == false || teleporting)
                    return false;

                if (timeLastUpdate == default)
                    timeLastUpdate = GetTimeSpanPlayed();
                //AddDebug("isUnderwaterForSwimming " + __instance.IsUnderwaterForSwimming());
                //AddDebug($"TrackTravelStats isNewBorn {__instance.isNewBorn} startTracking {startTracking}");
                SaveTravelStats(__instance);
                SaveTimeStats(__instance);
                SaveTempStats(__instance);
                return false;
            }

            [HarmonyPostfix, HarmonyPatch("OnKill")]
            public static void OnKillPostfix(Player __instance)
            {
                if (!ConfigMenu.modEnabled.Value)
                    return;

                if (GameModeManager.GetOption<bool>(GameOption.PermanentDeath))
                {
                    Main.configMain.permaDeaths++;
                    Main.configMain.Save();
                }
                else
                    UnsavedData.playerDeaths++;
            }

            [HarmonyPostfix, HarmonyPatch("Update")]
            public static void UpdatePostfix(Player __instance)
            {
                ShowBiomeName();
            }

            private static void ShowBiomeName()
            {
                if (ConfigMenu.biomeName.Value == false || ShouldBeTracking() == false)
                    return;

                string biomeName = Language.main.Get(Util.GetBiomeName());
                //AddDebug("biomeName " + biomeName);
                if (currentBiome.GetText() == biomeName)
                    return;

                currentBiome.ShowMessage(biomeName, 5);
                //currentBiome.SetColor(Color.green);
            }
        }

        [HarmonyPatch(typeof(TeleportationTool), "StartTeleportSequence")]
        class TeleportationTool_StartTeleportSequence_Patch
        {
            static void Prefix(TeleportationTool __instance)
            {
                //AddDebug("StartTeleportSequence");
                OnStartTeleporting();
            }
        }

        private static void OnStartTeleporting()
        {
            teleporting = true;
            Player.main.lastPosition = Vector3.zero;
        }

        [HarmonyPatch(typeof(PrecursorTeleporter))]
        class PrecursorTeleporter_Patch
        {
            [HarmonyPrefix, HarmonyPatch("BeginTeleportPlayer")]
            public static void BeginTeleportPlayerPrefix(PrecursorTeleporter __instance, GameObject teleportObject)
            {
                //AddDebug("BeginTeleportPlayer");
                OnStartTeleporting();
            }
            [HarmonyPostfix, HarmonyPatch("TeleportationComplete")]
            public static void TeleportationCompletePostfix(PrecursorTeleporter __instance)
            {
                //AddDebug("TeleportationComplete");
                teleporting = false;
            }
        }

        [HarmonyPatch(typeof(IntroDropshipExplode), "Explode")]
        class IntroDropshipExplode_Explode_Patch
        {
            static void Postfix(IntroDropshipExplode __instance)
            {
                //AddDebug("Explode");
                playerLanded = true;
            }
        }

        [HarmonyPatch(typeof(Creature), "Start")]
        class Creature_Start_Patch
        {
            static void Postfix(Creature __instance)
            {
                if (__instance.liveMixin)
                {
                    TechType tt = CraftData.GetTechType(__instance.gameObject);
                    if (creatures.Contains(tt) || leviathans.Contains(tt))
                        return;

                    if (__instance.liveMixin.maxHealth >= leviathanMinHealth)
                        leviathans.Add(tt);
                    else
                        creatures.Add(tt);
                }
            }
        }

        [HarmonyPatch(typeof(PowerSource))]
        class PowerSource_Patch
        {
            [HarmonyPostfix, HarmonyPatch("Start")]
            static void StartPostfix(PowerSource __instance)
            {
                if (!ConfigMenu.modEnabled.Value)
                    return;

                TechType tt = CraftData.GetTechType(__instance.gameObject);
                if (basePowerSourceTypes.Contains(tt))
                {
                    //AddDebug("PowerSource Start " + tt + " power " + (int)__instance.power);
                    UnsavedData.basePowerSources.Add(__instance);
                }
            }
        }

        [HarmonyPatch(typeof(CraftingAnalytics))]
        class CraftingAnalytics_Patch
        {
            //[HarmonyPrefix]
            //[HarmonyPatch("OnConstruct")]
            static void OnConstructPrefix(CraftingAnalytics __instance, TechType techType)
            {
                //AddDebug("CraftingAnalytics OnConstruct " + techType);
            }

            [HarmonyPostfix, HarmonyPatch("OnConstruct")]
            static void OnConstructPostfix(CraftingAnalytics __instance, TechType techType)
            {
                //AddDebug("CraftingAnalytics OnConstruct " + techType);
                if (!ConfigMenu.modEnabled.Value)
                    return;

                if (corridorTypes.Contains(techType) || roomTypes.Contains(techType) || techType == TechType.Hoverbike)
                    return;

                if (!constructorBuilt.Contains(techType))
                    UnsavedData.builderToolBuilt.AddValue(techType, 1);
            }

            [HarmonyPostfix, HarmonyPatch("OnCraft")]
            static void OnCraftPostfix(CraftingAnalytics __instance, TechType techType)
            {
                //AddDebug("CraftingAnalytics OnCraft " + techType);
                if (!ConfigMenu.modEnabled.Value)
                    return;

                if (constructorBuilt.Contains(techType) || techType == TechType.Hoverbike)
                {
                    UnsavedData.constructorBuilt.AddValue(techType, 1);
                    //constructorBuilt.Remove(techType);
                    return;
                }
                ReadOnlyCollection<Ingredient> ingredients = TechData.GetIngredients(techType);

                if (ingredients == null || ingredients.Count == 0)
                    return;

                for (int index1 = 0; index1 < ingredients.Count; ++index1)
                {
                    Ingredient ingredient = ingredients[index1];
                    TechType ingredientTT = ingredient.techType;
                    if (creatures.Contains(ingredientTT))
                    {
                        //AddDebug(ingredientTT + " animal killed by player");
                        UnsavedData.animalsKilled.AddValue(ingredientTT, ingredient.amount);
                    }
                    else if (flora.Contains(ingredientTT))
                    {
                        //AddDebug(ingredientTT + " plant killed by player");
                        UnsavedData.plantsKilled.AddValue(ingredientTT, ingredient.amount);
                    }
                }
                string tts = techType.ToString();
                if (tts.StartsWith("Cooked") || tts.StartsWith("Cured"))
                    return;

                int craftAmount = TechData.GetCraftAmount(techType);
                UnsavedData.itemsCrafted.AddValue(techType, craftAmount);
            }
        }

        [HarmonyPatch(typeof(Survival))]
        class Survival_Patch
        {
            [HarmonyPostfix, HarmonyPatch("Use")]
            public static void UsePostfix(Survival __instance, GameObject useObj, bool __result)
            {
                if (!ConfigMenu.modEnabled.Value || !__result)
                    return;

                TechType tt = CraftData.GetTechType(useObj);
                if (tt == TechType.FirstAidKit)
                {
                    //AddDebug("medkit used");
                    UnsavedData.medkitsUsed++;
                }
            }
            [HarmonyPostfix, HarmonyPatch("Eat")]
            public static void EatPostfix(Survival __instance, GameObject useObj, bool __result)
            {
                if (!ConfigMenu.modEnabled.Value || __result == false)
                    return;

                TechType tt = CraftData.GetTechType(useObj);
                if (creatures.Contains(tt))
                {
                    //AddDebug(tt + " animal killed by player");
                    LiveMixin lm = useObj.GetComponent<LiveMixin>();
                    if (lm && lm.IsAlive())
                        UnsavedData.animalsKilled.AddValue(tt, 1);
                }
                else if (flora.Contains(tt))
                {
                    //AddDebug(tt + " flora killed by player");
                    LiveMixin lm = useObj.GetComponent<LiveMixin>();
                    if (lm && lm.IsAlive())
                        UnsavedData.plantsKilled.AddValue(tt, 1);
                }
                float mass = GetItemMass(tt, useObj);
                if (mass == 0)
                    return;

                Eatable eatable = useObj.GetComponent<Eatable>();
                if (eatable == null)
                    return;

                float foodValue = eatable.GetFoodValue();
                float waterValue = eatable.GetWaterValue();
                if (foodValue >= waterValue)
                    UnsavedData.foodEaten.AddValue(tt, mass);
                else if (waterValue > 0 && foodValue == 0)
                    UnsavedData.waterDrunk += mass;
            }
        }

        [HarmonyPatch(typeof(BelowZeroEndGame), "ShowCredits")]
        internal class BelowZeroEndGame_ShowCredits_Patch
        {
            public static void Postfix(BelowZeroEndGame __instance)
            {
                if (!ConfigMenu.modEnabled.Value)
                    return;
                //AddDebug("BelowZeroEndGame ShowCredits ");
                Main.configMain.gamesWon++;
                Main.configMain.Save();
            }
        }

        [HarmonyPatch(typeof(GrowingPlant), "SpawnGrownModel")]
        internal class GrowingPlant_SpawnGrownModel_Patch
        {
            public static void Postfix(GrowingPlant __instance)
            {
                if (!ConfigMenu.modEnabled.Value)
                    return;

                TechType tt = __instance.seed.plantTechType;
                if (tt == TechType.None)
                    return;

                //string name = __instance.seed.plantTechType.AsString();
                //AddDebug("GrownPlant Awake " + tt);
                UnsavedData.plantsGrown.AddValue(tt, 1);
            }
        }

        [HarmonyPatch(typeof(DamageSystem), "CalculateDamage", new Type[] { typeof(TechType), typeof(float), typeof(float), typeof(DamageType), typeof(GameObject), typeof(GameObject) })]
        class DamageSystem_CalculateDamage_Patch
        {
            public static void Postfix(DamageSystem __instance, float damage, DamageType type, GameObject target, GameObject dealer, ref float __result)
            {
                if (!ConfigMenu.modEnabled.Value)
                    return;

                //AddDebug(target.name + " CalculateDamage " + __result);
                if (playerTakesDamage && __result > 0)
                {
                    float currentHealth = Player.main.GetComponent<LiveMixin>().health;
                    float d = __result > currentHealth ? currentHealth : __result;
                    //AddDebug("Player takes damage " + d);
                    UnsavedData.healthLost += Mathf.RoundToInt(d);
                    playerTakesDamage = false;
                }
            }
        }
        [HarmonyPatch(typeof(LiveMixin))]
        internal class LiveMixin_Patch
        {
            public static bool WasKilledByPlayer(LiveMixin liveMixin, GameObject killer)
            {
                //TechType tt = CraftData.GetTechType(__instance.gameObject);
                //AddDebug("WasKilledByPlayer " + tt);
                if (killer && killedLM && liveMixin == killedLM)
                {
                    if (killer == Player.main.gameObject || killer == Player.main.currentMountedVehicle?.gameObject || killer == Player.main.currentSub?.gameObject)
                        return true;
                }
                return false;
            }

            [HarmonyPrefix, HarmonyPatch("Kill")]
            public static void KillPrefix(LiveMixin __instance)
            {
                if (!ConfigMenu.modEnabled.Value)
                    return;

                //TechType tt = CraftData.GetTechType(__instance.gameObject);
                //AddDebug("Kill " + tt);
                killedLM = __instance;
            }

            [HarmonyPrefix, HarmonyPatch("TakeDamage")]
            public static void TakeDamagePrefix(LiveMixin __instance, float originalDamage, Vector3 position, DamageType type, GameObject dealer)
            {
                if (__instance.TryGetComponent<Player>(out _))
                {
                    playerTakesDamage = true;
                }
            }

            [HarmonyPostfix, HarmonyPatch("TakeDamage")]
            public static void TakeDamagePostfix(LiveMixin __instance, float originalDamage, Vector3 position, DamageType type, GameObject dealer)
            {
                //AddDebug(__instance.name + " TakeDamage ");
                if (!ConfigMenu.modEnabled.Value)
                    return;

                if (WasKilledByPlayer(__instance, dealer))
                {
                    killedLM = null;
                    TechType tt = CraftData.GetTechType(__instance.gameObject);
                    //AddDebug(__instance.name + " WasKilledByPlayer ");
                    if (tt == TechType.None)
                        return;
                    //AddDebug(tt + " killed by player");
                    //string name = tt.AsString();
                    if (creatures.Contains(tt))
                    {
                        //AddDebug(tt + " animal killed by player");
                        UnsavedData.animalsKilled.AddValue(tt, 1);
                    }
                    else if (flora.Contains(tt))
                    {
                        //AddDebug(tt + " plant killed by player");
                        UnsavedData.plantsKilled.AddValue(tt, 1);
                    }
                    else if (coral.Contains(tt))
                    {
                        //AddDebug(tt + " coral killed by player");
                        UnsavedData.coralKilled.AddValue(tt, 1);
                    }
                    else if (leviathans.Contains(tt))
                    {
                        //AddDebug(tt + " leviathan killed by player");
                        UnsavedData.leviathansKilled.AddValue(tt, 1);
                    }
                }
            }
        }

        //[HarmonyPatch(typeof(Inventory))]
        internal class Inventory_Patch
        {
            //[HarmonyPostfix]
            //[HarmonyPatch("Pickup")]
            static void PickupPostfix(Inventory __instance, Pickupable pickupable, bool __result)
            {
                if (__result && !Player.main.pda.isInUse)
                {
                    //AddDebug("Inventory Pickup " + pickupable.GetTechType() + " " + __result);
                    TechType tt = pickupable.GetTechType();
                    UnsavedData.pickedUpItems.AddValue(tt, 1);
                }
            }
        }

        [HarmonyPatch(typeof(BlueprintHandTarget), "UnlockBlueprint")]
        internal class BlueprintHandTarget_UnlockBlueprint_Patch
        {
            public static void Prefix(BlueprintHandTarget __instance)
            {
                //AddDebug("UnlockBlueprint  " + __instance.unlockTechType);
                if (!ConfigMenu.modEnabled.Value || string.IsNullOrEmpty(saveSlot) || __instance.used)
                    return;

                if (!KnownTech.Contains(__instance.unlockTechType))
                {
                    //AddDebug("unlock  " + __instance.unlockTechType);
                    UnsavedData.blueprintsFromDatabox.Add(__instance.unlockTechType);
                }
            }
        }

        [HarmonyPatch(typeof(ScannerTool), "Scan")]
        internal class ScannerTool_Scan_Patch
        {
            public static void Postfix(ScannerTool __instance, PDAScanner.Result __result)
            {
                if (!ConfigMenu.modEnabled.Value)
                    return;

                if (__result == PDAScanner.Result.None || __result == PDAScanner.Result.Scan || __result == PDAScanner.Result.Known) { }
                else
                {
                    //AddDebug("result " + __result + " IsFragment " + fragment);
                    UnsavedData.objectsScanned++;
                    TechType tt = PDAScanner.scanTarget.techType;
                    //string name = tt.AsString();
                    if (creatures.Contains(tt))
                    {
                        //AddDebug("scanned creature");
                        UnsavedData.faunaFound.Add(tt);
                    }
                    else if (flora.Contains(tt))
                    {
                        //AddDebug("scanned flora");
                        UnsavedData.floraFound.Add(tt);
                    }
                    else if (coral.Contains(tt))
                    {
                        //AddDebug("scanned coral");
                        UnsavedData.coralFound.Add(tt);
                    }
                    else if (leviathans.Contains(tt))
                    {
                        //AddDebug("scanned leviathan");
                        UnsavedData.leviathanFound.Add(tt);
                    }

                }

            }
        }

        [HarmonyPatch(typeof(PDAScanner), "Unlock")]
        internal class PDAScanner_Unlock_Patch
        {
            public static void Postfix(PDAScanner.EntryData entryData, bool unlockBlueprint, bool unlockEncyclopedia, bool verbose)
            {
                if (!ConfigMenu.modEnabled.Value || entryData == null || !verbose || !unlockBlueprint)
                    return;

                //AddDebug(" scanned " + entryData.key);
                //AddDebug("unlock Blueprint " + entryData.blueprint);
                TechType tt = entryData.blueprint;
                if (tt != TechType.None)
                { // scanning bladderfish unlocks filteredWater blueprint
                    if (creatures.Contains(tt) || flora.Contains(tt) || coral.Contains(tt) || leviathans.Contains(tt))
                        return;

                    UnsavedData.blueprintsUnlocked.Add(tt);
                }
                //if (!PDAEncyclopedia.ContainsEntry(entryData.encyclopedia))
                //{
                //    AddDebug("unlock Encyclopedia " + entryData.encyclopedia);
                //}
                //if (!string.IsNullOrEmpty( entryData.encyclopedia))
                //    AddDebug("unlock Encyclopedia ");
            }
        }

        [HarmonyPatch(typeof(Base), "Start")]
        internal class Base_Start_Patch
        {
            public static void Postfix(Base __instance)
            {
                if (!ConfigMenu.modEnabled.Value || __instance.isGhost)
                    return;

                UnsavedData.bases.Add(__instance);
            }
        }

        [HarmonyPatch(typeof(Constructable))]
        internal class Constructable_Patch
        {
            //[HarmonyPrefix]
            //[HarmonyPatch("Construct")]
            public static void ConstructPrefix(Constructable __instance, bool __result)
            {
                //AddDebug(" Construct Prefix" + __instance.techType + " " + __instance.constructedAmount);
                if (__instance.techType == TechType.None || __instance.constructedAmount != 0)
                    return;

                //foreach (TechType tt in __instance.resourceMap)
                //{
                //    if (!itemMass.ContainsKey(tt))
                //        itemMass[tt] = GetItemMass(tt);
                //}
            }

            //[HarmonyPostfix]
            //[HarmonyPatch("Construct")]
            public static void ConstructPostfix(Constructable __instance, bool __result)
            {
                if (__instance.techType == TechType.None || __instance.constructedAmount < 1)
                    return;

                //AddDebug(" Construct " + __instance.techType);
                //AddDebug(" Construct resourceMap " + __instance.resourceMap.Count);
                //if (GameModeUtils.RequiresIngredients())
                //    SaveResourcesUsedToConstruct(__instance.resourceMap);
            }

            [HarmonyPostfix, HarmonyPatch("DeconstructAsync")]
            public static void DeconstructAsyncPostfix(Constructable __instance)
            {
                if (!ConfigMenu.modEnabled.Value)
                    return;

                if (__instance.constructedAmount <= 0f)
                {
                    //AddDebug(" deconstructed " + __instance.techType + " " + __instance.constructedAmount);
                    HandleDeconstruction(__instance.techType);
                }
            }

            private static void HandleDeconstruction(TechType techType)
            {
                if (UnsavedData.builderToolBuilt.ContainsKey(techType) && UnsavedData.builderToolBuilt[techType] > 0)
                {
                    UnsavedData.builderToolBuilt[techType]--;
                }
                else if (Main.configMain.builderToolBuilt.ContainsKey(saveSlot))
                {
                    string name = techType.AsString();
                    if (Main.configMain.builderToolBuilt[saveSlot].ContainsKey(name) && Main.configMain.builderToolBuilt[saveSlot][name] > 0)
                    {
                        Main.configMain.builderToolBuilt[saveSlot][name]--;
                        Main.configMain.Save();
                    }
                }

            }

        }

        [HarmonyPatch(typeof(Vehicle))]
        internal class Vehicle_Patch
        {
            [HarmonyPostfix, HarmonyPatch("OnKill")]
            public static void OnKillPostfix(Vehicle __instance)
            {
                if (!ConfigMenu.modEnabled.Value)
                    return;

                //AddDebug(" OnKill " + CraftData.GetTechType(__instance.gameObject));
                if (__instance is Exosuit)
                {
                    //AddDebug("Exosuit lost");
                    UnsavedData.vehiclesLost.AddValue(TechType.Exosuit, 1);
                }
                else
                {
                    TechType tt = CraftData.GetTechType(__instance.gameObject);
                    if (tt != TechType.None)
                        UnsavedData.vehiclesLost.AddValue(tt, 1);
                }
            }
            [HarmonyPostfix, HarmonyPatch("EnterVehicle")]
            public static void EnterVehicleostfix(Vehicle __instance)
            {
                currentVehicleTT = CraftData.GetTechType(__instance.gameObject);
                //AddDebug("currentVehicleTT " + currentVehicleTT);
            }
        }


        [HarmonyPatch(typeof(SeaTruckSegment), "OnKill")]
        class SeaTruckSegment_OnKill_Patch
        { // runs twice for every instance
            static HashSet<SeaTruckSegment> lostSTSs = new HashSet<SeaTruckSegment>();
            static void Postfix(SeaTruckSegment __instance)
            {
                if (!ConfigMenu.modEnabled.Value || lostSTSs.Contains(__instance))
                    return;

                TechType tt = CraftData.GetTechType(__instance.gameObject);
                //AddDebug("OnKill " + tt);
                if (tt != TechType.None)
                {
                    lostSTSs.Add(__instance);
                    UnsavedData.vehiclesLost.AddValue(tt, 1);
                }
            }
        }

        [HarmonyPatch(typeof(Hoverbike), "OnKill")]
        class Hoverbike_OnKill_Patch
        {
            static void Postfix(Hoverbike __instance)
            {
                if (ConfigMenu.modEnabled.Value)
                    UnsavedData.vehiclesLost.AddValue(TechType.Hoverbike, 1);
            }
        }

        [HarmonyPatch(typeof(ConstructorInput), "OnCraftingBegin")]
        internal class ConstructorInput_OnCraftingBegin_Patch
        {
            public static void Postfix(ConstructorInput __instance, TechType techType)
            {
                if (ConfigMenu.modEnabled.Value)
                    constructorBuilt.Add(techType);
                //AddDebug("Constructor OnCraftingBegin " + techType);
            }
        }

        [HarmonyPatch(typeof(Bed))]
        internal class Bed_Patch
        {
            static TimeSpan bedTimeStart = TimeSpan.Zero;

            [HarmonyPostfix, HarmonyPatch("EnterInUseMode")]
            public static void EnterInUseModePostfix(Bed __instance)
            {
                if (ConfigMenu.modEnabled.Value)
                    bedTimeStart = GetTimeSpanPlayed();
                //AddDebug("EnterInUseMode " );
            }
            [HarmonyPostfix, HarmonyPatch("ExitInUseMode")]
            public static void ExitInUseModePostfix(Bed __instance)
            {
                if (!ConfigMenu.modEnabled.Value || bedTimeStart == default)
                    return;

                TimeSpan timeSlept = GetTimeSpanPlayed() - bedTimeStart;
                UnsavedData.timeSlept += timeSlept;
                bedTimeStart = default;
                //AddDebug("ExitInUseMode " );
            }
        }

        [HarmonyPatch(typeof(LiveMixin), "TakeDamage")]
        class LiveMixin_TakeDamage_Patch
        {
            static void Postfix(LiveMixin __instance, DamageType type)
            {
                if (!ConfigMenu.modEnabled.Value)
                    return;

                if (__instance.gameObject.tag == "Player" && __instance.damageInfo.damage > 0)
                {
                    if (type == DamageType.Fire || type == DamageType.Heat)
                    {
                        float temp = WaterTemperatureSimulation.main.GetTemperature(__instance.transform.position);
                        //AddDebug("player fire damage " + (int)temp);
                        if (UnsavedData.maxTemp < temp)
                            UnsavedData.maxTemp = Mathf.RoundToInt(temp);
                    }
                    else if (type == DamageType.Cold)
                    {
                        float temp = WaterTemperatureSimulation.main.GetTemperature(__instance.transform.position);
                        //AddDebug("player Cold damage " + (int)temp);
                        if (UnsavedData.minTemp > temp)
                            UnsavedData.minTemp = Mathf.RoundToInt(temp);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CreatureEgg), "Hatch")]
        internal class CreatureEgg_Hatch_Patch
        {
            public static void Postfix(CreatureEgg __instance)
            {
                if (!ConfigMenu.modEnabled.Value)
                    return;

                TechType tt = __instance.creatureType;
                //AddDebug("CreatureEgg Hatch  " + tt);
                if (tt == TechType.None)
                    return;

                hatchedTT = tt;
                UnsavedData.eggsHatched.AddValue(tt, 1);
            }
        }

        [HarmonyPatch(typeof(WaterParkCreature), "InitializeCreatureBornInWaterPark")]
        class GWaterParkCreature_InitializeCreatureBornInWaterPark_patch
        {
            public static void Postfix(WaterParkCreature __instance)
            {
                if (!ConfigMenu.modEnabled.Value || Main.setupDone == false)
                    return;

                TechType tt = CraftData.GetTechType(__instance.gameObject);
                if (tt == TechType.None)
                    return;

                if (hatchedTT != TechType.None)
                {
                    hatchedTT = TechType.None;
                    return;
                }
                UnsavedData.creaturesBred.AddValue(tt, 1);
                //AddDebug("InitializeCreatureBornInWaterPark  " + tt);
            }
        }

    }
}
