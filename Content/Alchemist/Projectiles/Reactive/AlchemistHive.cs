using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Alchemist.Projectiles.Reactive
{
	public class AlchemistHive : AlchemistProjReactive
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 30;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.alpha = 32;
			Main.projFrames[Projectile.type] = 5;
			this.spawnTimeLeft = 600;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hive");
		}

		public override void SafeAI()
		{
			Projectile.velocity.Y *= 0.95f;
			Projectile.velocity.X *= 0.99f;

			if (Main.rand.NextBool(20))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 153);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
			}

			if (Projectile.timeLeft % 10 == 0)
			{
				Projectile.frame++;
				if (Projectile.frame > 4)
				{
					Projectile.frame = 0;
				}
			}
		}

		public override void Despawn()
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 153);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void SafeKill(int timeLeft, Player player, OrchidAlchemist modPlayer)
		{
			SoundEngine.PlaySound(SoundID.Item85, Projectile.Center);
			int dmg = Projectile.damage;
			for (int i = 0; i < 10; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
				if (player.strongBees && Main.rand.NextBool(2))
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, vel, ProjectileID.GiantBee, (int)(dmg * 1.15f), 0f, Projectile.owner, 0f, 0f);
				else
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, vel, ProjectileID.Bee, dmg, 0f, Projectile.owner, 0f, 0f);
				}
			}
		}
	}
}