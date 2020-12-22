using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Misc
{
	public class OrchidEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orchid Emblem");
			Tooltip.SetDefault("While the Shamans can continue their playthrough, the journey for both Alchemists and Gamblers stops here"
							+  "\n... For now !"
							+  "\nContent up to Moon Lord for these classes will be added with the upcoming updates"
							+  "\nPlease, come and say hi on Discord if you want to support me ;)");
		}

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 32;
			item.maxStack = 1;
			item.useStyle = 0;
			item.rare = -11;
			item.value = Item.sellPrice(0, 1, 0, 0);
		}
	}
}