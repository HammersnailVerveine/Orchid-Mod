using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
    public class HoneyProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 600;	
			projectile.scale = 1f;
			projectile.extraUpdates = 1;
            this.empowermentType = 2;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 5;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Honey Magic");
        } 
		
        public override void AI()
        {
			if ((projectile.velocity.X > 0.001f || projectile.velocity.X < -0.001f ) || (projectile.velocity.Y > 0.001f || projectile.velocity.Y < -0.001f )) {
				projectile.rotation += 0.23f;
				projectile.velocity.Y += 0.1f;
			}
			
			// if (Main.rand.Next(20) < projectile.timeLeft) {
				// Vector2 Position = projectile.position;
				// int index2 = Dust.NewDust(Position, projectile.width, projectile.height, 153);
				// Main.dust[index2].scale = 1f;
				// Main.dust[index2].velocity *= 1f;
				// Main.dust[index2].noGravity = true;
			// }
			
			if (Main.rand.Next(6) == 0) {
				Vector2 Position = projectile.position;
				int index2 = Dust.NewDust(Position, projectile.width, projectile.height, 152);
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 1f;
				Main.dust[index2].noGravity = true;
			}
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if ((projectile.velocity.X > 0.001f || projectile.velocity.X < -0.001f ) || (projectile.velocity.Y > 0.001f || projectile.velocity.Y < -0.001f )) {
				projectile.position += projectile.velocity * 3;
			}
			
			projectile.velocity *= 0f;
            return false;
        }
		
		public override void Kill(int timeLeft)
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int dmg = (int)(10 * modPlayer.shamanDamage);
						
            for(int i=0; i<13; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 153);
				Main.dust[dust].noGravity = true;
            }
			
			for (int i = 0 ; i < modPlayer.getNbShamanicBonds() ; i ++) {
				if (Main.rand.Next(4) == 0) {
					if (player.strongBees && Main.rand.Next(2) == 0) 
						Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 3 - Main.rand.Next(6), 3 - Main.rand.Next(6), 566, (int) (dmg * 1.15f), 0f, projectile.owner, 0f, 0f);
					else {
						Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 3 - Main.rand.Next(6), 3 - Main.rand.Next(6), 181, dmg, 0f, projectile.owner, 0f, 0f);
					}
				}
			}
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.shamanOrbUnique != ShamanOrbUnique.HONEY) {
				modPlayer.shamanOrbUnique = ShamanOrbUnique.HONEY;
				modPlayer.orbCountUnique = 0;
			}
			modPlayer.orbCountUnique ++;
			//modPlayer.sendOrbCountPackets();
			
			if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1 && modPlayer.orbCountUnique < 5)
			{
				modPlayer.orbCountUnique += 5;
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("HoneyOrb1"), 0, 0, projectile.owner, 0f, 0f);
				player.ClearBuff(mod.BuffType("ShamanicBaubles"));
				//modPlayer.sendOrbCountPackets();
			}
			
			if (modPlayer.orbCountUnique == 5)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("HoneyOrb1"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountUnique == 10)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("HoneyOrb2"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountUnique == 15)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 79, 0f, 0f, mod.ProjectileType("HoneyOrb3"), 0, 0, projectile.owner, 0f, 0f);
		}
    }
}