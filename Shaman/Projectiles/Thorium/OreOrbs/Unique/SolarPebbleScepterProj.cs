using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{
    public class SolarPebbleScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 6;
            projectile.height = 6;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 35;	
			projectile.scale = 1f;
			aiType = ProjectileID.Bullet; 
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Burst");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {
			Lighting.AddLight(projectile.Center, 0.010f, 0.010f, 0f);
            for(int i=0; i<2; i++)
			{   
			    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, projectile.velocity.X / 2, projectile.velocity.Y / 2);
				Main.dust[dust].velocity = projectile.velocity;
				Main.dust[dust].scale = 0.8f + ((projectile.timeLeft)/45f) * 1.8f;
				Main.dust[dust].noGravity = true;
			}
						
            if (Main.rand.Next(5) == 0)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 269);
				Main.dust[dust].scale = 0.8f + ((projectile.timeLeft)/45f) * 0.9f;
				Main.dust[dust].velocity.X += projectile.velocity.X / 2;
				Main.dust[dust].velocity.Y += projectile.velocity.Y / 2;
            }
			
			if (projectile.timeLeft == 35) {
				projectile.ai[0] = (((float)Main.rand.Next(10) / 10f) - 0.5f);
			}
			projectile.velocity *= 1.03f;
			Vector2 projectileVelocity = ( new Vector2(projectile.velocity.X, projectile.velocity.Y ).RotatedBy(MathHelper.ToRadians(projectile.ai[0])));
			projectile.velocity = projectileVelocity;
			projectile.netUpdate = true;
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
            return true;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<13; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 269);
				Main.dust[dust].velocity.X += projectile.velocity.X / 2;
				Main.dust[dust].velocity.Y += projectile.velocity.Y / 2;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null) {
				target.AddBuff((thoriumMod.BuffType("Melting")), 2 * 60);
			}
			
			if (modPlayer.shamanOrbUnique != ShamanOrbUnique.ECLIPSE) {
				modPlayer.shamanOrbUnique = ShamanOrbUnique.ECLIPSE;
				modPlayer.orbCountUnique = 0;
			}
			modPlayer.orbCountUnique ++;

			if (modPlayer.orbCountUnique == 1)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("SolarPebbleScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
				
			if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1 && modPlayer.orbCountUnique < 5)
			{
				modPlayer.orbCountUnique += 5;
				player.ClearBuff(mod.BuffType("ShamanicBaubles"));
			}
			
			if (modPlayer.orbCountUnique == 18) {
				
				for (int i = 0 ; i < 10 ; i ++ ) {
					Vector2 projectileVelocity = ( new Vector2(8f, 0f).RotatedByRandom(MathHelper.ToRadians(360)));
					Projectile.NewProjectile(player.Center.X, player.position.Y - 79, projectileVelocity.X, projectileVelocity.Y, mod.ProjectileType("SolarPebbleScepterOrbProj"), 0, 0, projectile.owner, 0f, 0f);
				}
				
				for (int i = 0 ; i < 3 ; i ++ ) {
					Vector2 projectileVelocity = ( new Vector2(10f, 0f).RotatedByRandom(MathHelper.ToRadians(360)));
					Projectile.NewProjectile(player.Center.X, player.position.Y - 79, projectileVelocity.X, projectileVelocity.Y, mod.ProjectileType("SolarPebbleScepterOrbProjAlt"), projectile.damage * 5, 0, projectile.owner, 0f, 0f);
				}
				
				modPlayer.orbCountUnique = 0;
			}
		}
    }
}