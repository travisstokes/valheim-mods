using ExtendedItemDataFramework;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace OVC.GameMods.Valheim.ShippableMetal
{
    public class ShippableItemData : BaseExtendedItemComponent
    {
        public bool IsPackaged { get; private set; }
        public string OriginalPrefabName { get; set; }
        public string OriginalLocalizedName { get; private set; }
        public string OriginalDescription { get; set; }
        public bool OriginalQuestItemValue { get; set; }


        public ShippableItemData(ExtendedItemData parent) : base(typeof(ShippableItemData).AssemblyQualifiedName, parent)
        {
        }

        public bool CheckIsPackagable()
        {
            if(IsPackaged)
            {
                return false;
            }

            return ItemData.m_shared.m_teleportable == false;
        }

        public void Package(Player player, Inventory inventory)
        {
            if(!CheckIsPackagable())
            {
                return;
            }

            var items = inventory.GetAllItems();

            RemoveCurrent(inventory);

            var itemPrefab = ObjectDB.instance.GetItemPrefab(ItemData.m_dropPrefab.name);
            ZNetView.m_forceDisableInit = true;
            GameObject gameObject = Object.Instantiate(itemPrefab);
            ZNetView.m_forceDisableInit = false;
            var itemDrop = gameObject.GetComponent<ItemDrop>();
            if (itemDrop != null)
            {
                itemDrop.m_itemData = new ExtendedItemData(itemDrop.m_itemData);
                if (itemDrop.m_itemData.m_dropPrefab == null)
                {
                    itemDrop.m_itemData.m_dropPrefab = itemPrefab;
                }

                itemDrop.m_itemData.Extended()?.GetComponent<ShippableItemData>()?.ApplyPackagedAttributes();
            }
            player.Message(MessageHud.MessageType.TopLeft, $"Packaged {Localization.instance.Localize(ItemData.m_shared.m_name)}", 1, ItemData.GetIcon());
            inventory.AddItem(itemDrop.m_itemData);
            Object.DestroyImmediate(gameObject);
        }

        private void RemoveCurrent(Inventory inventory, int amount = 1)
        {
            if (inventory.RemoveItem(ItemData, amount))
            {
                return;
            }
            else
            {
                var itemToRemove = inventory.GetAllItems().Where(i => i.m_shared.m_name == ItemData.m_shared.m_name).FirstOrDefault();

                if (itemToRemove != null)
                {
                    inventory.RemoveItem(itemToRemove, amount);
                }
            }
        }

        public void Unpackage(Player player, Inventory inventory)
        {
            RemoveCurrent(inventory);
            inventory.AddItem(OriginalPrefabName, 1, 1, 0, 0, "");
            player.Message(MessageHud.MessageType.TopLeft, $"Unpackaged {OriginalLocalizedName}", 1, ItemData.GetIcon());
        }

        public static void OnNewExtendedItemData(ExtendedItemData itemdata)
        {
            if (itemdata.GetComponent<ShippableItemData>() == null)
            {
                itemdata.AddComponent<ShippableItemData>();
            }
        }

        public static void OnLoadExtendedItemData(ExtendedItemData itemdata)
        {
            if(itemdata.GetComponent<ShippableItemData>() == null)
            {
                itemdata.AddComponent<ShippableItemData>();
            }
        }

        public override BaseExtendedItemComponent Clone()
        {
            return MemberwiseClone() as ShippableItemData;
        }

        public override void Deserialize(string data)
        {
            if(bool.TryParse(data, out bool isPackaged) && isPackaged && !IsPackaged)
            {
                ApplyPackagedAttributes();
            }
        }

        public override string Serialize()
        {
            return IsPackaged.ToString();
        }

        private void ApplyPackagedAttributes()
        {
            CaptureRawData();

            var localizedDescription = Localization.instance.Localize(ItemData.m_shared.m_description) + " (Packaged)";
            ItemData.m_shared.m_name = GetPackagedName();
            ItemData.m_shared.m_description = localizedDescription;
            ItemData.m_shared.m_teleportable = true;
            ItemData.m_shared.m_questItem = false;

            IsPackaged = true;

            Save();
        }

        private string GetPackagedName()
        {
            return Localization.instance.Localize(ItemData.m_shared.m_name) + " (Packaged)";
        }

        private void CaptureRawData()
        {
            OriginalPrefabName = ItemData.m_dropPrefab.name;
            OriginalLocalizedName = Localization.instance.Localize(ItemData.m_shared.m_name);
            OriginalDescription = ItemData.m_shared.m_description;
            OriginalQuestItemValue = ItemData.m_shared.m_questItem;
        }
    }
}
