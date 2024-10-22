using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Runes
{
	public class EmpressRuneProj : GuardianRuneProjectile
	{
		public int TimeSpent = 0;
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public float ToSpin = 0;

		public override void RuneSetDefaults()
		{
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

		public override void FirstFrame()
		{
			if (Projectile.ai[2] != 0) TimeSpent += 60;
		}

		public override bool SafeAI()
		{
			TimeSpent++;
			//SetDistance(170 + (float)Math.Sin(TimeSpent * (MathHelper.Pi / 120f)) * 80f);
			Projectile.rotation = Main.player[Projectile.owner].velocity.ToRotation();

			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 15)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (TimeSpent % 120 == 0) ToSpin = 60f + Main.rand.NextFloat(300) * (Main.rand.NextBool() ? 1f : -1f);

			Spin(ToSpin * 0.025f);
			ToSpin *= 0.985f;

			return true;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f + (float)Math.Sin(TimeSpent * (MathHelper.Pi / 120f)) * 0.15f;
			if (Projectile.timeLeft < 60) colorMult *= Projectile.timeLeft / 60f;
			Color color = new Color(247, 119, 224); // to 40 105 240

			for (int i = 0; i < OldPosition.Count; i++)
			{
				if (i > 4)
				{
					color.R += 18;
					color.G += 1;
					color.B += 1;
				}

				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.066f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.08f * colorMult, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}