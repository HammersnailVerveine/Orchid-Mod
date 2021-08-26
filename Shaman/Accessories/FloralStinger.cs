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
			Tooltip.SetDefault("Under the effect of a shamanic earth bond, falling under 50% life will make you enrage"
							+ "\nWhile enraged, the effectiveness of your shamanic fire and earth bonds is increased a lot");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanRage = true;

			if (player.statLife < (int)(player.statLifeMax2 / 2) && modPlayer.shamanEarthTimer > 0)
			{
				//modPlayer.shamanFireBonus += 5;
				//modPlayer.shamanEarthBonus += 3;
				player.AddBuff((mod.BuffType("JungleRage")), 1);
			}
		}
	}
}
