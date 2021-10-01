using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SnowCardProj : OrchidModGamblerProjectile
	{
		private Vector2 originalVelocity = new Vector2(0f, 0f);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 22;
			projectile.height = 26;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 200;
			projectile.penetrate = 2;
			projectile.alpha = 64;
			this.gamblingChipChance = 5;
			this.projectileTrail = true;
		}

		public override void SafeAI()
		{
			projectile.rotation += projectile.velocity.X != 0f ? (projectile.velocity.X * 1.5f) / 20f : (projectile.velocity.Y * 1.5f) / 20f;

			if (!this.initialized)
			{
				this.originalVelocity = projectile.velocity;
				this.initialized = true;
			}

			if (projectile.timeLeft % 10 == 0)
			{
				projectile.damage += 1;
			}
			projectile.velocity -= this.originalVelocity / 60f;
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (projectile.ai[1] != 1f && projectile.owner == Main.myPlayer)
			{
				modPlayer.gamblerSeedCount += 8 + (modPlayer.gamblerLuckySprout ? 2 : 0);
				if (modPlayer.gamblerSeedCount > 99) {
					modPlayer.gamblerSeedCount = 0;
					Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
					int projType = ProjectileType<Gambler.Projectiles.SnowCardProjAlt>();
					bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
					for (int i = 0; i < 5; i++)
					{
						int dustType = 31;
						Main.dust[Dust.NewDust(player.Center, 10, 10, dustType)].velocity *= 0.25f;
					}
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			int dustType = 31;
			for (int i = 0 ; i < 5 ; i ++) {
				Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
			}
		}
	}
}