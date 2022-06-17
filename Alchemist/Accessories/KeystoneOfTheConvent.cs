using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Accessories
{
	public class KeystoneOfTheConvent : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 2, 50, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.crit = 4;
			Item.damage = 37;
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