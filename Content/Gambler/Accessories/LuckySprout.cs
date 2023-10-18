using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Gambler.Accessories
{
	public class LuckySprout : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Lucky Sprout");
			// Tooltip.SetDefault("Bushes from the 'biome' set will linger intead of disappearing");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			modPlayer.gamblerLuckySprout = true;
		}
	}
}