using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Misc
{
	public class GuardianEmpressMaterial : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 38;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(0, 0, 12, 50);
			Item.rare = ItemRarityID.Yellow;
		}
	}
}
