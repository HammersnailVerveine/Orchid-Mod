using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Air
{
    public class CrimsonFlaskProj : OrchidModAlchemistProjectile
    {
		private int sporeType = 127;
		private int sporeDamage = 0;
		
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 18;
            projectile.aiStyle = 0;
			projectile.timeLeft = 60;
			projectile.scale = 1f;
			projectile.penetrate = -1;
			projectile.friendly = false;
			Main.projFrames[projectile.type] = 3;
			this.catalytic = true;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom");
        }
		
        public override void AI() {
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			this.initialized = true;
			projectile.velocity *= 0.95f;
			if (projectile.timeLeft % 10 == 0) projectile.frame = projectile.frame == 2 ? 0 : projectile.frame + 1;
			
			if (projectile.ai[1] != 0f) {
				sporeType = (int)projectile.ai[1];
				this.sporeDamage = (int)projectile.ai[0];
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity) {
            this.Bounce(oldVelocity);
            return false;
        }
		
		public override void Catalyze(Player player, Projectile projectile, OrchidModGlobalProjectile modProjectile) {
			if (this.initialized) projectile.Kill();
		}
		
		public override void Kill(int timeLeft) {
			int range = 50;
			int nb = 10;
			OrchidModProjectile.spawnDustCircle(projectile.Center, sporeType, (int)(range / 2), nb, true, 1.5f, 1f, 2f);
			spawnGenericExplosion(projectile, projectile.damage, projectile.knockBack, range * 3, 2, false, 14);
			
			if (Main.rand.Next(2) == 0) {
				int spawnProj = 0;
				int spawnProj2 = 0;
				
				switch (sporeType) {
					case 6 :
						spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>();
						spawnProj2 = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProjAlt>();
						break;
					case 59 :
						spawnProj = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>();
						spawnProj2 = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProjAlt>();
						break;
					case 61 :
						spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
						spawnProj2 = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
						break;
					case 63 :
						spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>();
						spawnProj2 = ProjectileType<Alchemist.Projectiles.Air.AirSporeProjAlt>();
						break;
					default :
						break;
				}
				
				if (spawnProj != 0) {
					Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
					int newSpore = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, this.sporeDamage, 0f, projectile.owner);
					Main.projectile[newSpore].localAI[1] = 1f;
					vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj2, 0, 0f, projectile.owner);
				}
			}
		}
    }
}