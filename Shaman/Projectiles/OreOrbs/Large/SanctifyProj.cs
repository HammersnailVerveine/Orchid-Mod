using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Large
{
	public class SanctifyProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Magic");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 32;
			projectile.height = 34;
			projectile.aiStyle = 44;
			projectile.friendly = true;
			projectile.scale = 1f;
			Main.projFrames[projectile.type] = 4;
			projectile.timeLeft = 100;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			projectile.rotation += 0.1f;

			if (projectile.timeLeft == 100)
			{
				spawnDustCircle(169, 30);
				spawnDustCircle(169, 20);
				spawnDustCircle(169, 10);
			}

			if (modPlayer.timer120 % 15 == 0)
			{
				projectile.frame = (projectile.frame + 1) > 4 ? 0 : projectile.frame + 1;
			}

			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 169, projectile.velocity.X / 2, projectile.velocity.Y / 2);
			Main.dust[dust].scale = 1.2f;
			Main.dust[dust].noGravity = true;
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 10; i++)
			{
				double deg = (i * (72 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter);
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter);

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = projectile.velocity * 20 / distToCenter;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			spawnDustCircle(169, 30);
			spawnDustCircle(169, 20);
			spawnDustCircle(169, 10);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.shamanOrbLarge != ShamanOrbLarge.SANCTIFY)
			{
				modPlayer.shamanOrbLarge = ShamanOrbLarge.SANCTIFY;
				modPlayer.orbCountLarge = 0;
			}
			modPlayer.orbCountLarge++;

			float orbX = player.position.X + player.width / 2;
			float orbY = player.position.Y;

			if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1 && modPlayer.orbCountLarge < 5)
			{
				modPlayer.orbCountLarge += 5;
				Projectile.NewProjectile(orbX - 43, orbY - 38, 0f, 0f, mod.ProjectileType("SanctifyOrb"), 0, 0, projectile.owner, 0f, 0f);
				player.ClearBuff(mod.BuffType("ShamanicBaubles"));
			}

			if (modPlayer.orbCountLarge == 5)
				Projectile.NewProjectile(orbX - 43, orbY - 38, 0f, 0f, mod.ProjectileType("SanctifyOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 10)
				Projectile.NewProjectile(orbX - 30, orbY - 48, 0f, 0f, mod.ProjectileType("SanctifyOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 15)
				Projectile.NewProjectile(orbX - 15, orbY - 53, 0f, 0f, mod.ProjectileType("SanctifyOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 20)
				Projectile.NewProjectile(orbX, orbY - 55, 0f, 0f, mod.ProjectileType("SanctifyOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 25)
				Projectile.NewProjectile(orbX + 15, orbY - 53, 0f, 0f, mod.ProjectileType("SanctifyOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 30)
				Projectile.NewProjectile(orbX + 30, orbY - 48, 0f, 0f, mod.ProjectileType("SanctifyOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 35)
				Projectile.NewProjectile(orbX + 43, orbY - 38, 0f, 0f, mod.ProjectileType("SanctifyOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge > 35)
			{
				int dmg = (int)(30 * player.GetModPlayer<OrchidModPlayer>().shamanDamage);
				Projectile.NewProjectile(orbX - 43, orbY - 38, -3f, -5f, mod.ProjectileType("SanctifyOrbHoming"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX - 30, orbY - 48, -2f, -5f, mod.ProjectileType("SanctifyOrbHoming"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX - 15, orbY - 53, -1f, -5f, mod.ProjectileType("SanctifyOrbHoming"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX, orbY - 55, 0f, -5f, mod.ProjectileType("SanctifyOrbHoming"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX + 15, orbY - 53, 1f, -5f, mod.ProjectileType("SanctifyOrbHoming"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX + 30, orbY - 48, 2f, -5f, mod.ProjectileType("SanctifyOrbHoming"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX + 43, orbY - 38, 3f, -5f, mod.ProjectileType("SanctifyOrbHoming"), dmg, 0f, projectile.owner, 0f, 0f);
				modPlayer.orbCountLarge = 0;
			}
		}
	}
}
