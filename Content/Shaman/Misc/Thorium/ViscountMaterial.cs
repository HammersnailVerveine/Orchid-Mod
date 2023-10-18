using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Misc.Thorium
{
	public class ViscountMaterial : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 24;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 3, 50);
			Item.rare = ItemRarityID.Blue;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vampiric Membrane");
		}
	}
}