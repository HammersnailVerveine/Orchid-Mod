using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Misc
{
	public class CopperKey : OrchidModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Opens the Mineshaft Box");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 22;
			item.maxStack = 1;
			item.useStyle = 0;
			item.value = Item.sellPrice(0, 0, 5, 0);
		}
	}
}