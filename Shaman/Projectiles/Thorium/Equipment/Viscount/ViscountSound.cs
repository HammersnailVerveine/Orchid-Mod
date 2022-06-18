using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.Equipment.Viscount
{
	public class ViscountSound : OrchidModShamanProjectile
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
			DisplayName.SetDefault("Viscount Sound");
		}

		public override void AI()
		{
			int DustID = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 1, 1, 89, 0f, 0f, 125, default(Color), 1.25f);
			Main.dust[DustID].noGravity = true;
			Main.dust[DustID].velocity = Projectile.velocity / 5;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("ViscountOrbSound").Type, 0, 0.0f, Projectile.owner, 0.0f, 0.0f);
		}
	}
}