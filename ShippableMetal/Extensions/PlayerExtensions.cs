using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVC.GameMods.Valheim.ShippableMetal.Extensions
{
    public static class PlayerExtensions
    {
        public static IOrderedEnumerable<string> GetHeldPackageableNames(this Player player) 
        {
            return player
                .GetInventory()
                .GetAllItems()
                .Where(i => i.IsPackageable())
                .Select(i => Localization.instance.Localize(i.m_shared.m_name))
                .Distinct()
                .OrderBy(i => i);
        }

        public static IOrderedEnumerable<string> GetHeldUnpackageableNames(this Player player)
        {
            return player
                .GetInventory()
                .GetAllItems()
                .Where(i => i.IsPackageable())
                .Select(i => i.m_shared.m_name)
                .Distinct()
                .OrderBy(i => i);
        }
    }
}
