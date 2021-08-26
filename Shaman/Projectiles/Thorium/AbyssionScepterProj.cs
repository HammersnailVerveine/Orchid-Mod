using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class AbyssionScepterProj : OrchidModShamanProjectile
    {
		public Vector2 storedVelocity;
		public float dustVelocity = 0f;
		public int storedDamage = 0;
		
        public override void SafeSetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 120;
			projectile.alpha = 255;
			projectile.penetrate = 3;
			projectile.tileCollide = false;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Old Gods Energy");
        } 
		
        public override void AI()
        {	
			if (projectile.timeLeft == 120) {
				projectile.position.X += projectile.velocity.X * 50;
				projectile.position.Y += projectile.velocity.Y * 50;
				this.storedVelocity = projectile.velocity * -1f;
				projectile.velocity *= 0f;
				this.storedDamage = projectile.damage;
				projectile.damage = 0;
			}
			
			if (projectile.timeLeft > 50) {
				this.dustVelocity += 0.07f;
				int dust = Dust.NewDust(projectile.Center, 1, 1, 27);
				Main.dust[dust].velocity *= dustVelocity;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				if (projectile.timeLeft % 10 == 0) {
					spawnDustCircle(27, (int)(120 - projectile.timeLeft) / 3);
				}
			} else {
				projectile.damage = this.storedDamage;
				projectile.velocity = storedVelocity * 1.75f;
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
				Main.dust[dust].velocity = projectile.velocity / 2f;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
				Main.dust[dust2].velocity = projectile.velocity / 3f;
				Main.dust[dust2].scale = 2.5f;
				Main.dust[dust2].noGravity = true;
			}
		}
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 10 ; i ++ ) {
				double deg = (i * (72 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);
					 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - projectile.width/2 + projectile.velocity.X + 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - projectile.height/2 + projectile.velocity.Y + 4;
					
				Vector2 dustPosition = new Vector2(posX, posY);
					
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 3f;
            }
			spawnDustCircle(27, 20);
			spawnDustCircle(27, 30);
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(153, 60 * 5); // Shadowflame
		}
    }
}