using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{

	public class HoneyOrb2 : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		int orbsNumber = 0;
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Honey Orb");
        } 
		public override void SafeSetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.timeLeft = 12960000;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			Main.projFrames[projectile.type] = 8;
		}
		
		// public override Color? GetAlpha(Color lightColor)
        // {
            // return Color.White;
        // }
		
		public override bool? CanCutTiles() {
			return false;
		}
		
        public override void AI()
        {
			Player player = Main.player[projectile.owner];
			
			if (player != Main.player[Main.myPlayer]) {
				projectile.active = false;
			}
			
			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 10 == 0)
				projectile.frame ++;
			if (projectile.frame == 8)
				projectile.frame = 0;
			
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique == 0 || player.GetModPlayer<OrchidModPlayer>().orbCountUnique > 14 || player.GetModPlayer<OrchidModPlayer>().shamanOrbUnique != ShamanOrbUnique.HONEY)
				projectile.Kill();
			else orbsNumber = player.GetModPlayer<OrchidModPlayer>().orbCountUnique;

			if (projectile.timeLeft == 12960000) {
				startX = projectile.position.X - player.position.X;
				startY = projectile.position.Y - player.position.Y;
			}
			projectile.velocity.X = player.velocity.X;
			projectile.position.X = player.position.X + startX;
			projectile.position.Y = player.position.Y + startY;
			
			if (Main.rand.Next(16) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 153);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.2f;
				
			}
			
        }
		
		public override void Kill(int timeLeft)
        {
			for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 153);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 5f;
            }
        }
    }
}
 