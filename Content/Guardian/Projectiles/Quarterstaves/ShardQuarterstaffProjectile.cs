using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;

namespace OrchidMod.Content.Guardian.Projectiles.Quarterstaves
{
	public class ShardQuarterstaffProjectile : OrchidModGuardianProjectile
	{
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 90;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void AI()
		{
			if (!Initialized)
			{
				Initialized = true;
				Projectile.rotation = Projectile.ai[1];
				if (Projectile.velocity.Length() >= 8)
				{
					Projectile.ai[2] = Projectile.velocity.Length() / 8 - 1;
				}
			}

			//Projectile.velocity *= 0.99f;
			Projectile.rotation += Projectile.ai[0];
			if (Projectile.ai[0] > 0.2f)
			{
				Projectile.ai[0] *= 0.97f;
				if (Projectile.ai[0] < 0.2f)
				{
					Projectile.ai[0] = 0.2f;
				}
			}

			if (Projectile.ai[2] > 0 && Projectile.velocity != Vector2.Zero)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity * Projectile.ai[2] / Projectile.velocity.Length(), Projectile.velocity, 0.9f);
			}
			else Projectile.velocity *= 0.9f;

			if (Projectile.timeLeft < 30) Projectile.scale = (float)Math.Sin(Projectile.timeLeft * 0.05f);

			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldRotation.Count > 10)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
		//	spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
		//  spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			Texture2D projTexture = TextureAssets.Projectile[Projectile.type].Value;
			float colorMult = 1f;
			if (Projectile.timeLeft < 30f) colorMult *= Projectile.timeLeft / 30f;

			for (int i = 0; i < OldRotation.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(projTexture, drawPosition, null, Color.White * 0.05f * (i + 1) * colorMult, OldRotation[i], projTexture.Size() * 0.5f, Projectile.scale * 1.25f, SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(projTexture, Projectile.Center - Main.screenPosition, null, Color.White * colorMult * 0.5f, Projectile.rotation, projTexture.Size() * 0.5f, Projectile.scale * 1.25f, SpriteEffects.None, 0f);

			// Draw code ends here

			//spriteBatch.End();
			//spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}