using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;

namespace OrchidMod.Content.Guardian.Projectiles.Quarterstaves
{
	public class ThoriumAquaiteQuarterstaffProjectile : OrchidModGuardianProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_27";

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.timeLeft = 50;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 10)
			{
				Projectile.friendly = true;
				Projectile.velocity = Vector2.Zero;
				Projectile.extraUpdates = 0;
				if (Projectile.tileCollide)
				{
					Projectile.tileCollide = false;
					Strong = true;
				}
				else
				{
					Projectile.damage = (int)(Projectile.damage * 0.4f);
				}
				SoundEngine.PlaySound(SoundID.Item21.WithPitchOffset(Strong ? -0.2f : 0.4f), Projectile.Center);
				float dustRot = Main.rand.NextFloat(MathHelper.TwoPi);
				bool ccw = Main.rand.NextBool();
				for (int i = Strong ? 100 : 60; i > 0; i--)
				{
					Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.BlueFairy);
					dust.velocity = Vector2.UnitX.RotatedBy(dustRot) * (i * 0.12f);
					dust.scale *= (Strong ? 2f : 1.5f) - i * 0.015f;
					dust.noGravity = true;
					if (ccw) dustRot -= 0.2f - i * 0.0015f;
					else dustRot += 0.2f - i * 0.0015f;
					dust.alpha = 127;
				}
				for (int i = Strong ? 30 : 15; i > 0; i--)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.BlueTorch);
					dust.scale *= 0.5f + Main.rand.NextFloat(Strong ? 0.8f : 0.4f);
					dust.velocity.Y *= 2.5f;
					dust.velocity.X *= 4f;
				}
			}
			else if (Projectile.timeLeft > 10)
			{
				Projectile.velocity *= 0.9f;
				Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.BlueFairy);
				dust.velocity = Vector2.UnitY.RotatedBy(Projectile.velocity.ToRotation()) * 0.4f * dust.velocity.Length() * (float)Math.Sin(Projectile.timeLeft / 2f);
				dust.noGravity = true;
				dust.alpha = 127;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.timeLeft = 11;
			Projectile.tileCollide = false;
			return false;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int size = Strong ? 50 : 20;
			hitbox.X -= size;
			hitbox.Y -= size;
			hitbox.Width += size * 2;
			hitbox.Height += size * 2;
		}
	}
}