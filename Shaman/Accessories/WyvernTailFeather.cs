using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Accessories
{
	public class WyvernTailFeather : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern Tail Feather");
			Tooltip.SetDefault("Increases flight time as long as you have an active shamanic air bond"
							+ "\nShamanic damage is increased by 10% during flight, but reduced by 5% on land");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();
			modPlayer.shamanWyvern = true;


			if (player.wingTimeMax > 0 && modPlayer.shamanAirTimer > 0)
			{
				player.wingTimeMax += 60;
			}

			modPlayer.shamanDamage += player.wingTime < player.wingTimeMax || player.velocity.Y != 0 ? 0.1f : -0.05f;
		}
	}
}
