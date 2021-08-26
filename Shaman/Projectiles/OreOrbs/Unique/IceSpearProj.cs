using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class IceSpearProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.height = 40;
			projectile.width = 14;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 15;
			projectile.penetrate = 3;
			projectile.scale = 1.25f;
			this.projectileTrail = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spear");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (projectile.timeLeft == 15)
				projectile.velocity *= 25;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 7; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].velocity = projectile.velocity / 4;
			}
		}
	}
}