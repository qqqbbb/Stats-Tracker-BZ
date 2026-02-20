using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stats_Tracker
{
    public class ConfigMenu
    {

        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> fahrenhiet;
        public static ConfigEntry<bool> miles;
        public static ConfigEntry<bool> pounds;
        public static ConfigEntry<bool> biomeName;

        public static void Bind()
        {
            modEnabled = Main.configMenu.Bind("", Language.main.Get("ST_mod_enabled"), true);
            biomeName = Main.configMenu.Bind("", Language.main.Get("ST_biome_name"), false);

            fahrenhiet = Main.configMenu.Bind("", "Show temperature in Fahrenhiet", false);
            miles = Main.configMenu.Bind("", "Show distance in miles and yards", false);
            pounds = Main.configMenu.Bind("", "Show weight in pounds", false);

        }
    }
}
