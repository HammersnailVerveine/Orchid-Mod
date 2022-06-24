using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	public class TreasuredBaubles : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 35, 20);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasured Baubles");
			Tooltip.SetDefault("After completing an orb weapon cycle, you will be given a bonus orb on your next hit");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();

			if (modPlayer.GetNbShamanicBonds() > 0)
			{
				if (modPlayer.orbCountSmall == 0 && modPlayer.shamanOrbSmall != ShamanOrbSmall.NULL)
				{
					player.AddBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type, 10 * 60);
					modPlayer.shamanOrbSmall = ShamanOrbSmall.NULL;
				}

				if (modPlayer.orbCountBig == 0 && modPlayer.shamanOrbBig != ShamanOrbBig.NULL)
				{
					player.AddBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type, 10 * 60);
					modPlayer.shamanOrbBig = ShamanOrbBig.NULL;
				}

				if (modPlayer.orbCountLarge == 0 && modPlayer.shamanOrbLarge != ShamanOrbLarge.NULL)
				{
					player.AddBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type, 10 * 60);
					modPlayer.shamanOrbLarge = ShamanOrbLarge.NULL;
				}

				if (modPlayer.orbCountUnique == 0 && modPlayer.shamanOrbUnique != ShamanOrbUnique.NULL)
				{
					player.AddBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type, 10 * 60);
					modPlayer.shamanOrbUnique = ShamanOrbUnique.NULL;
				}
			}
		}
	}
}
