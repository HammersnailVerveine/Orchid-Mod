using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 10;
			this.gamblingChipChance = 5;
		}

		public override void SafeAI()
		{
			Projectile.velocity.Y += 0.1f;
			if (Projectile.velocity.Y > 0.5f)
			{
				this.firstCollide = false;
			}
			if (Projectile.velocity.X > 0f && Projectile.velocity.X < 3f)
			{
				Projectile.velocity.X = 3f;
			}
			if (Projectile.velocity.X <= 0f && Projectile.velocity.X > -3f)
			{
				Projectile.velocity.X = -3f;
			}

			Projectile.rotation += Projectile.velocity.X > 0 ? 0.15f : -0.15f;
			if (Projectile.timeLeft % 600 == 0)
			{
				int dustType = 31;
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = Projectile.timeLeft > 590;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.penetrate < 0) Projectile.Kill();
			Projectile.velocity.Y = oldVelocity.Y < 0f ? -oldVelocity.Y : 0f;
			if (!firstCollide)
			{
				firstCollide = true;
			}
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.Kill();
				SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			}
			return false;
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (Projectile.ai[1] != 1f && Projectile.owner == Main.myPlayer)
			{
				modPlayer.gamblerSeedCount += 10 + (modPlayer.gamblerLuckySprout ? 3 : 0);
				if (modPlayer.gamblerSeedCount > 99) {
					modPlayer.gamblerSeedCount = 0;
					Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
					int projType = ProjectileType<Gambler.Projectiles.OceanCardProjAlt>();
					bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, projType, Projectile.damage, Projectile.knockBack, Projectile.owner), dummy);
					Main.projectile[newProjectile].ai[1] = 1f;
					Main.projectile[newProjectile].netUpdate = true;
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
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
			}
		}
	}
}