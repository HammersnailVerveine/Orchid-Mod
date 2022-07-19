using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Accessories
{
	public class DemonicPocketMirror : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 1, 50);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demonic Pocket Mirror");
			Tooltip.SetDefault("Allows you to see the next card you will draw");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			modPlayer.gamblerSeeCards ++;
		}
	}
}