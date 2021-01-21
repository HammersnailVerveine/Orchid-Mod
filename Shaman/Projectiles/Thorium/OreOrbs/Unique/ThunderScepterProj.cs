using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{
    public class ThunderScepterProj : OrchidModShamanProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thunder bolt");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
			projectile.scale = 1f;
            projectile.aiStyle = 0;
			projectile.timeLeft = 20;
			projectile.tileCollide = true;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;   
			projectile.alpha = 255;
			this.empowermentType = 3;
			this.empowermentLevel = 1;
			this.spiritPollLoad = 2;
        }
		
        public override void AI()
        {    
             if (projectile.timeLeft == 20)
            {	
				projectile.ai[0] = (Main.rand.Next(40) + 20);
				
				if (Main.player[projectile.owner].velocity.Y == 0) {
					projectile.ai[0] /= 2;
				}
				
				if (Main.rand.Next(2) == 0) {
					projectile.ai[0] = -projectile.ai[0];
				}
				
				for (int index1 = 0; index1 < 5; ++index1)
				{	
					float x = projectile.position.X - projectile.velocity.X / 2f * (float) index1;
					float y = projectile.position.Y - projectile.velocity.Y / 2f * (float) index1;
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 159, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].alpha = projectile.alpha;
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = (float) 150 * 0.015f;
					Main.dust[index2].velocity *= 2f;
					Main.dust[index2].noGravity = true;
				}
			}		
			
		    for (int index1 = 0; index1 < 9; ++index1)
            {	
				if (index1 % 3 == 0) {
					float x = projectile.position.X - projectile.velocity.X / 10f * (float) index1;
					float y = projectile.position.Y - projectile.velocity.Y / 10f * (float) index1;
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 159, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].alpha = projectile.alpha;
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					
					Main.dust[index2].scale = 65f * 0.015f;
					if (projectile.timeLeft > 12) {
						Main.dust[index2].scale = 95f * 0.015f;
					} else if (projectile.timeLeft > 8) {
						Main.dust[index2].scale = 80f * 0.015f;
					}
					
					Main.dust[index2].velocity *= 0.0f;
					Main.dust[index2].noGravity = true;
				}
            }
				
            if (projectile.timeLeft == 20)
            {	
				Vector2 projectileVelocity = ( new Vector2(projectile.velocity.X, projectile.velocity.Y ).RotatedBy(MathHelper.ToRadians(projectile.ai[0] / 2)));
				projectile.velocity = projectileVelocity;
			}
			
            if (projectile.timeLeft == 12)
            {	
				Vector2 projectileVelocity = ( new Vector2(projectile.velocity.X, projectile.velocity.Y ).RotatedBy(MathHelper.ToRadians(-((projectile.ai[0])*2))));
				projectile.velocity = projectileVelocity;
			}
			
            if (projectile.timeLeft == 8)
            {	
				Vector2 projectileVelocity = ( new Vector2(projectile.velocity.X, projectile.velocity.Y ).RotatedBy(MathHelper.ToRadians((projectile.ai[0])*2)));
				projectile.velocity = projectileVelocity;
			}
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
        {
			for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(target.position, target.width, target.height, 159);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = (float) 120 * 0.015f;
				Main.dust[dust].velocity *= 1.5f;
            }
			
			if (modPlayer.shamanOrbUnique != ShamanOrbUnique.GRANDTHUNDERBIRD) {
				modPlayer.shamanOrbUnique = ShamanOrbUnique.GRANDTHUNDERBIRD;
				modPlayer.orbCountUnique = 0;
			}
			modPlayer.orbCountUnique ++;
			//modPlayer.sendOrbCountPackets();
			
			if (modPlayer.orbCountUnique == 5)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("ThunderScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			
			if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1 && modPlayer.orbCountUnique < 5)
			{
				modPlayer.orbCountUnique += 5;
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("ThunderScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
				player.ClearBuff(mod.BuffType("ShamanicBaubles"));
				//modPlayer.sendOrbCountPackets();
			}
			
			if (modPlayer.orbCountUnique == 20) {	
				modPlayer.orbCountUnique = 0;
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 5f, mod.ProjectileType("ThunderScepterOrbProj"), 0, 0.0f, projectile.owner, 0.0f, 0.0f);
			}
		}
    }
}