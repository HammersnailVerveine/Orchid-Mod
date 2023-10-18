using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class IceSpearProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.height = 40;
			Projectile.width = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 15;
			Projectile.penetrate = 3;
			Projectile.scale = 1.25f;
			this.projectileTrail = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Spear");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (Projectile.timeLeft == 15)
				Projectile.velocity *= 25;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 7; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].velocity = Projectile.velocity / 4;
			}
		}
	}
}