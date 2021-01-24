using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Misc
{
	public class HarpyTalon : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 3, 50);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy Talon");
		}
	}
}
