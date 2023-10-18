using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.DataStructures;

namespace OrchidMod.Content.Alchemist.Projectiles.Reactive
{
	public class SeafoamBubble : AlchemistProjReactive
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
			// DisplayName.SetDefault("Seafoam Bubble");
		}

		public override void OnSpawn(IEntitySource source)
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
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 217);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void Despawn()
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 217);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void SafeKill(int timeLeft, Player player, OrchidAlchemist modPlayer)
		{
			SoundEngine.PlaySound(SoundID.Item85, Projectile.Center);
			int proj = ProjectileType<Content.Alchemist.Projectiles.Water.SeafoamVialProj>();
			int dmg = Projectile.damage;
			for (int i = 0; i < 8; i++)
			{
				Vector2 perturbedSpeed = new Vector2(0f, 12.5f).RotatedBy(MathHelper.ToRadians(22.5f + i * 45));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0.5f, Projectile.owner, 0.0f, 0.0f);
			}

			for (int i = 0; i < 3; i++)
			{
				Vector2 perturbedSpeed = new Vector2(0f, 12.5f).RotatedByRandom(MathHelper.ToRadians(180));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0.5f, Projectile.owner, 0.0f, 0.0f);
			}
		}
	}
}