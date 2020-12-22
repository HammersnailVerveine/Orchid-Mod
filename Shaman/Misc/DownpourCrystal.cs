using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Misc
{
	public class DownpourCrystal : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 26;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 5;
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Downpour Crystal");
		}

	}
}
