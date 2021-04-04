using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Misc
{
	public class AlchemicStabilizer : OrchidModItem
	{
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 26;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemical Stabilizer");
			Tooltip.SetDefault("Used to make various alchemist weapons"
							+  "\n'Smells like bee wax'");
		}

	}
}
