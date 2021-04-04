using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Misc
{
	public class BrokenHeroScepter : OrchidModItem
	{
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 8;
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Hero Scepter");
		}

	}
}
