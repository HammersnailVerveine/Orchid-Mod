using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class SpiritDropletScepterProj : OrchidModShamanProjectile
	{
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Bone");
        } 
		
		public override void SafeSetDefaults()
		{
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 100;
            this.empowermentType = 1;
		}
		
        public override void AI()
        {	
			if (projectile.timeLeft == 100) {
				float rand = (float)(Main.rand.Next(4) - 2f);
				projectile.velocity.X += rand;
				projectile.velocity.Y += rand;
			}
		
			projectile.rotation += 0.25f;
			
            projectile.velocity = projectile.velocity * 0.95f;
			
			projectile.alpha += 2;
			
			for(int i=0; i<1; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
            }
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<3; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 3f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
	}
}