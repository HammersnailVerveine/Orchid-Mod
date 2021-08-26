using Terraria;

namespace OrchidMod.Alchemist.Accessories
{
	public class WeightedBottles : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 26;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.rare = 1;
			item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Weighted Bottles");
			Tooltip.SetDefault("Increases alchemic main projectile velocity by 25%");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistVelocity += 0.25f;
		}
	}
}