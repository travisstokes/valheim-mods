using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OVC.GameMods.Valheim.ShippableMetal
{
    public static class ShippableMetalBarsConfig
    {
        public static KeyCode PackageItemHotkey { get; set; }
        public static KeyCode PackageNextItemHotkey { get; set; }

        public static KeyCode UnpackageItemHotkey { get; set; }
        public static KeyCode UnpackageNextItemHotkey { get; set; }
        public static int MinimumStationLevel { get; set; }

        public static void Bind(ShippableMetalBars plugin)
        {
            PackageItemHotkey = plugin.Config.Bind("General", "PackageItemHotkey", KeyCode.H, "Hotkey for packaging an item").Value;
            PackageNextItemHotkey = plugin.Config.Bind("General", "PackageNextItemHotkey", KeyCode.J, "Hotkey for scrolling through packageable options").Value;
            UnpackageItemHotkey = plugin.Config.Bind("General", "UnpackageItemHotkey", KeyCode.K, "Hotkey for unpackaging an item").Value;
            UnpackageNextItemHotkey = plugin.Config.Bind("General", "UnpackageNextItemHotkey", KeyCode.L, "Hotkey for scrolling through unpackageable options").Value;
            MinimumStationLevel = plugin.Config.Bind("General", "MinimumStationLevel", 3, "Minimum forge level for packaging and unpackaging items").Value;
        }
    }
}
