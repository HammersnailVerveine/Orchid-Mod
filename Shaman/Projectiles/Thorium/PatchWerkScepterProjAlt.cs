using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class PatchWerkScepterProjAlt : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Maggot");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 14;
            projectile.height = 10;
            projectile.aiStyle = 63;
            projectile.friendly = true;
            projectile.timeLeft = 180;
            projectile.penetrate = 3;
			projectile.scale = 1f;
			Main.projFrames[projectile.type] = 2;
            this.empowermentType = 4;
		}
		
        public override void AI()
        {
			projectile.frameCounter++;
			
			if (projectile.frameCounter > 5)
			{
			   projectile.frame++;
               projectile.frameCounter = 0;
			}
            if (projectile.frame > 2)
            {
               projectile.frame = 0;
            }
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity.X = 0;
            projectile.velocity.Y = -3;
            return false;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 16);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].velocity = projectile.velocity / 2;
            }
        }
    }
}
