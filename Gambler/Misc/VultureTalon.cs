using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Misc
{
	public class VultureTalon : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 24;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 0, 50);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulture Talon");
		}
	}
}
