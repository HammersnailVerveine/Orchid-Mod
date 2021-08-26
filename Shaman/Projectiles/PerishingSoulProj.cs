using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles
{
	public class PerishingSoulProj : OrchidModShamanProjectile
	{
		private Vector2 storeVelocity;
		private int storeDamage;
		private float dustScale = 0;
		private bool dustSpawned;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Perishing Slash");
		}
		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 0;
			projectile.timeLeft = 160;
			projectile.friendly = true;
			projectile.tileCollide = true;
			aiType = ProjectileID.Bullet;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			projectile.alpha += 30;

			if (projectile.timeLeft == 160)
			{
				storeVelocity = projectile.velocity;
				storeDamage = projectile.damage;
			}
			if (projectile.timeLeft > 35)
			{
				projectile.velocity *= 0f;
				projectile.damage = 0;
				dustScale += 0.0195f;
				dustSpawned = false;

				Player player = Main.player[projectile.owner];
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 3)
				{
					projectile.timeLeft--;
					dustScale += 0.0195f;
					projectile.netUpdate = true;
				}
			}

			if (projectile.timeLeft == 80 || projectile.timeLeft == 115)
			{
				int dustDist = 20;
				if (projectile.timeLeft == 115)
					dustDist = 10;

				OrchidModProjectile.spawnDustCircle(projectile.Center, 6, dustDist, 10, true, 1.5f, 1f, 1.5f, true, true, false, 0, 0, true);
				projectile.netUpdate = true;
			}

			if (projectile.timeLeft <= 35)
			{
				projectile.damage = storeDamage;
				projectile.velocity = storeVelocity;
				projectile.extraUpdates = 1;
				projectile.netUpdate = true;

				if (dustSpawned == false)
				{
					dustSpawned = true;
					OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 20, 8, false, 1f, 1f, 1f, true, true, false, 0, 0, true);
					OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 20, 8, true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);
					for (int i = 0; i < 10; i++)
					{
						int index = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
						Main.dust[index].scale = 1.5f;
						Main.dust[index].velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(20));
						Main.dust[index].noGravity = true;
					}
				}
			}

			for (int i = 0; i < 3; i++)
			{
				Vector2 Position = projectile.position;
				int index = Dust.NewDust(Position, projectile.width, projectile.height, 6);
				Main.dust[index].scale = (float)90 * 0.010f + dustScale / 3;
				Main.dust[index].velocity *= 0.2f;
				Main.dust[index].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].velocity *= 5f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (Main.rand.Next(4) == 0) target.AddBuff((24), 5 * 60);
		}
	}
}