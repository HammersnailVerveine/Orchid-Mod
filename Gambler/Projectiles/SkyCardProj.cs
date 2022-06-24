using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
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
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 300;
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
				OrchidModProjectile.spawnDustCircle(Projectile.Center, 64, 5, 5, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
				this.initialized = true;
			}

			Projectile.rotation += this.rotation;

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64);
			Main.dust[dust].scale *= 1.5f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;

			if (Projectile.ai[0] != -1f && Main.myPlayer == Projectile.owner)
			{
				if (Projectile.position.Y >= Projectile.ai[0])
				{
					OrchidModProjectile.spawnDustCircle(Projectile.Center, 64, 5, 5, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
					Vector2 mousePos = Main.MouseWorld;
					Projectile.velocity.X = (mousePos.X > Projectile.Center.X ? Projectile.velocity.Y : -Projectile.velocity.Y);
					Projectile.velocity.Y = 0f;
					Projectile.ai[0] = -1f;
					Projectile.netUpdate = true;
				}
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer)
		{
			if (Projectile.ai[1] != 1f && Projectile.owner == Main.myPlayer)
			{
				modPlayer.gamblerSeedCount += 6 + (modPlayer.gamblerLuckySprout ? 2 : 0);
				if (modPlayer.gamblerSeedCount > 99) {
					modPlayer.gamblerSeedCount = 0;
					Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
					int projType = ProjectileType<Gambler.Projectiles.SkyCardProjAlt>();
					bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					int newProjectile = DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.Center.Y, vel.X, vel.Y, projType, Projectile.damage, Projectile.knockBack, Projectile.owner), dummy);
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
			SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 64, 5, 5, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
		}
	}
}