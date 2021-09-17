using Terraria;

namespace OrchidMod.Shaman.Accessories
{
	public class FloralStinger : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 34;
			item.height = 40;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = 7;
			item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floral Stinger");
			Tooltip.SetDefault("Exhausting your Earth Bond weapons will make you enrage"
							+ "\nWhile enraged, your shamanic damage is increased by 20%");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanRage = true;

			if (modPlayer.shamanPollEarthMax) {
				player.AddBuff((mod.BuffType("JungleRage")), 1);
				modPlayer.shamanDamage += 0.2f;
			}
		}
	}
}
