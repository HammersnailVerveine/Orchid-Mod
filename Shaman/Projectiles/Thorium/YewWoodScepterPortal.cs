using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class YewWoodScepterPortal : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 38;
			projectile.height = 40;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 500;
			projectile.scale = 1f;
			Main.projFrames[projectile.type] = 4;
			projectile.alpha = 32;
			projectile.tileCollide = false;
			projectile.penetrate = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Portal");
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (projectile.timeLeft == 500)
			{
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = 2f;
				}
			}

			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 7 == 0)
				projectile.frame++;
			if (projectile.frame == 4)
				projectile.frame = 0;

			if (Main.rand.Next(3) == 0)
			{
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				int dust = Dust.NewDust(pos, projectile.width, projectile.height, 27, 0f, 0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;
			}

			if (projectile.timeLeft % 60 == 0)
			{
				int dmg = (int)(18 * player.GetModPlayer<OrchidModPlayer>().shamanDamage);
				Projectile.NewProjectile(projectile.position.X + projectile.width / 2, projectile.position.Y + projectile.height / 2, 0f, 0f, mod.ProjectileType("YewWoodScepterPortalProj"), dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
				projectile.netUpdate = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
			}
		}
	}
}