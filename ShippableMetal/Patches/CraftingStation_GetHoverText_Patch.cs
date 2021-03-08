using HarmonyLib;

namespace OVC.GameMods.Valheim.ShippableMetal
{
    [HarmonyPatch(typeof(CraftingStation), "GetHoverText")]
    public static class CraftingStation_GetHoverText_Patch
    {
        public static string Postfix(string __result, CraftingStation __instance)
        {
            if(__instance.GetLevel() < ShippableMetalBarsConfig.MinimumStationLevel)
            {
                return __result;
            }

            var currentPackageableState = ShippableMetalBars.PackageableItemsMenuManager.GetCurrentOptionState();

            if (currentPackageableState.TotalOptionsAvailable > 0)
            {
                __result +=
                        $"\n[<color=yellow><b>{ShippableMetalBarsConfig.PackageItemHotkey}</b></color>] Package " + currentPackageableState.Option;

                if(currentPackageableState.TotalOptionsAvailable > 1)
                {
                    __result +=
                        $"\n[<color=yellow><b>{ShippableMetalBarsConfig.PackageNextItemHotkey}</b></color>] Select different item to pack";
                }       
            }

            var currentUnpackageableState = ShippableMetalBars.UnPackageableItemsMenuManager.GetCurrentOptionState();
            if (currentUnpackageableState.TotalOptionsAvailable > 0)
            {
                __result +=
                        $"\n[<color=yellow><b>{ShippableMetalBarsConfig.UnpackageItemHotkey}</b></color>] Unpack " + currentUnpackageableState.Option;

                if (currentUnpackageableState.TotalOptionsAvailable > 1)
                {
                    __result +=
                        $"\n[<color=yellow><b>{ShippableMetalBarsConfig.UnpackageNextItemHotkey}</b></color>] Select different item to unpack";
                }
            }

            return __result;
        }
    }
}
