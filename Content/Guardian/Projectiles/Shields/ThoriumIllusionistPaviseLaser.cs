using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Projectiles.Shields
{
	public class ThoriumIllusionistPaviseLaser : OrchidModGuardianProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 360;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 10;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (!Initialized)
			{ // dust on spawn
				Initialized = true;
				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity * 8f, Projectile.width, Projectile.height, DustID.BlueTorch);
					dust.noGravity = true;
					dust.velocity *= 2f;
					dust.scale *= 1.5f;
				}

				SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact.WithPitchOffset(Main.rand.NextFloat(0.5f, 0.75f)), Projectile.Center);
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
				dust.noGravity = true;
				dust.velocity *= 2f;
				dust.scale *= 1.5f;
			}
		}
	}
}