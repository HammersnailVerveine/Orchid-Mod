using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OrchidMod.Utilities
{
	public static class ModUtils
	{
		public static bool IsItemTypeEquivalentToNumber(this Mod mod, string itemName, int number)
			=> mod.TryFind(itemName, out ModItem modItem) && modItem.Type == number;
	}
}