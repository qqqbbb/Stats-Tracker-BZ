using HarmonyLib;
using mset;
using Nautilus.Handlers;
using Nautilus.Utility;
using ProtoBuf.Compiler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static ErrorMessage;


namespace Stats_Tracker
{
    internal class Stats_Display_old
    {
        public static string saveSlot;
        public static Dictionary<string, string> myStrings = new Dictionary<string, string>();
        public static Dictionary<string, string> descs = new Dictionary<string, string>();

        static TimeSpan GetTimePlayed()
        {
            if (ConfigMenu.modEnabled.Value)
                return Patches.GetTimeSpanPlayed();
            else if (Main.configMain.timePlayed.ContainsKey(saveSlot))
                return Main.configMain.timePlayed[saveSlot];
            else
                return Patches.GetTimeSpanPlayed();
        }

        private static int GetInt(Dictionary<string, int> configDic, int unsaved)
        {
            int total = unsaved;
            if (configDic.ContainsKey(saveSlot))
                total += configDic[saveSlot];

            return total;
        }

        private static float GetFloat(Dictionary<string, float> configDic, float unsaved)
        {
            float total = unsaved;
            if (configDic.ContainsKey(saveSlot))
                total += configDic[saveSlot];

            return total;
        }

        private static float GetFloatGlobal(Dictionary<string, Dictionary<string, float>> configDic)
        {
            float total = 0;
            foreach (var kv in configDic)
                total += kv.Value.Values.Sum();

            return total;
        }

        private static HashSet<string> GetSetGlobal(Dictionary<string, HashSet<string>> dic)
        {
            HashSet<string> newSet = new HashSet<string>();
            foreach (var kv in dic)
            {
                foreach (var s in kv.Value)
                    newSet.Add(s);
            }
            return newSet;
        }

        private static HashSet<string> MergeSets(Dictionary<string, HashSet<string>> configDic, HashSet<TechType> set)
        {
            HashSet<string> newSet = new HashSet<string>();
            if (configDic.ContainsKey(saveSlot))
            {
                foreach (var s in configDic[saveSlot])
                    newSet.Add(s);
            }
            foreach (var tt in set)
                newSet.Add(tt.AsString());

            return newSet;
        }

        public static TimeSpan GetSumOfDicValues(Dictionary<string, TimeSpan> dic)
        {
            TimeSpan total = TimeSpan.Zero;
            foreach (var kv in dic)
                total += kv.Value;

            return total;
        }

        public static void AppendTimeSpan(StringBuilder sb, TimeSpan time, string s, bool indent = false)
        {
            if (time.TotalMinutes < 1)
                return;

            if (indent)
                sb.Append("     " + Language.main.Get(s));
            else
                sb.Append(Language.main.Get(s));

            string day = time.Days == 1 ? Language.main.Get("ST_day") : Language.main.Get("ST_days");
            if (time.Days > 0)
                sb.Append(time.Days + " " + day);

            if (time.Days > 0 && (time.Hours > 0 || time.Minutes > 0))
                sb.Append(" ");

            string hour = time.Hours == 1 ? Language.main.Get("ST_hour") : Language.main.Get("ST_hours");
            if (time.Hours > 0)
                sb.Append(time.Hours + " " + hour);

            if (time.Hours > 0 && time.Minutes > 0)
                sb.Append(" ");

            string minute = time.Minutes == 1 ? Language.main.Get("ST_minute") : Language.main.Get("ST_minutes");
            if (time.Minutes > 0)
                sb.AppendLine(time.Minutes + " " + minute);
            else
                sb.AppendLine();
        }

        public static string AppendTimeSpan(TimeSpan time, string s)
        {
            if (time.TotalMinutes < 1)
                return s;

            StringBuilder sb = new StringBuilder(s);

            string day = time.Days == 1 ? Language.main.Get("ST_day") : Language.main.Get("ST_days");
            if (time.Days > 0)
                sb.Append(time.Days + " " + day);

            if (time.Days > 0 && (time.Hours > 0 || time.Minutes > 0))
                sb.Append(" ");

            string hour = time.Hours == 1 ? Language.main.Get("ST_hour") : Language.main.Get("ST_hours");
            if (time.Hours > 0)
                sb.Append(time.Hours + " " + hour);

            if (time.Hours > 0 && time.Minutes > 0)
                sb.Append(" ");

            string minute = time.Minutes == 1 ? Language.main.Get("ST_minute") : Language.main.Get("ST_minutes");
            if (time.Minutes > 0)
                sb.Append(time.Minutes + " " + minute);

            return sb.ToString();
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
            PDAHandler.AddEncyclopediaEntry(entry);
            //mapping[key] = entry;
            myStrings[key] = desc;
            descs["EncyDesc_" + key] = desc;
            LanguageHandler.SetLanguageLine("Ency_" + key, name);
            LanguageHandler.SetLanguageLine("EncyDesc_" + key, desc);
            //PDAEncyclopedia.Add(entryData.encyclopedia, false);
        }

        public static string GetTraveledString(int meters)
        {
            if (ConfigMenu.miles.Value)
            {
                int yards = Mathf.RoundToInt(Util.MeterToYard(meters));
                int miles = yards / 1760;
                int y = yards % 1760;
                StringBuilder sb = new StringBuilder();
                if (miles == 1)
                    sb.Append(miles + " mile");
                else if (miles > 1)
                    sb.Append(miles + " miles");

                if (miles > 0 && y > 0)
                    sb.Append(" ");

                if (y == 1)
                    sb.Append(y + " yard");
                else if (y > 1)
                    sb.Append(y + " yards");

                return sb.ToString();
            }
            else
            {
                int km = meters / 1000;
                int m = meters % 1000;
                StringBuilder sb = new StringBuilder();
                if (km == 1)
                    sb.Append(km + " " + Language.main.Get("ST_kilometer"));
                else if (km > 1)
                    sb.Append(km + " " + Language.main.Get("ST_kilometers"));

                if (km > 0 && m > 0)
                    sb.Append(" ");

                if (m == 1)
                    sb.Append(m + " " + Language.main.Get("ST_meter"));
                else if (m > 1)
                    sb.Append(m + " " + Language.main.Get("ST_meters"));

                //AddDebug(" GetTraveledString " + sb.ToString());
                return sb.ToString();
            }
        }

        public static void AddEntries()
        {
            AddPDAentry("ST_StatsGlobal", Language.main.Get("ST_global_statistics"), "", "ST_stats");
            AddPDAentry("ST_StatsThisGame", Language.main.Get("ST_current_game_statistics"), "", "ST_stats");
            //LanguageHandler.SetLanguageLine("EncyPath_ST_Stats", "ST_statistics");
        }

        private static Dictionary<string, int> MergeDics(Dictionary<string, Dictionary<string, int>> configDic, Dictionary<TechType, int> unsavedDic)
        {
            Dictionary<string, int> newDic;
            if (configDic.ContainsKey(saveSlot))
                newDic = new Dictionary<string, int>(configDic[saveSlot]);
            else
                newDic = new Dictionary<string, int>();

            foreach (var kv in unsavedDic)
                newDic.AddValue(kv.Key.ToString(), kv.Value);

            return newDic;
        }


        private static Dictionary<string, TimeSpan> MergeDics(Dictionary<string, Dictionary<string, TimeSpan>> configDic, Dictionary<TechType, TimeSpan> unsavedDic)
        {
            Dictionary<string, TimeSpan> newDic;
            if (configDic.ContainsKey(saveSlot))
                newDic = new Dictionary<string, TimeSpan>(configDic[saveSlot]);
            else
                newDic = new Dictionary<string, TimeSpan>();

            foreach (var kv in unsavedDic)
                newDic.AddValue(kv.Key.ToString(), kv.Value);

            return newDic;
        }

        private static Dictionary<string, TimeSpan> GetDic(Dictionary<string, Dictionary<string, TimeSpan>> configDic, Dictionary<TechType, TimeSpan> unsavedDic)
        {
            Dictionary<string, TimeSpan> newDic;
            if (configDic.ContainsKey(saveSlot))
                newDic = new Dictionary<string, TimeSpan>(configDic[saveSlot]);
            else
                newDic = new Dictionary<string, TimeSpan>();

            foreach (var kv in unsavedDic)
                newDic.AddValue(kv.Key.ToString(), kv.Value);

            return newDic;
        }

        private static Dictionary<string, int> GetDicGlobal(Dictionary<string, Dictionary<string, int>> configDic)
        {
            Dictionary<string, int> newDic = new Dictionary<string, int>();
            foreach (var kv1 in configDic)
            {
                foreach (var kv in kv1.Value)
                    newDic.AddValue(kv.Key, kv.Value);
            }
            return newDic;
        }

        private static Dictionary<string, float> GetDicGlobal(Dictionary<string, Dictionary<string, float>> configDic)
        {
            Dictionary<string, float> newDic = new Dictionary<string, float>();
            foreach (var kv1 in configDic)
            {
                foreach (var kv in kv1.Value)
                    newDic.AddValue(kv.Key, kv.Value);
            }
            return newDic;
        }

        private static Dictionary<string, float> GetDic(Dictionary<string, Dictionary<string, float>> configDic, Dictionary<TechType, float> unsavedDic)
        {
            Dictionary<string, float> newDic;
            if (configDic.ContainsKey(saveSlot))
                newDic = new Dictionary<string, float>(configDic[saveSlot]);
            else
                newDic = new Dictionary<string, float>();

            foreach (var kv in unsavedDic)
                newDic.AddValue(kv.Key.ToString(), kv.Value);

            return newDic;
        }

        private static Dictionary<string, TimeSpan> MergeDics(Dictionary<string, Dictionary<string, TimeSpan>> configDic, Dictionary<string, TimeSpan> unsavedDic)
        {
            Dictionary<string, TimeSpan> newDic;
            if (configDic.ContainsKey(saveSlot))
                newDic = new Dictionary<string, TimeSpan>(configDic[saveSlot]);
            else
                newDic = new Dictionary<string, TimeSpan>();

            foreach (var kv in unsavedDic)
                newDic.AddValue(kv.Key.ToString(), kv.Value);

            return newDic;
        }

        private static Dictionary<string, TimeSpan> GetDicGlobal(Dictionary<string, Dictionary<string, TimeSpan>> configDic)
        {
            Dictionary<string, TimeSpan> newDic = new Dictionary<string, TimeSpan>();
            foreach (var kv1 in configDic)
            {
                foreach (var kv in kv1.Value)
                    newDic.AddValue(kv.Key, kv.Value);
            }
            return newDic;
        }

        private static void GetTimeStats(StringBuilder sb)
        {
            AppendTimeSpan(sb, GetTimePlayed(), "ST_time_since_landing");
            TimeSpan timeOnFeet = Main.configMain.timeWalked.ContainsKey(saveSlot) ? Main.configMain.timeWalked[saveSlot] + UnsavedData.timeWalked : UnsavedData.timeWalked;
            //AddDebug("timeOnFeet " + timeOnFeet);
            AppendTimeSpan(sb, timeOnFeet, "ST_time_on_feet");
            TimeSpan timeSat = Main.configMain.timeSat.ContainsKey(saveSlot) ? Main.configMain.timeSat[saveSlot] + UnsavedData.timeSat : UnsavedData.timeSat;
            AppendTimeSpan(sb, timeSat, "ST_time_sat");
            TimeSpan timeSwam = Main.configMain.timeSwam.ContainsKey(saveSlot) ? Main.configMain.timeSwam[saveSlot] + UnsavedData.timeSwam : UnsavedData.timeSwam;
            AppendTimeSpan(sb, timeSwam, "ST_time_swimming");
            TimeSpan timeSlept = Main.configMain.timeSlept.ContainsKey(saveSlot) ? Main.configMain.timeSlept[saveSlot] + UnsavedData.timeSlept : UnsavedData.timeSlept;
            AppendTimeSpan(sb, timeSlept, "ST_time_sleeping");
            TimeSpan timeEscapePod = Main.configMain.timeEscapePod.ContainsKey(saveSlot) ? Main.configMain.timeEscapePod[saveSlot] + UnsavedData.timeEscapePod : UnsavedData.timeEscapePod;
            AppendTimeSpan(sb, timeEscapePod, "ST_time_escape_pod");
            TimeSpan timeBase = Main.configMain.timeBase.ContainsKey(saveSlot) ? Main.configMain.timeBase[saveSlot] + UnsavedData.timeBase : UnsavedData.timeBase;
            AppendTimeSpan(sb, timeBase, "ST_time_base");
            TimeSpan timePrecursor = Main.configMain.timePrecursor.ContainsKey(saveSlot) ? Main.configMain.timePrecursor[saveSlot] + UnsavedData.timePrecursor : UnsavedData.timePrecursor;
            AppendTimeSpan(sb, timePrecursor, "ST_time_precursor");
            AppendTimeDicWithSumInTitle(sb, MergeDics(Main.configMain.timeVehicles, UnsavedData.timeVehicles), "ST_time_vehicles");
        }

        private static void GetTimeGlobalStats(StringBuilder sb)
        {
            AppendTimeSpan(sb, GetSumOfDicValues(Main.configMain.timePlayed), "ST_time_since_landing");
            AppendTimeSpan(sb, GetSumOfDicValues(Main.configMain.timeWalked), "ST_time_on_feet");
            AppendTimeSpan(sb, GetSumOfDicValues(Main.configMain.timeSat), "ST_time_sat");
            AppendTimeSpan(sb, GetSumOfDicValues(Main.configMain.timeSwam), "ST_time_swimming");
            AppendTimeSpan(sb, GetSumOfDicValues(Main.configMain.timeSlept), "ST_time_sleeping");
            AppendTimeSpan(sb, GetSumOfDicValues(Main.configMain.timeEscapePod), "ST_time_escape_pod");
            AppendTimeSpan(sb, GetSumOfDicValues(Main.configMain.timeBase), "ST_time_base");
            AppendTimeSpan(sb, GetSumOfDicValues(Main.configMain.timePrecursor), "ST_time_precursor");
            AppendTimeDicWithSumInTitle(sb, Main.configMain.timeVehicles, "ST_time_vehicles");
        }

        private static void AppendTimeDicWithSumInTitle(StringBuilder sb, Dictionary<string, TimeSpan> dic, string title)
        {
            TimeSpan totalTime = TimeSpan.Zero;
            foreach (var d in dic)
                totalTime += d.Value;

            if (totalTime == TimeSpan.Zero)
            {
                sb.AppendLine();
                return;
            }
            title = AppendTimeSpan(totalTime, Language.main.Get(title));
            AppendTimeDic(sb, dic, title);
        }

        private static void AppendTimeDicWithSumInTitle(StringBuilder sb, Dictionary<string, Dictionary<string, TimeSpan>> dic, string title)
        {
            TimeSpan totalTime = TimeSpan.Zero;
            foreach (var d in dic)
                totalTime += GetSumOfDicValues(d.Value);

            if (totalTime == TimeSpan.Zero)
            {
                sb.AppendLine();
                return;
            }
            title = AppendTimeSpan(totalTime, Language.main.Get(title));
            AppendTimeDic(sb, GetDicGlobal(dic), title);
        }

        private static void AppendTravelLine(StringBuilder sb, string s, int dist, bool indent = false)
        {
            if (dist <= 0)
                return;

            if (indent)
                sb.AppendLine("     " + Language.main.Get(s) + GetTraveledString(dist));
            else
                sb.AppendLine(Language.main.Get(s) + GetTraveledString(dist));
        }

        private static void GetTravelStats(StringBuilder sb)
        {
            int distanceTraveled = Main.configMain.distanceTraveled.ContainsKey(saveSlot) ? Main.configMain.distanceTraveled[saveSlot] + UnsavedData.distanceTraveled : UnsavedData.distanceTraveled;
            AppendTravelLine(sb, "ST_distance_traveled", distanceTraveled);
            int distanceTraveledSwim = Main.configMain.distanceTraveledSwim.ContainsKey(saveSlot) ? Main.configMain.distanceTraveledSwim[saveSlot] + UnsavedData.distanceTraveledSwim : UnsavedData.distanceTraveledSwim;
            AppendTravelLine(sb, "ST_distance_swam", distanceTraveledSwim);
            int distanceTraveledWalk = Main.configMain.distanceTraveledWalk.ContainsKey(saveSlot) ? Main.configMain.distanceTraveledWalk[saveSlot] + UnsavedData.distanceTraveledWalk : UnsavedData.distanceTraveledWalk;
            AppendTravelLine(sb, "ST_distance_walked", distanceTraveledWalk);
            int distanceTraveledSeaglide = Main.configMain.distanceTraveledSeaglide.ContainsKey(saveSlot) ? Main.configMain.distanceTraveledSeaglide[saveSlot] + UnsavedData.distanceTraveledSeaglide : UnsavedData.distanceTraveledSeaglide;
            AppendTravelLine(sb, "ST_distance_seaglide", distanceTraveledSeaglide);
            int maxDepth = Main.configMain.maxDepth.ContainsKey(saveSlot) && Main.configMain.maxDepth[saveSlot] > UnsavedData.maxDepth ? Main.configMain.maxDepth[saveSlot] : UnsavedData.maxDepth;
            AppendTravelLine(sb, "ST_max_depth", maxDepth);
            Dictionary<string, int> traveledVehicles = MergeDics(Main.configMain.distanceTraveledVehicle, UnsavedData.distanceTraveledVehicle);
            AppendVehicleTravel(sb, traveledVehicles);
            sb.AppendLine();
        }

        private static void AppendVehicleTravel(StringBuilder sb, Dictionary<string, int> traveledVehicles)
        {
            if (traveledVehicles.Count == 0)
                return;

            AppendTravelLine(sb, "ST_distance_vehicles", traveledVehicles.Values.Sum());
            SortedDictionary<string, int> sortedDic = GetTranslatedSortedDic(traveledVehicles);
            foreach (var kv in sortedDic)
                AppendTravelLine(sb, kv.Key + ": ", kv.Value, true);
        }

        private static void GetTravelGlobalStats(StringBuilder sb)
        {
            AppendTravelLine(sb, "ST_distance_traveled", Main.configMain.distanceTraveled.Values.Sum());
            AppendTravelLine(sb, "ST_distance_swam", Main.configMain.distanceTraveledSwim.Values.Sum());
            AppendTravelLine(sb, "ST_distance_walked", Main.configMain.distanceTraveledWalk.Values.Sum());
            AppendTravelLine(sb, "ST_distance_seaglide", Main.configMain.distanceTraveledSeaglide.Values.Sum());
            AppendTravelLine(sb, "ST_max_depth", Main.configMain.maxDepth.Values.Max());
            Dictionary<string, int> traveledVehicles = GetDicGlobal(Main.configMain.distanceTraveledVehicle);
            AppendVehicleTravel(sb, traveledVehicles);
            sb.AppendLine();
        }

        private static void GetKilledStats(StringBuilder sb)
        {
            AppendDic(sb, MergeDics(Main.configMain.plantsKilled, UnsavedData.plantsKilled), "ST_plants_killed");
            AppendDic(sb, MergeDics(Main.configMain.animalsKilled, UnsavedData.animalsKilled), "ST_animals_killed");
            AppendDic(sb, MergeDics(Main.configMain.coralKilled, UnsavedData.coralKilled), "ST_corals_killed");
            AppendDic(sb, MergeDics(Main.configMain.leviathansKilled, UnsavedData.leviathansKilled), "ST_leviathans_killed");
        }

        private static void GetKilledGlobalStats(StringBuilder sb)
        {
            AppendDic(sb, GetDicGlobal(Main.configMain.plantsKilled), "ST_plants_killed");
            AppendDic(sb, GetDicGlobal(Main.configMain.animalsKilled), "ST_animals_killed");
            AppendDic(sb, GetDicGlobal(Main.configMain.coralKilled), "ST_corals_killed");
            AppendDic(sb, GetDicGlobal(Main.configMain.leviathansKilled), "ST_leviathans_killed");
        }

        private static string GetTempSuffix()
        {
            if (ConfigMenu.fahrenhiet.Value)
                return "°F";
            else
                return "°C";
        }

        private static void GetHealthStats(StringBuilder sb)
        {
            //if (GameModeManager.GetOption<bool>(GameOption.PlayerDamageTakenModifier))
            //    return;

            int deaths = GetInt(Main.configMain.playerDeaths, UnsavedData.playerDeaths);
            if (deaths > 0)
                sb.AppendLine(Language.main.Get("ST_deaths") + deaths);

            int healthLost = GetInt(Main.configMain.healthLost, UnsavedData.healthLost);
            int medkitsUsed = GetInt(Main.configMain.medkitsUsed, UnsavedData.medkitsUsed);
            if (healthLost > 0)
                sb.AppendLine(Language.main.Get("ST_health_lost") + healthLost);

            if (medkitsUsed > 0)
                sb.AppendLine(Language.main.Get("ST_med_kits_used") + medkitsUsed);

            string tempSuffix = GetTempSuffix();
            int minTemp = GetMinTemp();
            int maxTemp = GetMaxTemp();
            int minVehicleTemp = GetMinVehicleTemp();
            int maxVehicleTemp = GetMaxVehicleTemp();

            if (minTemp != int.MaxValue)
                sb.AppendLine(Language.main.Get("ST_min_temp") + minTemp + tempSuffix);

            if (maxTemp != int.MinValue)
                sb.AppendLine(Language.main.Get("ST_max_temp") + maxTemp + tempSuffix);

            if (minVehicleTemp != int.MaxValue)
                sb.AppendLine(Language.main.Get("ST_min_vehicle_temp") + minVehicleTemp + tempSuffix);

            if (maxVehicleTemp != int.MinValue)
                sb.AppendLine(Language.main.Get("ST_max_vehicle_temp") + maxVehicleTemp + tempSuffix);
            //if (deaths > 0 || medkitsUsed > 0 || healthLost > 0)
            sb.AppendLine();
        }

        private static int GetMinTemp()
        {
            int temp = UnsavedData.minTemp;
            if (Main.configMain.minTemp.ContainsKey(saveSlot) && Main.configMain.minTemp[saveSlot] < temp)
                temp = Main.configMain.minTemp[saveSlot];

            if (ConfigMenu.fahrenhiet.Value)
                temp = Mathf.RoundToInt(Util.CelciusToFahrenhiet(temp));

            return temp;
        }

        private static int GetMaxTemp()
        {
            int temp = UnsavedData.maxTemp;
            if (Main.configMain.maxTemp.ContainsKey(saveSlot) && Main.configMain.maxTemp[saveSlot] > temp)
                temp = Main.configMain.maxTemp[saveSlot];

            if (ConfigMenu.fahrenhiet.Value)
                temp = Mathf.RoundToInt(Util.CelciusToFahrenhiet(temp));

            return temp;
        }

        private static int GetMinVehicleTemp()
        {
            int temp = UnsavedData.minVehicleTemp;
            if (Main.configMain.minVehicleTemp.ContainsKey(saveSlot) && Main.configMain.minVehicleTemp[saveSlot] < temp)
                temp = Main.configMain.minVehicleTemp[saveSlot];

            if (temp != int.MaxValue && ConfigMenu.fahrenhiet.Value)
                temp = Mathf.RoundToInt(Util.CelciusToFahrenhiet(temp));

            return temp;
        }

        private static int GetMaxVehicleTemp()
        {
            int temp = UnsavedData.maxVehicleTemp;
            if (Main.configMain.maxVehicleTemp.ContainsKey(saveSlot) && Main.configMain.maxVehicleTemp[saveSlot] > temp)
                temp = Main.configMain.maxVehicleTemp[saveSlot];

            if (temp != int.MinValue && ConfigMenu.fahrenhiet.Value)
                temp = Mathf.RoundToInt(Util.CelciusToFahrenhiet(temp));

            return temp;
        }

        private static void GetHealthGlobalStats(StringBuilder sb)
        {
            int healthLost = Main.configMain.healthLost.Values.Sum();
            int medkitsUsed = Main.configMain.medkitsUsed.Values.Sum();
            int deaths = Main.configMain.playerDeaths.Values.Sum() + Main.configMain.permaDeaths;
            int minTemp = Main.configMain.minTemp.Values.Min();
            int maxTemp = Main.configMain.maxTemp.Values.Max();
            int minVehicleTemp = Main.configMain.minVehicleTemp.Values.Min();
            int maxVehicleTemp = Main.configMain.maxVehicleTemp.Values.Max();
            int sbLength = sb.Length;
            string tempSuffix = GetTempSuffix();

            if (ConfigMenu.fahrenhiet.Value)
            {
                maxTemp = Mathf.RoundToInt(Util.CelciusToFahrenhiet(maxTemp));
                minTemp = Mathf.RoundToInt(Util.CelciusToFahrenhiet(minTemp));
                maxVehicleTemp = Mathf.RoundToInt(Util.CelciusToFahrenhiet(maxVehicleTemp));
                minVehicleTemp = Mathf.RoundToInt(Util.CelciusToFahrenhiet(minVehicleTemp));
            }
            if (deaths > 0)
                sb.AppendLine(Language.main.Get("ST_deaths") + deaths);

            if (healthLost > 0)
                sb.AppendLine(Language.main.Get("ST_health_lost") + healthLost);

            if (medkitsUsed > 0)
                sb.AppendLine(Language.main.Get("ST_med_kits_used") + medkitsUsed);

            if (minTemp != int.MaxValue)
                sb.AppendLine(Language.main.Get("ST_min_temp") + minTemp + tempSuffix);

            if (maxTemp != int.MinValue)
                sb.AppendLine(Language.main.Get("ST_max_temp") + maxTemp + tempSuffix);

            if (minVehicleTemp != int.MaxValue)
                sb.AppendLine(Language.main.Get("ST_min_vehicle_temp") + minVehicleTemp + tempSuffix);

            if (maxVehicleTemp != int.MinValue)
                sb.AppendLine(Language.main.Get("ST_max_vehicle_temp") + maxVehicleTemp + tempSuffix);

            if (sb.Length != sbLength)
                sb.AppendLine();
        }

        private static void GetFoodStats(StringBuilder sb)
        {
            if (!GameModeManager.GetOption<bool>(GameOption.Hunger))
                return;

            float waterTotal = GetFloat(Main.configMain.waterDrunk, UnsavedData.waterDrunk);
            AppendWater(sb, waterTotal);
            Dictionary<string, float> dic = GetDic(Main.configMain.foodEaten, UnsavedData.foodEaten);
            float foodTotal = dic.Values.Sum();
            AppendFood(sb, foodTotal, dic);
            if (waterTotal > 0 || foodTotal > 0)
                sb.AppendLine();
        }

        private static void AppendFood(StringBuilder sb, float foodTotal, Dictionary<string, float> dic)
        {
            if (foodTotal == 0)
                return;

            string kgLoc = ConfigMenu.pounds.Value ? " " + Language.main.Get("ST_pounds") : " " + Language.main.Get("ST_kilograms");
            if (ConfigMenu.pounds.Value)
                foodTotal = Util.KiloToPound(foodTotal);

            sb.AppendLine(Language.main.Get("ST_food_eaten") + foodTotal.ToString("0.0") + kgLoc);
            SortedDictionary<string, float> sortedDic = GetTranslatedSortedDic(dic);
            foreach (var kv in sortedDic)
            {
                float kg = kv.Value;
                if (ConfigMenu.pounds.Value)
                    kg = Util.KiloToPound(kg);

                sb.AppendLine("     " + kv.Key + ": " + kg.ToString("0.0") + kgLoc);
            }
        }

        private static void GetFoodGlobalStats(StringBuilder sb)
        {
            float waterTotal = Main.configMain.waterDrunk.Values.Sum();
            AppendWater(sb, waterTotal);
            float foodTotal = GetFloatGlobal(Main.configMain.foodEaten);
            AppendFood(sb, foodTotal, GetDicGlobal(Main.configMain.foodEaten));
            if (waterTotal > 0 || foodTotal > 0)
                sb.AppendLine();
        }

        private static void AppendWater(StringBuilder sb, float water)
        {
            if (water == 0)
                return;

            if (ConfigMenu.pounds.Value)
                sb.AppendLine(Language.main.Get("ST_water_drunk") + Util.literToGallon(water).ToString("0.0") + " " + Language.main.Get("ST_gallons"));
            else
                sb.AppendLine(Language.main.Get("ST_water_drunk") + water.ToString("0.0") + " " + Language.main.Get("ST_liters"));
        }

        private static void GetBaseStats(StringBuilder sb)
        {
            Dictionary<TechType, int> roomsBuilt = UnsavedData.GetRoomsDic();
            int corridorsBuilt = UnsavedData.GetCorridorsBuilt();
            //AddDebug("sb length " + sb.Length);
            //AddDebug("roomsBuilt " + roomsBuilt);
            //AddDebug("corridorsBuilt " + corridorsBuilt);
            AppendDic(sb, GetBasePowerDic(), "ST_base_power_generated");
            if (corridorsBuilt > 0)
                sb.AppendLine(Language.main.Get("ST_base_corridors_built") + corridorsBuilt);

            AppendDic(sb, roomsBuilt, "ST_base_rooms_built");
            if (roomsBuilt.Count == 0 && corridorsBuilt > 0)
                sb.AppendLine();
        }

        private static Dictionary<TechType, int> GetBasePowerDic()
        {
            Dictionary<TechType, int> basePower = new Dictionary<TechType, int>();
            foreach (PowerSource ps in UnsavedData.basePowerSources)
            {
                if (ps == null)
                    continue;

                int power = Mathf.RoundToInt(ps.power);
                if (ps == null || (int)ps.power < 1)
                    continue;
                //AddDebug("GetBasePowerDic " + ps.name + " " + power);
                TechType tt = CraftData.GetTechType(ps.gameObject);
                if (Patches.basePowerSourceTypes.Contains(tt))
                    basePower.AddValue(tt, power);
            }
            return basePower;
        }

        private static void GetBaseGlobalStats(StringBuilder sb)
        {
            Dictionary<string, int> roomsBuilt = GetDicGlobal(Main.configMain.baseRoomsBuilt);
            int corridorsBuilt = Main.configMain.baseCorridorsBuilt.Values.Sum();
            //AddDebug("roomsBuilt " + roomsBuilt.Count);
            //AddDebug("corridorsBuilt " + corridorsBuilt);
            AppendDic(sb, GetDicGlobal(Main.configMain.basePower), "ST_base_power_generated");
            if (corridorsBuilt > 0)
                sb.AppendLine(Language.main.Get("ST_base_corridors_built") + corridorsBuilt);

            AppendDic(sb, roomsBuilt, "ST_base_rooms_built");
            if (roomsBuilt.Count == 0 && corridorsBuilt > 0)
                sb.AppendLine();
        }

        private static void GetUnlockedStats(StringBuilder sb)
        {
            int numObjectsScanned = GetInt(Main.configMain.objectsScanned, UnsavedData.objectsScanned);
            if (numObjectsScanned > 0)
                sb.AppendLine(Language.main.Get("ST_objects_scanned") + numObjectsScanned);

            int sbLength = sb.Length;
            AppendSet(sb, MergeSets(Main.configMain.blueprintsUnlocked, UnsavedData.blueprintsUnlocked), "ST_scanned_blueprints");
            AppendSet(sb, MergeSets(Main.configMain.blueprintsFromDatabox, UnsavedData.blueprintsFromDatabox), "ST_databox_blueprints");

            if (numObjectsScanned > 0 && sbLength == sb.Length)
                sb.AppendLine();
        }

        private static void GetUnlockedGlobalStats(StringBuilder sb)
        {
            int numObjectsScanned = Main.configMain.objectsScanned.Values.Sum();
            if (numObjectsScanned > 0)
                sb.AppendLine(Language.main.Get("ST_objects_scanned") + numObjectsScanned);

            int sbLength = sb.Length;
            AppendSet(sb, GetSetGlobal(Main.configMain.blueprintsUnlocked), "ST_scanned_blueprints");
            AppendSet(sb, GetSetGlobal(Main.configMain.blueprintsFromDatabox), "ST_databox_blueprints");

            if (numObjectsScanned > 0 && sbLength == sb.Length)
                sb.AppendLine();
        }

        private static void AppendDic(StringBuilder sb, Dictionary<string, int> dic, string title)
        {
            int ValuesSum = dic.Values.Sum();
            if (dic.Count == 0 || ValuesSum == 0)
                return;

            //AddDebug("AppendDic " + title);
            sb.AppendLine(Language.main.Get(title) + ValuesSum);
            foreach (var kv in GetTranslatedSortedDic(dic))
                sb.AppendLine("     " + kv.Key + ": " + kv.Value);

            sb.AppendLine();
        }

        private static SortedDictionary<string, int> GetTranslatedSortedDic(Dictionary<string, int> dic)
        {
            SortedDictionary<string, int> sortedDic = new SortedDictionary<string, int>();
            foreach (var kv in dic)
            {
                if (kv.Value > 0)
                    sortedDic.Add(Language.main.Get(kv.Key), kv.Value);
            }
            return sortedDic;
        }

        private static SortedDictionary<string, int> GetTranslatedSortedDic(Dictionary<TechType, int> dic)
        {
            SortedDictionary<string, int> sortedDic = new SortedDictionary<string, int>();
            foreach (var kv in dic)
            {
                if (kv.Value > 0)
                    sortedDic.Add(Language.main.Get(kv.Key), kv.Value);
            }
            return sortedDic;
        }

        private static SortedDictionary<string, float> GetTranslatedSortedDic(Dictionary<string, float> dic)
        {
            SortedDictionary<string, float> sortedDic = new SortedDictionary<string, float>();
            foreach (var kv in dic)
            {
                if (kv.Value > 0)
                    sortedDic.Add(Language.main.Get(kv.Key), kv.Value);
            }
            return sortedDic;
        }

        private static SortedDictionary<string, TimeSpan> GetTranslatedSortedDic(Dictionary<string, TimeSpan> dic)
        {
            SortedDictionary<string, TimeSpan> sortedDic = new SortedDictionary<string, TimeSpan>();
            foreach (var kv in dic)
            {
                if (kv.Value.TotalMinutes >= 1)
                    sortedDic.Add(Language.main.Get(kv.Key), kv.Value);
            }
            return sortedDic;
        }

        private static void AppendDic(StringBuilder sb, Dictionary<TechType, int> dic, string title)
        {
            int ValuesSum = dic.Values.Sum();
            if (dic.Count == 0 || ValuesSum == 0)
                return;

            sb.AppendLine(Language.main.Get(title) + ValuesSum);
            SortedDictionary<string, int> sortedDic = GetTranslatedSortedDic(dic);
            foreach (var kv in sortedDic)
                sb.AppendLine("     " + kv.Key + ": " + kv.Value);

            sb.AppendLine();
        }

        private static void AppendTimeDic(StringBuilder sb, Dictionary<string, TimeSpan> dic, string title)
        {
            if (dic.Count == 0)
                return;

            SortedDictionary<string, TimeSpan> sortedDic = GetTranslatedSortedDic(dic);
            sb.AppendLine(Language.main.Get(title));
            foreach (var kv in sortedDic)
                AppendTimeSpan(sb, kv.Value, kv.Key + ": ", true);

            sb.AppendLine();
        }

        private static void AppendSet(StringBuilder sb, HashSet<string> set, string title)
        {
            if (set.Count == 0)
                return;

            sb.AppendLine(Language.main.Get(title));
            SortedSet<string> sortedSet = GetTranslatedSortedSet(set);
            foreach (var s in sortedSet)
                sb.AppendLine("     " + s);

            sb.AppendLine();
        }

        private static SortedSet<string> GetTranslatedSortedSet(HashSet<string> set)
        {
            SortedSet<string> sortedSet = new SortedSet<string>();
            foreach (var s in set)
                sortedSet.Add(Language.main.Get(s));

            return sortedSet;
        }

        private static void GetDiscoverStats(StringBuilder sb)
        {
            AppendSet(sb, MergeSets(Main.configMain.faunaFound, UnsavedData.faunaFound), "ST_fauna_discovered");
            AppendSet(sb, MergeSets(Main.configMain.floraFound, UnsavedData.floraFound), "ST_flora_discovered");
            AppendSet(sb, MergeSets(Main.configMain.coralFound, UnsavedData.coralFound), "ST_corals_discovered");
            AppendSet(sb, MergeSets(Main.configMain.leviathanFound, UnsavedData.leviathanFound), "ST_leviathans_discovered");
        }
        private static void GetGlobalDiscoverStats(StringBuilder sb)
        {
            AppendSet(sb, GetSetGlobal(Main.configMain.faunaFound), "ST_fauna_discovered");
            AppendSet(sb, GetSetGlobal(Main.configMain.floraFound), "ST_flora_discovered");
            AppendSet(sb, GetSetGlobal(Main.configMain.coralFound), "ST_corals_discovered");
            AppendSet(sb, GetSetGlobal(Main.configMain.leviathanFound), "ST_leviathans_discovered");
        }

        private static string GetCurrentStats()
        {
            //AddDebug("GetCurrentStats");
            string biomeName = Language.main.Get(Util.GetBiomeName());
            StringBuilder sb = new StringBuilder();
            if (!ConfigMenu.modEnabled.Value)
                sb.Append(Language.main.Get("ST_mod_disabled") + "\n" + "\n");

            sb.Append(Language.main.Get("ST_current_biome") + biomeName + "\n");
            sb.AppendLine();
            GetTimeStats(sb);
            GetTravelStats(sb);
            GetHealthStats(sb);
            GetFoodStats(sb);
            GetBaseStats(sb);
            AppendDic(sb, MergeDics(Main.configMain.constructorBuilt, UnsavedData.constructorBuilt), "ST_constructor_built");
            AppendDic(sb, MergeDics(Main.configMain.vehiclesLost, UnsavedData.vehiclesLost), "ST_vehicles_lost");
            AppendDic(sb, MergeDics(Main.configMain.builderToolBuilt, UnsavedData.builderToolBuilt), "ST_builder_tool_built");
            GetUnlockedStats(sb);
            AppendTimeDic(sb, MergeDics(Main.configMain.timeBiomes, UnsavedData.timeBiomes), "ST_time_biomes");
            AppendDic(sb, MergeDics(Main.configMain.itemsCrafted, UnsavedData.itemsCrafted), "ST_items_crafted");
            AppendDic(sb, MergeDics(Main.configMain.plantsGrown, UnsavedData.plantsGrown), "ST_plants_grown");
            AppendDic(sb, MergeDics(Main.configMain.eggsHatched, UnsavedData.eggsHatched), "ST_hatched_eggs");
            AppendDic(sb, MergeDics(Main.configMain.creaturesBred, UnsavedData.creaturesBred), "ST_creatures_bred");
            GetDiscoverStats(sb);
            GetKilledStats(sb);
            return sb.ToString();
        }

        private static string GetGlobalStats()
        {
            StringBuilder sb = new StringBuilder();
            AppendGamesWon(sb);
            GetTimeGlobalStats(sb);
            GetTravelGlobalStats(sb);
            GetHealthGlobalStats(sb);
            GetFoodGlobalStats(sb);
            GetBaseGlobalStats(sb);
            AppendDic(sb, GetDicGlobal(Main.configMain.constructorBuilt), "ST_constructor_built");
            AppendDic(sb, GetDicGlobal(Main.configMain.vehiclesLost), "ST_vehicles_lost");
            AppendDic(sb, GetDicGlobal(Main.configMain.builderToolBuilt), "ST_builder_tool_built");
            GetUnlockedGlobalStats(sb);
            AppendTimeDic(sb, GetDicGlobal(Main.configMain.timeBiomes), "ST_time_biomes");
            AppendDic(sb, GetDicGlobal(Main.configMain.itemsCrafted), "ST_items_crafted");
            AppendDic(sb, GetDicGlobal(Main.configMain.plantsGrown), "ST_plants_grown");
            AppendDic(sb, GetDicGlobal(Main.configMain.eggsHatched), "ST_hatched_eggs");
            AppendDic(sb, GetDicGlobal(Main.configMain.creaturesBred), "ST_creatures_bred");
            GetGlobalDiscoverStats(sb);
            GetKilledGlobalStats(sb);
            return sb.ToString();
        }

        private static void AppendGamesWon(StringBuilder sb)
        {
            if (Main.configMain.gamesWon > 0)
                sb.AppendLine(Language.main.Get("ST_games_won") + " " + Main.configMain.gamesWon);
        }

        [HarmonyPatch(typeof(Language), "TryGet")]
        internal class Language_TryGet_Patch
        {
            public static void Postfix(Language __instance, string key, ref string result)
            {
                if (!Main.setupDone || descs == null || key == null || !descs.ContainsKey(key))
                    return;
                //AddDebug("TryGet " + key);
                if (key == "EncyDesc_ST_StatsThisGame")
                {
                    result = GetCurrentStats();
                }
                else if (key == "EncyDesc_ST_StatsGlobal")
                {
                    result = GetGlobalStats();
                }
            }
        }

        [HarmonyPatch(typeof(uGUI_EncyclopediaTab))]
        internal class uGUI_EncyclopediaTab_Patch
        {
            static CraftNode lastEncNode;

            [HarmonyPostfix]
            [HarmonyPatch("Open")]
            public static void OpenPostfix(uGUI_EncyclopediaTab __instance)
            {
                if (lastEncNode != null && myStrings.ContainsKey(lastEncNode.id))
                { // update stats 
                  //AddDebug("update tab");
                    __instance.activeEntry = null;
                    __instance.Activate(lastEncNode);
                }
            }
            [HarmonyPostfix]
            [HarmonyPatch("Activate")]
            public static void ActivatePostfix(uGUI_EncyclopediaTab __instance, CraftNode node)
            {
                lastEncNode = node;
                //if (strings.ContainsKey(node.id))
                //{
                //    AddDebug("Activate " + __instance.activeEntry.key);
                //}
            }
        }


    }

}