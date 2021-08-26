using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Reactive
{
	public class SlimeBubble : AlchemistProjReactive
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.timeLeft = 600;
			projectile.scale = 1f;
			projectile.alpha = 64;
			this.spawnTimeLeft = 600;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slime Bubble");
		}

		public override void SafeAI()
		{
			projectile.velocity.Y *= 0.95f;
			projectile.velocity.X *= 0.99f;
			projectile.rotation += 0.02f;

			if (Main.rand.Next(20) == 0)
			{
				int Alpha = 175;
				Color newColor = new Color(0, 80, (int)byte.MaxValue, 100);
				int dust = Dust.NewDust(projectile.position + Vector2.One * 6f, projectile.width, projectile.height, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void Despawn()
		{
			for (int i = 0; i < 5; i++)
			{
				int Alpha = 175;
				Color newColor = new Color(0, 80, (int)byte.MaxValue, 100);
				int dust = Dust.NewDust(projectile.position + Vector2.One * 6f, projectile.width, projectile.height, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void SafeKill(int timeLeft, Player player, OrchidModPlayer modPlayer)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 85);
			int proj = player.HasBuff(BuffType<Alchemist.Buffs.KingSlimeFlaskBuff>()) ? ProjectileType<Alchemist.Projectiles.Reactive.ReactiveSpawn.AlchemistSlimeJungle>()
			: ProjectileType<Alchemist.Projectiles.Reactive.ReactiveSpawn.AlchemistSlime>();
			int dmg = projectile.damage;
			int rand = Main.rand.Next(3) + 2;
			for (int i = 0; i < rand; i++)
			{
				Vector2 perturbedSpeed = new Vector2(player.velocity.X, player.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y - 1f, proj, dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
			}
		}
	}
}