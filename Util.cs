using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static ErrorMessage;
using static VFXParticlesPool;

namespace Stats_Tracker
{
    internal static class Util
    {
        static readonly Dictionary<string, string> biomeNames = new Dictionary<string, string>()
        {
            { "arctickelp", "ST_kelp_forest"},
            { "arcticspires", "ST_arctic_spires"},
            { "crystalcave", "ST_crystal_caves"},
            { "crystalcave_inner", "ST_crystal_caves"},
            { "eastarctic", "ST_arctic"},
            { "fabricatorcaverns", "ST_fabricator_caverns"},
            { "glacialbay", "ST_glacial_bay"},
            { "glacialbasin", "ST_glacial_basin"},
            { "glacialbasin_underwater", "ST_glacial_basin"},
            { "icesheet", "ST_arctic"},
            { "lilypads", "ST_lilypads"},
            { "lilypads_deep", "ST_lilypads"},
            { "purplevents", "ST_purple_vents"},
            { "purplevents_deep", "ST_purple_vents"},
            { "rocketarea", "ST_delta_island"},
            { "sparsearctic", "ST_sparse_arctic"},
            { "treespires", "ST_tree_spires"},
            { "thermalspires", "ST_thermal_spires"},
            { "twistybridges", "ST_twisty_bridges"},
            { "twistybridges_deep", "ST_twisty_bridges"},
            { "twistybridges_shallow", "ST_twisty_bridges"},
            { "westarctic", "ST_arctic"},
            { "tundravoid", "ST_void"},
            { "worldedge", "ST_void"},
        };

        // arcticKelp
        // arcticSpires
        // CrystalCave
        // CrystalCave_Inner
        // eastarctic
        // fabricatorcaverns
        // glacialBay
        // glacialBasin
        // glacialBasin_Underwater
        // Glacier
        // IceSheet
        // lilyPads
        // lilyPads_Deep
        // purpleVents
        // PurpleVents_Deep
        // rocketArea
        // sparseArctic
        // treeSpires
        // thermalSpires
        // tundraVoid
        // TwistyBridges
        // twistyBridges_Deep
        // twistyBridges_Shallow
        // westArctic
        // worldEdge
        static BodyTemperature bodyTemperature;
        static int insideBaseTemp = 22;
        public static float GetPlayerTemperature()
        {
            //AddDebug("GetPlayerTemperature ");
            //IInteriorSpace currentInterior = Player.main.GetComponentInParent<IInteriorSpace>();
            //if (currentInterior != null)
            //    return currentInterior.GetInsideTemperature();
            bool bodyTemperatureDecreases = GameModeManager.GetOption<bool>(GameOption.BodyTemperatureDecreases);
            if (bodyTemperature == null && Player.main != null)
                bodyTemperature = Player.main.GetComponent<BodyTemperature>();

            if (bodyTemperature == null)
                return float.NaN;

            if (Player.main.currentMountedVehicle)
            {
                if (bodyTemperatureDecreases && Player.main.currentMountedVehicle.IsPowered())
                    return insideBaseTemp;
                //else if (!ConfigMenu.useRealTempForPlayerTemp.Value)
                //    return insideBaseTemp;
            }
            else if (Player.main.inHovercraft && !bodyTemperatureDecreases)
            {
                return insideBaseTemp;
            }
            else if (Player.main._currentInterior != null && Player.main._currentInterior is SeaTruckSegment)
            {
                SeaTruckSegment sts = Player.main._currentInterior as SeaTruckSegment;
                //AddDebug("SeaTruck IsPowered " + sts.relay.IsPowered());
                if (sts.relay.IsPowered())
                    return insideBaseTemp;
            }
            return insideBaseTemp;
        }

        public static bool IsPlayerInTruck()
        {
            return Player.main._currentInterior is SeaTruckSegment;
        }

        public static bool IsPlayerInVehicle()
        {
            Player player = Player.main;
            return player.inHovercraft || player.currentMountedVehicle || player._currentInterior is SeaTruckSegment;
        }

        public static string GetBiomeName()
        {
            string name = LargeWorld.main.GetBiome(Player.main.transform.position).ToLower();
            if (biomeNames.ContainsKey(name))
                return biomeNames[name];
            else
                return "ST_unknown_biome";
        }

        public static string GetFriendlyName(GameObject go)
        {
            TechType tt = CraftData.GetTechType(go);
            return Language.main.Get(tt.AsString(false));
        }

        public static string GetRawBiomeName()
        {
            AtmosphereDirector atmosphereDirector = AtmosphereDirector.main;
            if (atmosphereDirector)
            {
                string biomeOverride = atmosphereDirector.GetBiomeOverride();
                if (!string.IsNullOrEmpty(biomeOverride))
                    return biomeOverride;
            }
            LargeWorld largeWorld = LargeWorld.main;
            return largeWorld && Player.main ? largeWorld.GetBiome(Player.main.transform.position) : "<unknown>";
        }

        public static void AddValue(this Dictionary<string, TimeSpan> dic, string key, TimeSpan value)
        {
            dic[key] = dic.ContainsKey(key) ? dic[key] + value : value;
        }

        public static void AddValue(this Dictionary<string, int> dic, string key, int value)
        {
            dic[key] = dic.ContainsKey(key) ? dic[key] + value : value;
        }

        public static void AddValue(this Dictionary<string, float> dic, string key, float value)
        {
            dic[key] = dic.ContainsKey(key) ? dic[key] + value : value;
        }

        public static void AddValue(this Dictionary<TechType, int> dic, TechType key, int value)
        {
            dic[key] = dic.ContainsKey(key) ? dic[key] + value : value;
            //dic[tt] = dic.TryGetValue(tt, out var currentAmount) ? currentAmount + value : value;
        }

        public static void AddValue(this Dictionary<TechType, TimeSpan> dic, TechType key, TimeSpan value)
        {
            dic[key] = dic.ContainsKey(key) ? dic[key] + value : value;
        }

        public static void AddValue(this Dictionary<TechType, float> dic, TechType key, float value)
        {
            dic[key] = dic.ContainsKey(key) ? dic[key] + value : value;
        }
        public static float CelciusToFahrenhiet(float celcius)
        {
            return celcius * 1.8f + 32f;
        }

        public static float MeterToYard(float meter)
        {
            return meter * 1.0936f;
        }

        public static float KiloToPound(float kilos)
        {
            return kilos * 2.20462f;
        }

        public static float literToGallon(float liters)
        { // US gallon
            return liters / 3.785f;
        }




    }
}
