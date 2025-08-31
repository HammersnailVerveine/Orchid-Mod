using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Shields
{
	public class DemoniteShieldProjectile : OrchidModGuardianProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		Vector2 Dir;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void AI()
		{
			if (Projectile.timeLeft == 60)
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
				Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
				Dir = Projectile.velocity;
			}

			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 skew = Main.MouseWorld - Projectile.Center;
				float toMouse = MathHelper.WrapAngle(Projectile.velocity.ToRotation() - skew.ToRotation());
				if (Projectile.ai[0] != 0)
				{
					if (Math.Abs(toMouse + Projectile.ai[0]) > Math.Abs(toMouse) || Math.Abs(toMouse) > MathHelper.TwoPi / 3)
					{
						Projectile.ai[0] = 0;
						Projectile.netUpdate = true;
					}
				}
					
				if (Projectile.ai[0] == 0)
				{
					if (Math.Abs(toMouse) < MathHelper.PiOver2 && Math.Abs(toMouse) > 0.1f)
					{
						if (toMouse > 0)
							Projectile.ai[0] = -0.0314f;
						else Projectile.ai[0] = 0.0314f;
						Projectile.netUpdate = true;
					}
				}
			}

			Projectile.rotation += Projectile.ai[0];
			Dir = Dir.RotatedBy(Projectile.ai[0]);

			if (Projectile.timeLeft > 20)
			{
				Projectile.velocity = Dir * (Projectile.timeLeft - 20) / 40;
				if (Projectile.timeLeft == 35) SoundEngine.PlaySound(SoundID.DD2_LightningBugDeath.WithPitchOffset(0.25f).WithVolumeScale(0.3f), Projectile.Center);
			}
			else
			{
				Projectile.velocity = Dir * Projectile.timeLeft / 10;
				Projectile.friendly = Projectile.timeLeft > 5;
				if (Projectile.timeLeft % 2 == 0)
				{
					OldPosition.Add(new Vector2(Projectile.Center.X, Projectile.Center.Y));
					OldRotation.Add(0f + Projectile.rotation);
					if (OldPosition.Count > 4)
					{
						OldPosition.RemoveAt(0);
						OldRotation.RemoveAt(0);
					}
				}
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Demonite, 0f, 0f, 200, default, 1.2f).velocity += Projectile.velocity * 0.3f;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			DrawEater(Projectile.Center, Projectile.rotation, 1f);
			if (OldPosition.Count > 0)
			for (int i = 0; i < OldPosition.Count; i++) DrawEater(OldPosition[i], OldRotation[i], (i + 1f) / (OldPosition.Count + 1f));
			return false;
		}

		void DrawEater(Vector2 pos, float rot, float lum)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
			Color color = new Color(0.1f, 0.1f, 0.1f, 0.05f) * lum * Math.Min(10, Projectile.timeLeft);
			if (Projectile.timeLeft < 45)
			{
				Vector2 eaterOffs = Vector2.UnitX.RotatedBy(rot) * 5;
				int eaterFade = Math.Min(64, 70 - (Projectile.timeLeft - 20) * 3);
				if (eaterFade > 28)
				{
					int mouthFrame = Projectile.timeLeft >= 18
						? (Projectile.timeLeft - 6) / 8 : Projectile.timeLeft >= 10
						? 3 - (Projectile.timeLeft - 10) / 2
						: 3;
					Main.EntitySpriteDraw(texture, pos + eaterOffs - Main.screenPosition, new Rectangle(66 - eaterFade, mouthFrame * 40, eaterFade - 30, 38), color, rot, new Vector2(eaterFade - 30, 19), 1f, SpriteEffects.None);
				}
				eaterFade = Math.Min(28, eaterFade);
				Main.EntitySpriteDraw(texture, pos + eaterOffs - Main.screenPosition, new Rectangle(66 - eaterFade, Projectile.timeLeft / 4 % 3 * 40, eaterFade, 38), color, rot, new Vector2(0, 19), 1f, SpriteEffects.None);
			}
			float eyeRotation = rot;
			if (Projectile.timeLeft > 20) eyeRotation += (float)Math.Pow(Projectile.timeLeft - 15, 3) * Projectile.direction * -0.0002f;
			Main.EntitySpriteDraw(texture, pos - Main.screenPosition, new Rectangle(42, 120, 18, 18), color, eyeRotation, new Vector2(9), 1f, SpriteEffects.None);
		}
	}
}