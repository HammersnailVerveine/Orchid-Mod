using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Alchemist.Projectiles.Air
{
    public class ShadowChestFlaskProj : OrchidModAlchemistProjectile
    {
		Vector2 startPosition = new Vector2(0, 0);
		Vector2 startVelocity = new Vector2(0, 0);
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alchemical Shadow");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
			projectile.scale = 1f;
			projectile.alpha = 172;
            projectile.aiStyle = 29;
			projectile.timeLeft = 120;
			projectile.penetrate = -1; 
        }
		
        public override void AI()
        {    
            if (projectile.timeLeft == 120) {	
				this.startPosition = projectile.position;
				this.startVelocity = projectile.velocity;
			}	
		    if (Main.rand.Next(2) == 0) {	
				float x = projectile.position.X - projectile.velocity.X / 10f;
				float y = projectile.position.Y - projectile.velocity.Y / 10f;
				int rand = Main.rand.Next(2) == 0 ? 21 : 70;
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, rand, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 100, default(Color), 3.5f);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity *= 0.5f;
				Main.dust[dust].noGravity = true;
            }	
			
			Vector2 newMove = projectile.Center - this.startPosition;
			float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
			if (distanceTo < 10f && projectile.timeLeft < 100) {
				projectile.Kill();
			}
			
			projectile.rotation += 0.1f;
			projectile.velocity -= this.startVelocity * 0.02f;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 70);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity *= 3f;
            }
        }
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			OrchidModAlchemistNPC modTarget = target.GetGlobalNPC<OrchidModAlchemistNPC>();
			modTarget.alchemistAir = 600;
        }
    }
}