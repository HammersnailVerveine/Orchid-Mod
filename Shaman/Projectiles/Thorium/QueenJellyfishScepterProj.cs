using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class QueenJellyfishScepterProj : OrchidModShamanProjectile
    {
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sorta Fish Consisting of Gelatin");
        } 
        public override void SafeSetDefaults()
        {
            projectile.width = 22;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 90;
			projectile.penetrate = 4;		
			projectile.alpha = 126;
			projectile.ignoreWater = true;   
        }

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, 0.5f, 0.5f, 0f);
			projectile.velocity.Y += 0.1f;
			
			if (Main.rand.Next(3) == 0) {
				int index2 = Dust.NewDust(projectile.position - projectile.velocity * 0.25f, projectile.width, projectile.height, 64, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(80, 110) * 0.013f);
				Main.dust[index2].velocity = projectile.velocity / 3;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 20 ; i ++ ) {
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);
					 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - projectile.width/2;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - projectile.height/2;
					
				Vector2 dustPosition = new Vector2(posX, posY);
					
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
				Main.dust[index2].velocity *= 1f;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.3f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			projectile.damage += (2 + OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod));
			
			projectile.penetrate--;
			projectile.timeLeft = 90;
			spawnDustCircle(64, 20);
            if (projectile.penetrate < 0) projectile.Kill();
            projectile.velocity.X = (projectile.velocity.X != oldVelocity.X) ? -oldVelocity.X : projectile.velocity.X * 0.8f;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y * 1.2f;
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 87);
			projectile.netUpdate = true;
            return false;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<13; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
        {
			projectile.Kill();
			spawnDustCircle(64, 20);
		}
    }
}