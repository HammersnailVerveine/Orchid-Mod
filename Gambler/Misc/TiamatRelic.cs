using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Misc
{
	public class TiamatRelic : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 12;
			item.maxStack = 99;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 15, 0);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiamat Relic");
		}
	}
}
