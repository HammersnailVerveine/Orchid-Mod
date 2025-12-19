using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Projectiles.Shields
{
	public class ThoriumIllusionistPaviseMagic : OrchidModGuardianProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 360;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();


			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
			dust.noGravity = true;
			dust.scale *= 3f;
			dust.velocity *= 0.5f;

			if (!Initialized)
			{ // dust on spawn
				Initialized = true;
				for (int i = 0; i < 12; i++)
				{
					Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
					dust2.noGravity = true;
					dust2.velocity *= 1.5f;
					dust2.scale *= 2f;
				}

				SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact.WithPitchOffset(Main.rand.NextFloat(0.5f, 0.75f)), Projectile.Center);
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			for (int i = 0; i < 12; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale *= 2f;
			}
		}
	}
}