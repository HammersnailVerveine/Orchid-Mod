using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;

namespace OrchidMod.Content.Guardian.Projectiles.Quarterstaves
{
	public class ShardQuarterstaffProjectile : OrchidModGuardianProjectile
	{
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
			OldRotation = new List<float>();
		}

		public override void AI()
		{
			if (!initialized)
			{
				initialized = true;
				Projectile.rotation = Projectile.ai[1];
			}

			Projectile.rotation += Projectile.ai[0];
			if (Projectile.ai[0] > 0.25f)
			{
				Projectile.ai[0] *= 0.95f;
				if (Projectile.ai[0] < 0.25f)
				{
					Projectile.ai[0] = 0.25f;
				}
			}

			OldRotation.Add(Projectile.rotation);

			if (OldRotation.Count > 10)
			{
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
			if (Projectile.timeLeft < 20f) colorMult *= Projectile.timeLeft / 20f;

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;

			for (int i = 0; i < OldRotation.Count; i++)
			{
				spriteBatch.Draw(projTexture, drawPosition, null, Color.White * 0.05f * (i + 1) * colorMult, OldRotation[i], projTexture.Size() * 0.5f, Projectile.scale * 1.25f, SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(projTexture, drawPosition, null, Color.White * colorMult * 0.5f, Projectile.rotation, projTexture.Size() * 0.5f, Projectile.scale * 1.25f, SpriteEffects.None, 0f);

			// Draw code ends here

			//spriteBatch.End();
			//spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}