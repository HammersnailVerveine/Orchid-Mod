using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Misc
{
	public class AlchemicStabilizer : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 26;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.White;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemical Stabilizer");
			Tooltip.SetDefault("Used to make various alchemist weapons"
							+ "\n'Smells like bee wax'");
		}

	}
}
