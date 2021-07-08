using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class BulbScepterProj : OrchidModShamanProjectile
    {
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiky Bulbyball");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 38;
            projectile.height = 38;
            projectile.friendly = true;
            projectile.aiStyle = 2;
			projectile.timeLeft = 150;	
            projectile.scale = 0.7f;		
			projectile.penetrate = 3;
			this.empowermentType = 4;
        }
		
        public override void AI()
        {
            for (int index1 = 0; index1 < 1; ++index1)
            {
                projectile.velocity = projectile.velocity * 1.002f;						
			}
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<3; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 248);
                int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 248);
				Main.dust[dust].noGravity = true;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust2].scale = 1.7f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}