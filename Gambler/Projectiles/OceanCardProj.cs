using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class OceanCardProj : OrchidModGamblerProjectile
	{
		private bool firstCollide = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 600;
			projectile.penetrate = 10;
			this.gamblingChipChance = 5;
		}

		public override void SafeAI()
		{
			projectile.velocity.Y += 0.1f;
			if (projectile.velocity.Y > 0.5f)
			{
				this.firstCollide = false;
			}
			if (projectile.velocity.X > 0f && projectile.velocity.X < 3f)
			{
				projectile.velocity.X = 3f;
			}
			if (projectile.velocity.X <= 0f && projectile.velocity.X > -3f)
			{
				projectile.velocity.X = -3f;
			}

			projectile.rotation += projectile.velocity.X > 0 ? 0.15f : -0.15f;
			if (projectile.timeLeft % 600 == 0)
			{
				int dustType = 31;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = projectile.timeLeft > 590;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.penetrate < 0) projectile.Kill();
			projectile.velocity.Y = oldVelocity.Y < 0f ? -oldVelocity.Y : 0f;
			if (!firstCollide)
			{
				firstCollide = true;
			}
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.Kill();
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			}
			return false;
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (projectile.ai[1] != 1f && projectile.owner == Main.myPlayer)
			{
				modPlayer.gamblerSeedCount += 10 + (modPlayer.gamblerLuckySprout ? 3 : 0);
				if (modPlayer.gamblerSeedCount > 99) {
					modPlayer.gamblerSeedCount = 0;
					Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
					int projType = ProjectileType<Gambler.Projectiles.OceanCardProjAlt>();
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