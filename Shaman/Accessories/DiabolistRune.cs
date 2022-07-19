using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
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
			DisplayName.SetDefault("Diabolist Rune");
			Tooltip.SetDefault("Allows you to cauterize your injuries if you take heavy damage while a shamanic earth bond is active"
							+ "\nThe cauterization will occur if you lose over 50% of your total health without interruption, and has a 1 minute cooldown");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanDiabolist = true;

			if (modPlayer.shamanTimerDiabolist > 0)
			{
				modPlayer.shamanTimerDiabolist--;
				if (modPlayer.shamanTimerDiabolist == 0)
				{
					modPlayer.shamanDiabolistCount = 0;
				}

				if (modPlayer.shamanDiabolistCount >= player.statLifeMax2 / 2 && !player.HasBuff(Mod.Find<ModBuff>("DiabolistCauterizationCooldown").Type))
				{
					modPlayer.shamanDiabolistCount = 0;
					SoundEngine.PlaySound(SoundID.Item74);
					player.AddBuff((Mod.Find<ModBuff>("DiabolistCauterize").Type), 60 * 10);
					player.AddBuff((Mod.Find<ModBuff>("DiabolistCauterizeCooldown").Type), 60 * 60);

					for (int i = 0; i < 15; i++)
					{
						int dust = Dust.NewDust(player.position, player.width, player.height, 6);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
				}
			}
		}
	}
}
