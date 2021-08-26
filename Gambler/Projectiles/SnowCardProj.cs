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

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int dustType = 31;
			Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			int rand = 10 - (modPlayer.gamblerLuckySprout ? 2 : 0);
			if (Main.rand.Next(rand) == 0 && projectile.ai[1] != 1f)
			{
				Vector2 vel = (new Vector2(0f, -0.5f).RotatedBy(MathHelper.ToRadians(10)));
				int projType = ProjectileType<Gambler.Projectiles.SnowCardProjAlt>();
				bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
				for (int i = 0; i < 5; i++)
				{
					Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
				}
			}
			Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
		}
	}
}