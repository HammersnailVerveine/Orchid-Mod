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
		private bool faster;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Perishing Slash");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 140;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			AIType = ProjectileID.Bullet;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Projectile.alpha += 30;
			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			if (Projectile.timeLeft == 140)
			{
				storeVelocity = Projectile.velocity;
				storeDamage = Projectile.damage;
				faster = modPlayer.GetNbShamanicBonds() > 2;
			}
			
			if (Projectile.timeLeft > 35)
			{
				Projectile.velocity *= 0f;
				Projectile.damage = 0;
				dustScale += 0.0195f;
				dustSpawned = false;

				if (faster)
				{
					Projectile.timeLeft--;
					dustScale += 0.0195f;
					Projectile.netUpdate = true;
				}
			}

			if (Projectile.timeLeft == 71 || Projectile.timeLeft == 111)
			{
				int dustDist = 20;
				if (Projectile.timeLeft == 111)
					dustDist = 10;

				OrchidModProjectile.spawnDustCircle(Projectile.Center, 6, dustDist, 10, true, 1.5f, 1f, 1.5f, true, true, false, 0, 0, true);
				Projectile.netUpdate = true;
			}

			if (Projectile.timeLeft <= 35)
			{
				Projectile.damage = storeDamage;
				Projectile.velocity = storeVelocity;
				Projectile.extraUpdates = 1;
				Projectile.netUpdate = true;

				if (dustSpawned == false)
				{
					dustSpawned = true;
					OrchidModProjectile.spawnDustCircle(Projectile.Center, 6, 20, 8, false, 1f, 1f, 1f, true, true, false, 0, 0, true);
					OrchidModProjectile.spawnDustCircle(Projectile.Center, 6, 20, 8, true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);
					for (int i = 0; i < 10; i++)
					{
						int index = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
						Main.dust[index].scale = 1.5f;
						Main.dust[index].velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(20));
						Main.dust[index].noGravity = true;
					}
				}
			}

			for (int i = 0; i < 3; i++)
			{
				Vector2 Position = Projectile.position;
				int index = Dust.NewDust(Position, Projectile.width, Projectile.height, 6);
				Main.dust[index].scale = (float)90 * 0.010f + dustScale / 3;
				Main.dust[index].velocity *= 0.2f;
				Main.dust[index].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].velocity *= 5f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (Main.rand.NextBool(4)) target.AddBuff((24), 5 * 60);
		}
	}
}