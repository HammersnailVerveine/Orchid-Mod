using Terraria;

namespace OrchidMod.Gambler.Accessories
{
	public class LuckySprout : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = 1;
			item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lucky Sprout");
			Tooltip.SetDefault("Increases the frequency at which 'biome' cards spawns seeds");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerLuckySprout = true;
		}
	}
}