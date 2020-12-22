using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Dusts.Thorium;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.NPCs.Hostile.DownpourElemental.Projectiles
{
    public class DownpourElementalCloudProj : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 14;
            projectile.hostile = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 350;
			projectile.scale = 1f;
			aiType = ProjectileID.Bullet; 	
			projectile.alpha = 255;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Bolt");
        } 
		
        public override void AI()
        {
			int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 125, default(Color), 1f);
			Main.dust[DustID2].noGravity = true;
			Main.dust[DustID2].velocity = projectile.velocity / 2;
			
			projectile.tileCollide = projectile.timeLeft < 320;
			
            if (projectile.timeLeft % 15 == 0)
            {
				projectile.ai[0] = (Main.rand.Next(40) - 20);
				Vector2 projectileVelocity = ( new Vector2(projectile.velocity.X, projectile.velocity.Y ).RotatedBy(MathHelper.ToRadians(projectile.ai[0] / 2)));
				projectile.velocity = projectileVelocity;
			}
		}
    }
}