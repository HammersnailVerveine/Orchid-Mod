using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Reactive
{
	public class BloomingReactiveAlt : AlchemistProjReactive
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.timeLeft = 600;
			projectile.scale = 1f;
			this.spawnTimeLeft = 600;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flower");
		}

		public override void SafeAI()
		{
			projectile.rotation += 0.02f;

			if (Main.rand.Next(60) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<Content.Dusts.BloomingDust>());
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Despawn()
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<Content.Dusts.BloomingDust>());
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void SafeKill(int timeLeft, Player player, OrchidModPlayer modPlayer)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 17);
			int proj = ProjectileType<Alchemist.Projectiles.Reactive.ReactiveSpawn.BloomingPetal>();
			int dmg = projectile.damage;
			for (int i = 0; i < 8; i++)
			{
				Vector2 perturbedSpeed = new Vector2(0f, 0.5f).RotatedBy(MathHelper.ToRadians(22.5f + i * 45));
				int newProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
			}
			for (int i = 0; i < 8; i++)
			{
				Vector2 perturbedSpeed = new Vector2(0f, 1f).RotatedBy(MathHelper.ToRadians(i * 45));
				int newProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
			}
		}
	}
}