using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Accessories
{
	public class KeystoneOfTheConvent : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 26;
			item.height = 26;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = 3;
			item.accessory = true;
			item.crit = 4;
			item.damage = 37;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Keystone of the Convent");
			Tooltip.SetDefault("Attacks with at least 3 non-air ingredients will be empowered"
							+ "\nHit target will be dealt damage for all nearby air coatings, consuming them");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistCovent = true;
		}
	}
}