using BepInEx;
using ExtendedItemDataFramework;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ItemDrop;

namespace OVC.GameMods.Valheim.ShippableMetal
{
    [BepInPlugin("OVC.GameMods.Valheim.shippablemetalbars", "Shippable Metal Bars", "1.0.0")]
    [BepInDependency("randyknapp.mods.extendeditemdataframework")]
    public class ShippableMetalBars : BaseUnityPlugin
    {
        private static readonly Harmony _harmony = new Harmony(typeof(ShippableMetalBars).GetCustomAttributes(typeof(BepInPlugin), false)
            .Cast<BepInPlugin>()
            .First()
            .GUID);

        public static RotatingMenuManager<ItemData> PackageableItemsMenuManager;
        public static RotatingMenuManager<ItemData> UnPackageableItemsMenuManager;

        private void Awake()
        {
            Func<List<ItemData>> playerItemsLookup = () => Player.m_localPlayer?.GetInventory()?.GetAllItems() ?? new List<ItemData>();
            PackageableItemsMenuManager = new RotatingMenuManager<ItemData>(playerItemsLookup, i => i.IsPackageable(), i => Localization.instance.Localize(i.m_shared.m_name));
            UnPackageableItemsMenuManager = new RotatingMenuManager<ItemData>(playerItemsLookup, i => i.IsUnpackageable(), i => i.m_shared.m_name);

            ShippableMetalBarsConfig.Bind(this);

            ExtendedItemData.NewExtendedItemData += ShippableItemData.OnNewExtendedItemData;
            ExtendedItemData.LoadExtendedItemData += ShippableItemData.OnLoadExtendedItemData;

            _harmony.PatchAll();
            var patchedMethods = _harmony.GetPatchedMethods();
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchAll();
        }
    }
}
