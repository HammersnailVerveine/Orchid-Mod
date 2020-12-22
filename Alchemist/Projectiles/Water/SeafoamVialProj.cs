using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles.Water
{
    public class SeafoamVialProj : OrchidModAlchemistProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
			projectile.alpha = 128;
			projectile.penetrate = 5;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seafoam Bubble");
        } 
		
		public override void AI()
        {
			projectile.velocity *= 0.9f;
			projectile.rotation += 0.02f;
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 217);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
            }
        }
    }
}