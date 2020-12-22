using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Misc.Thorium
{
	public class ViscountMaterial : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 24;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 3, 50);
			item.rare = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vampiric Membrane");
		}
	}
}