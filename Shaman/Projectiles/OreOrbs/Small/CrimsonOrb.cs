using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Small
{

	public class CrimsonOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float	 startY = 0;
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Orb");
        } 
		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.timeLeft = 12960000;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			Main.projFrames[projectile.type] = 8;
		}
		
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override bool? CanCutTiles() {
			return false;
		}
		
        public override void AI()
        {         					
			Player player = Main.player[projectile.owner];
			
			if (player != Main.player[Main.myPlayer]) {
				projectile.active = false;
			}

			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 65 || player.GetModPlayer<OrchidModPlayer>().timer120 == 0)
				projectile.frame = 1;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 70 || player.GetModPlayer<OrchidModPlayer>().timer120 == 5)
				projectile.frame = 2;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 75 || player.GetModPlayer<OrchidModPlayer>().timer120 == 10)
				projectile.frame = 3;
		   	if (player.GetModPlayer<OrchidModPlayer>().timer120 == 80 || player.GetModPlayer<OrchidModPlayer>().timer120 == 15)
				projectile.frame = 4;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 85 || player.GetModPlayer<OrchidModPlayer>().timer120 == 20)
				projectile.frame = 5;
		    if (player.GetModPlayer<OrchidModPlayer>().timer120 == 90 || player.GetModPlayer<OrchidModPlayer>().timer120 == 25)
				projectile.frame = 6;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 95 || player.GetModPlayer<OrchidModPlayer>().timer120 == 30)
				projectile.frame = 7;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 100 || player.GetModPlayer<OrchidModPlayer>().timer120 == 35)
				projectile.frame = 0;
			
			
			if (player.GetModPlayer<OrchidModPlayer>().shamanOrbSmall != ShamanOrbSmall.CRIMSON || player.GetModPlayer<OrchidModPlayer>().orbCountSmall == 0)
				projectile.Kill();

			if (projectile.timeLeft == 12960000) {
				int nbOrb = player.GetModPlayer<OrchidModPlayer>().orbCountSmall;
				int offsetX = 7;
				
				if (nbOrb == 1) {
					startX = - 15 - offsetX;
					startY = - 20 - offsetX;
				}
				
				if (nbOrb == 2) {
					startX = + 0  - offsetX;
					startY = - 25 - offsetX;
				}
				
				if (nbOrb == 3) {
					startX = + 15 - offsetX;
					startY = - 20 - offsetX;
				}
				
				if (projectile.damage != 0) {
					projectile.damage = 0;
					startX = - 15 - offsetX;
					startY = - 20 - offsetX;
				}
			}
			
			projectile.velocity.X = player.velocity.X;
			projectile.position.X = player.position.X + player.width / 2 + startX;
			projectile.position.Y = player.position.Y + startY;
			
			if (Main.rand.Next(20) == 0) {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 90);
				Main.dust[dust].velocity /= 3f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
			}
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 90);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
            }
        }
    }
}
 