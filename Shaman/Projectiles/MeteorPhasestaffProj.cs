using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class MeteorPhasestaffProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.aiStyle = 29;
			projectile.penetrate = 100;
			projectile.timeLeft = 60;
            projectile.extraUpdates = 5;
			projectile.ignoreWater = true;   
			projectile.alpha = 255;
            this.empowermentType = 1;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 0;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phasebeam");
        } 
		
        public override void AI()
		{
			if (projectile.timeLeft % 5 == 0)
				projectile.damage -= 1;
			
            int index1 = Dust.NewDust(projectile.position, 1, 1, 270, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
            Main.dust[index1].velocity *= 0.2f;
			Main.dust[index1].fadeIn = 1f;
			Main.dust[index1].scale = 0.8f + ((projectile.timeLeft)/90f) * 1.8f;
            Main.dust[index1].noGravity = true;
			
			if (projectile.timeLeft == 60 || projectile.timeLeft == 55 || projectile.timeLeft == 1) {
				 
				for (int i = 0 ; i < 20 ; i ++ ) {
					
					double dist = 20;
					if (projectile.timeLeft == 55)
						dist = 15;
					
					double deg = (double) projectile.ai[1] * (i * (36 + 5 - Main.rand.Next(10)));
					double rad = deg * (Math.PI / 180);
					 
					float posX = projectile.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2 + projectile.velocity.X;
					float posY = projectile.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2 + projectile.velocity.Y;
					
					Vector2 dustPosition = new Vector2(posX, posY);
					
					int index2 = Dust.NewDust(dustPosition, 1, 1, 170, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
					Main.dust[index2].velocity *= 0.2f;
					Main.dust[index2].fadeIn = 1f;
					Main.dust[index2].scale = 1f;
					Main.dust[index2].noGravity = true;
				}
			}
        }
		
		public override void Kill(int timeLeft)
        {
            int dust = Dust.NewDust(projectile.position, 1, 1, 270, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0.2f;
			Main.dust[dust].scale = 2.5f;
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
        {
			target.AddBuff((24), 1 * 60);
		}
    }
}