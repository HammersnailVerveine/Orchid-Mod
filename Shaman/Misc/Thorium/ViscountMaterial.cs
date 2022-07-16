using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Misc.Thorium
{
	public class ViscountMaterial : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 24;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 3, 50);
			Item.rare = ItemRarityID.Blue;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Vampiric Membrane");
		}
	}
}