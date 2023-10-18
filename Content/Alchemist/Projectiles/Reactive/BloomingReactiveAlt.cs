using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Projectiles.Reactive
{
	public class BloomingReactiveAlt : AlchemistProjReactive
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			this.spawnTimeLeft = 600;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Flower");
		}

		public override void SafeAI()
		{
			Projectile.rotation += 0.02f;

			if (Main.rand.Next(60) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<Content.Dusts.BloomingDust>());
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Despawn()
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<Content.Dusts.BloomingDust>());
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void SafeKill(int timeLeft, Player player, OrchidAlchemist modPlayer)
		{
			SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
			int proj = ProjectileType<ReactiveSpawn.BloomingPetal>();
			int dmg = Projectile.damage;
			for (int i = 0; i < 8; i++)
			{
				Vector2 perturbedSpeed = new Vector2(0f, 0.5f).RotatedBy(MathHelper.ToRadians(22.5f + i * 45));
				int newProj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, proj, dmg, 0.0f, Projectile.owner, 0.0f, 0.0f);
			}
			for (int i = 0; i < 8; i++)
			{
				Vector2 perturbedSpeed = new Vector2(0f, 1f).RotatedBy(MathHelper.ToRadians(i * 45));
				int newProj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, proj, dmg, 0.0f, Projectile.owner, 0.0f, 0.0f);
			}
		}
	}
}