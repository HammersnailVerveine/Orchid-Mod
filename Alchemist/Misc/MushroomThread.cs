using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Misc
{
	public class MushroomThread : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.rare = 0;
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phosphorescent Thread");
			Tooltip.SetDefault("Result of the hidden reaction between blinkroot and glowing mushroom extracts"
							+  "\n'quite an unexpected outcome'");
							//+  "\n[c/FF0000:PLEASE MAKE SURE THE HIDDEN REACTION KEYBIND IS SET]");
		}

	}
}
