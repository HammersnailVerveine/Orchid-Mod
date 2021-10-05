using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Accessories
{
	public class DemonicPocketMirror : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 26;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 1, 50);
			item.rare = 3;
			item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demonic Pocket Mirror");
			Tooltip.SetDefault("Allows you to see the next card you will draw");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerSeeCards ++;
		}
	}
}