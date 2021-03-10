using HarmonyLib;

namespace OVC.GameMods.Valheim.WorkshopImprovements
{
    [HarmonyPatch(typeof(Smelter), "Awake")]
    public static class Smelter_Awake_Patch
    {
        public static void Postfix(Smelter __instance)
        {
            //__instance. += $"\n[<color=yellow><b>{WorkshopImprovementsConfig.LoadMaxHotkey}</b></color>] Load maximum " + __instance.m_fuelItem
        }
    }
}
