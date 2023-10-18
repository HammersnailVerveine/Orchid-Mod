using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Accessories
{
	public class FloralStinger : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Floral Stinger");
			/* Tooltip.SetDefault("Exhausting your Earth Bond weapons will make you enrage"
							+ "\nWhile enraged, your shamanic damage is increased by 20%"); */
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanRage = true;
			
			/*
			if (modPlayer.shamanPollEarthMax) {
				player.AddBuff((Mod.Find<ModBuff>("JungleRage").Type), 1);
				player.GetDamage<ShamanDamageClass>() += 0.2f;
			}
			*/
		}
	}
}
