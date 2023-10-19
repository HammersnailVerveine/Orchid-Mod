using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Accessories
{
	public class DiabolistRune : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Diabolist Rune");
			/* Tooltip.SetDefault("Allows you to cauterize your injuries if you take heavy damage while a shamanic earth bond is active"
							+ "\nThe cauterization will occur if you lose over 50% of your total health without interruption, and has a 1 minute cooldown"); */
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		}
	}
}
