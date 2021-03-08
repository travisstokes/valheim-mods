using ExtendedItemDataFramework;

namespace OVC.GameMods.Valheim.ShippableMetal
{
    public static class ItemDataExtensions
    {
        public static void Package(this ItemDrop.ItemData item, Player player, Inventory inventory)
        {
            item.Extended()?.GetComponent<ShippableItemData>()?.Package(player, inventory);
        }

        public static bool IsPackageable(this ItemDrop.ItemData item)
        {
            return item.Extended()?.GetComponent<ShippableItemData>()?.CheckIsPackagable() ?? false;
        }

        public static void Unpackage(this ItemDrop.ItemData item, Player player, Inventory inventory)
        {
            item.Extended()?.GetComponent<ShippableItemData>()?.Unpackage(player, inventory);
        }

        public static bool IsUnpackageable(this ItemDrop.ItemData item)
        {
            return item.Extended()?.GetComponent<ShippableItemData>()?.IsPackaged ?? false;
        }
    }
}
