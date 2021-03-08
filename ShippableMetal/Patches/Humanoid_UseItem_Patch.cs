using ExtendedItemDataFramework;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OVC.GameMods.Valheim.ShippableMetal
{ 
    // Disabled in favor of the hotkey approach
    //[HarmonyPatch(typeof(Humanoid), "UseItem")]
    public static class Humanoid_UseItem_Patch
    {
        public static void Prefix(Humanoid __instance, Inventory inventory, ItemDrop.ItemData item, bool fromInventoryGui)
        {
            var shippableItem = item.Extended()?.GetComponent<ShippableItemData>();

            var player = __instance as Player;

            if (player == null || (!shippableItem.IsPackaged && !shippableItem.CheckIsPackagable()))
            {
                return;
            }

            UnityEngine.Debug.Log("Station check...");
            if (ShippableMetalBarsConfig.MinimumStationLevel > 0)
            {
                List<CraftingStation> stations = new List<CraftingStation>();
                CraftingStation.FindStationsInRange("forge", (Vector3)player.transform.position, 5f, stations);
                if (!stations.Exists(s => s.GetLevel() >= ShippableMetalBarsConfig.MinimumStationLevel))
                {
                    return;
                }
            }

            if (shippableItem.IsPackaged)
            {
                shippableItem.Unpackage(player, inventory);
            } 
            else if (shippableItem.CheckIsPackagable())
            {
                shippableItem.Package(player, inventory);
            }

            return;
        }
    }
}
