using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Equipment.Abyss
{
	public class AbyssPortal : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyssal Gate");
		}
		public override void SafeSetDefaults()
		{
			projectile.width = 172;
			projectile.height = 139;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.timeLeft = 250;
			projectile.penetrate = 100;
			projectile.scale = 1f;
			Main.projFrames[projectile.type] = 4;
			projectile.timeLeft = 1000;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void AI()
		{
			projectile.frameCounter++;

			if (projectile.frameCounter > 5)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame > 3)
			{
				projectile.frame = 0;
			}

			if (projectile.timeLeft == 1000)
			{
				for (int i = 0; i < 100; i++)
				{
					int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
					Main.dust[dust2].velocity /= 10f;
					Main.dust[dust2].scale = 2.5f;
					Main.dust[dust2].noGravity = true;
					Main.dust[dust2].noLight = true;
				}
			}

			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
			Main.dust[dust].velocity /= 10f;
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;

			projectile.ai[1]++;
			if (projectile.ai[1] >= 0)
			{
				Player player = Main.player[projectile.owner];
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

				int dmg = (int)(150 * modPlayer.shamanDamage + 5E-06f);
				Projectile.NewProjectile(projectile.position.X, projectile.Center.Y, 0f, 14f, mod.ProjectileType("AbyssPortalLaser"), dmg, 0f, projectile.owner, projectile.whoAmI, 0f);
				projectile.ai[1] = -360;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172, projectile.velocity.X * 0f, projectile.velocity.Y * 0f);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
			}
			Main.player[projectile.owner].GetModPlayer<OrchidModPlayer>().doubleTapCooldown = 0;
		}
	}
}
