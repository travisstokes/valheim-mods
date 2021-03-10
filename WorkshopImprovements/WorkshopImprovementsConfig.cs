using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OVC.GameMods.Valheim.WorkshopImprovements
{
    public static class WorkshopImprovementsConfig
    {
        public static KeyCode LoadMaxHotkey { get; set; }

        public static void Bind(WorkshopImprovements plugin)
        {
            LoadMaxHotkey = plugin.Config.Bind("General", "LoadMaxHotkey", KeyCode.H, "Hotkey for loading the maximum number of items into an input or output slot.").Value;
        }
    }
}
