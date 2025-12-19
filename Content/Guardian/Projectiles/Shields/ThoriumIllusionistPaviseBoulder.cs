using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Projectiles.Shields
{
	public class ThoriumIllusionistPaviseBoulder : OrchidModGuardianProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.aiStyle = ProjAIStyleID.Boulder;
			Projectile.timeLeft = 180;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 15f)
			{
				Projectile.ai[0] = 15f;
				Projectile.velocity.Y += 0.1f;
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}

			if (Main.rand.NextBool())
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
				dust.noGravity = true;
				dust.scale *= 2f;
				dust.velocity *= 0.5f;
			}

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