using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OVC.GameMods.Valheim.ShippableMetal
{
    [HarmonyPatch(typeof(Player), "Update")]
    public static class Player_Update_Patch
    {
        public static void Postfix(Player __instance)
        {
            bool keyPackageItemDown = Input.GetKeyDown(ShippableMetalBarsConfig.PackageItemHotkey);
            bool keyPackageNextItemDown = Input.GetKeyDown(ShippableMetalBarsConfig.PackageNextItemHotkey);
            bool keyUnpackageItemDown = Input.GetKeyDown(ShippableMetalBarsConfig.UnpackageItemHotkey);
            bool keyUnpackageNextItemDown = Input.GetKeyDown(ShippableMetalBarsConfig.UnpackageNextItemHotkey);

            // If none of the hotkeys we care about are down, don't bother processing at all.
            if(!keyPackageItemDown
                && !keyPackageNextItemDown
                && !keyUnpackageItemDown
                && !keyUnpackageNextItemDown)
            {
                return;
            }

            // If they aren't near a required station, bail.
            // Don't display a message since we can't assume our hotkeys aren't used for something else that is currently valid
            var craftingStation = __instance.GetHoverObject()?.GetComponent<CraftingStation>();
            if (ShippableMetalBarsConfig.MinimumStationLevel > 0)
            {
                List<CraftingStation> stations = new List<CraftingStation>();
                CraftingStation.FindStationsInRange(ShippableMetalBars.CRAFTING_STATION_NAME_FORGE, (Vector3)__instance.transform.position, 1.7f, stations);
                if (!stations.Exists(s => s.GetLevel() >= ShippableMetalBarsConfig.MinimumStationLevel))
                {
                    return;
                }
            }

            if(keyPackageItemDown)
            {
                var currentPackMenuState = ShippableMetalBars.PackageableItemsMenuManager.GetCurrentOptionState();
                if(currentPackMenuState.TotalOptionsAvailable > 0)
                {
                    // TODO: We need a better way of checking for a match here so we don't have to remember the logic everywhere.
                    var item = __instance.GetInventory().GetAllItems().FirstOrDefault(i => Localization.instance.Localize(i.m_shared.m_name) == currentPackMenuState.Option);
                    if(item != null)
                    {
                        item.Package(__instance, __instance.GetInventory());
                    }
                }
            }

            if(keyPackageNextItemDown)
            {
                ShippableMetalBars.PackageableItemsMenuManager.GotoNext();
            }

            if(keyUnpackageItemDown)
            {
                var currentUnpackMenuState = ShippableMetalBars.UnPackageableItemsMenuManager.GetCurrentOptionState();
                if (currentUnpackMenuState.TotalOptionsAvailable > 0)
                {
                    // TODO: We need a better way of checking for a match here so we don't have to remember the logic everywhere.
                    var item = __instance.GetInventory().GetAllItems().FirstOrDefault(i => i.m_shared.m_name == currentUnpackMenuState.Option);
                    if (item != null)
                    {
                        item.Unpackage(__instance, __instance.GetInventory());
                    }
                }
            }

            if(keyUnpackageNextItemDown)
            {
                ShippableMetalBars.UnPackageableItemsMenuManager.GotoNext();
            }
        }
    }
}
