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
		public static bool IsItemTypeEquals(this Mod mod, string itemName, int type)
			=> mod.TryFind(itemName, out ModItem modItem) && modItem.Type.Equals(type);

		public static bool IsNPCTypeEquals(this Mod mod, string npcName, int type)
			=> mod.TryFind(npcName, out ModNPC modNPC) && modNPC.Type.Equals(type);
	}
}