using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Misc
{
	public class RitualScepter : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 1;
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ritual Scepter");
			Tooltip.SetDefault("Can be upgraded into various shamanic weapons");
		}

	}
}
