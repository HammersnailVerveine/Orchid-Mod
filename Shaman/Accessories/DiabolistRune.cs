using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
    public class DiabolistRune : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 30;
            item.height = 36;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 8;
            item.accessory = true;
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diabolist Rune");
			Tooltip.SetDefault("Allows you to cauterize your injuries if you take heavy damage while a shamanic earth bond is active"
							+  "\nThe cauterization will occur if you lose over 50% of your total health without interruption, and has a 1 minute cooldown");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDiabolist = true;
			
			if (modPlayer.shamanTimerDiabolist > 0) {
				modPlayer.shamanTimerDiabolist --;
				if (modPlayer.shamanTimerDiabolist == 0) {
					modPlayer.shamanDiabolistCount = 0;
				}
				
				if (modPlayer.shamanDiabolistCount >= player.statLifeMax2 / 2 && !player.HasBuff(mod.BuffType("DiabolistCauterizationCooldown"))) {
					modPlayer.shamanDiabolistCount = 0;
					Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y, 74);
					player.AddBuff((mod.BuffType("DiabolistCauterize")), 60 * 10);
					player.AddBuff((mod.BuffType("DiabolistCauterizeCooldown")), 60 * 60);
					
					for(int i=0; i < 15; i++)
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
