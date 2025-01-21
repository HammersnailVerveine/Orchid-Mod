using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using OrchidMod.Content.Shaman.Projectiles;
using Terraria.Audio;
using OrchidMod.Utilities;
using OrchidMod.Common.ModObjects;

namespace OrchidMod.Content.Shaman.Projectiles.Water
{
	public class AvalancheProjectile : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 25;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.alpha = 255;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void SafeAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 10)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(6))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FrostDaggerfish, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.velocity = dust.velocity * 0.25f + Projectile.velocity * 0.2f;
				dust.noGravity = true;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 8) colorMult *= Projectile.timeLeft / 8f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				Color color = new Color(100, 200, 255);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.1f * i * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * i * 0.175f, SpriteEffects.None, 0f); ;
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.05f * i * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * i * 0.12f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
	public class AvalancheProjectileAlt : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 40;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.alpha = 255;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.penetrate = -1;
		}

		public override void SafeAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 15)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FrostDaggerfish, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.velocity = dust.velocity * 0.25f + Projectile.velocity * 0.2f;
				dust.noGravity = true;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 15) colorMult *= Projectile.timeLeft / 8f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				Color color = new Color(100, 200, 255);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.1f * i * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * i * 0.12f, SpriteEffects.None, 0f); ;
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.05f * i * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * i * 0.1f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}

