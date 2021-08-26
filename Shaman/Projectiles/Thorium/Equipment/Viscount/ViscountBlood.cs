using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium.Equipment.Viscount
{
	public class ViscountBlood : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.friendly = true;
			projectile.aiStyle = 2;
			projectile.timeLeft = 50;
			projectile.scale = 1f;
			projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Viscount Blood");
		}

		public override void AI()
		{
			int DustID = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 1, 1, 182, 0f, 0f, 125, default(Color), 1.25f);
			Main.dust[DustID].noGravity = true;
			Main.dust[DustID].velocity = projectile.velocity / 5;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("ViscountOrbBlood"), 10, 0.0f, projectile.owner, 0.0f, 0.0f);
		}
	}
}