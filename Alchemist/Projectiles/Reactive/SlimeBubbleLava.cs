using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Reactive
{
	public class SlimeBubbleLava : AlchemistProjReactive
	{
		private int animDirection;

		public override void SafeSetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.alpha = 64;
			this.spawnTimeLeft = 600;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Slime Bubble");
		}

		public override void OnSpawn()
		{
			animDirection = (Main.rand.NextBool(2) ? 1 : -1);
		}

		public override void SafeAI()
		{
			Projectile.velocity.Y *= 0.95f;
			Projectile.velocity.X *= 0.99f;
			Projectile.rotation += (0.05f * (0.2f - Math.Abs(Projectile.rotation)) + 0.001f) * animDirection;
			if (Math.Abs(Projectile.rotation) >= 0.2f)
			{
				Projectile.rotation = 0.2f * animDirection;
				animDirection *= -1;
			}

			if (Main.rand.NextBool(20))
			{
				int Alpha = 175;
				Color newColor = new Color(255, 80, 0, 100);
				int dust = Dust.NewDust(Projectile.position + Vector2.One * 6f, Projectile.width, Projectile.height, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
			}

			if (Main.rand.NextBool(10))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Despawn()
		{
			for (int i = 0; i < 5; i++)
			{
				int Alpha = 175;
				Color newColor = new Color(255, 80, 0, 100);
				int dust = Dust.NewDust(Projectile.position + Vector2.One * 6f, Projectile.width, Projectile.height, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void SafeKill(int timeLeft, Player player, OrchidAlchemist modPlayer)
		{
			SoundEngine.PlaySound(SoundID.Item85, Projectile.Center);
			int proj = ProjectileType<Alchemist.Projectiles.Reactive.ReactiveSpawn.AlchemistSlimeLava>();
			int dmg = Projectile.damage;
			int rand = Main.rand.Next(3) + 2;
			for (int i = 0; i < rand; i++)
			{
				Vector2 perturbedSpeed = new Vector2(player.velocity.X, player.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y - 1f, proj, dmg, 0.0f, Projectile.owner, 0.0f, 0.0f);
			}
		}
	}
}