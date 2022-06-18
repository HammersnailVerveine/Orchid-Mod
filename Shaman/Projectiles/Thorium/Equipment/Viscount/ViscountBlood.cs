using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.Equipment.Viscount
{
	public class ViscountBlood : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.aiStyle = 2;
			Projectile.timeLeft = 50;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Viscount Blood");
		}

		public override void AI()
		{
			int DustID = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 1, 1, 182, 0f, 0f, 125, default(Color), 1.25f);
			Main.dust[DustID].noGravity = true;
			Main.dust[DustID].velocity = Projectile.velocity / 5;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("ViscountOrbBlood").Type, 10, 0.0f, Projectile.owner, 0.0f, 0.0f);
		}
	}
}