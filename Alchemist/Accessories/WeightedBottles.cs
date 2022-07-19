using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Accessories
{
	public class WeightedBottles : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Weighted Bottles");
			Tooltip.SetDefault("Increases alchemic main projectile velocity by 25%");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			modPlayer.alchemistVelocity += 0.25f;
		}
	}
}