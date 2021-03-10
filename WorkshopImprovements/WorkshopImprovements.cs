using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVC.GameMods.Valheim.WorkshopImprovements
{
    [BepInPlugin("OVC.GameMods.Valheim.ProcessorLoadingToilReducer", "Processor Loading Toil Reducer", "1.0.0")]
    public class WorkshopImprovements : BaseUnityPlugin
    {
        private static readonly Harmony _harmony = new Harmony(typeof(WorkshopImprovements).GetCustomAttributes(typeof(BepInPlugin), false)
            .Cast<BepInPlugin>()
            .First()
            .GUID);

        private void Awake()
        {
            WorkshopImprovementsConfig.Bind(this);

            _harmony.PatchAll();
            var patchedMethods = _harmony.GetPatchedMethods();
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchAll();
        }
    }
}
