using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SkyCardProj : OrchidModGamblerProjectile
	{
		private float rotation = 0f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Banana Star");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			this.gamblingChipChance = 15;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SafeAI()
		{
			if (!this.initialized)
			{
				this.rotation = 0.3f * (Main.rand.Next(2) == 0 ? -1 : 1);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 64, 5, 5, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
				this.initialized = true;
			}

			projectile.rotation += this.rotation;

			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
			Main.dust[dust].scale *= 1.5f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;

			if (projectile.ai[0] != -1f && Main.myPlayer == projectile.owner)
			{
				if (projectile.position.Y >= projectile.ai[0])
				{
					OrchidModProjectile.spawnDustCircle(projectile.Center, 64, 5, 5, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
					Vector2 mousePos = Main.MouseWorld;
					projectile.velocity.X = (mousePos.X > projectile.Center.X ? projectile.velocity.Y : -projectile.velocity.Y);
					projectile.velocity.Y = 0f;
					projectile.ai[0] = -1f;
					projectile.netUpdate = true;
				}
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (projectile.ai[1] != 1f && projectile.owner == Main.myPlayer)
			{
				modPlayer.gamblerSeedCount += 6 + (modPlayer.gamblerLuckySprout ? 2 : 0);
				if (modPlayer.gamblerSeedCount > 99) {
					modPlayer.gamblerSeedCount = 0;
					Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
					int projType = ProjectileType<Gambler.Projectiles.SkyCardProjAlt>();
					bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
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
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 9);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 64, 5, 5, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
		}
	}
}