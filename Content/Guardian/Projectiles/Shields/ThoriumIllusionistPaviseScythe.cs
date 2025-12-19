using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Projectiles.Shields
{
	public class ThoriumIllusionistPaviseScythe : OrchidModGuardianProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 180;
			Projectile.scale = 1f;
			Projectile.penetrate = 5;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 30f && Projectile.ai[0] < 100f)
			{
				Projectile.velocity *= 1.06f;
			}

			Projectile.rotation += MathHelper.ToRadians(45f);

			Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
			dust2.noGravity = true;
			dust2.scale *= 2f;
			dust2.velocity *= 0.5f;

			if (!Initialized)
			{ // dust on spawn
				Initialized = true;
				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
					dust.noGravity = true;
					dust.velocity *= 1.5f;
					dust.scale *= 2f;
				}

				SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact.WithPitchOffset(Main.rand.NextFloat(0.5f, 0.75f)), Projectile.Center);
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale *= 2f;
			}
		}
	}
}