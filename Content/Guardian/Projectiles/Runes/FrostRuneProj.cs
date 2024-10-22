using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Runes
{
	public class FrostRuneProj : GuardianRuneProjectile
	{
		public int TimeSpent = 0;
		float rotation = 0f;
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void RuneSetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Main.projFrames[Projectile.type] = 4;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void FirstFrame()
		{
			rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			Projectile.frame = Main.rand.Next(Main.projFrames[Projectile.type]);
		}

		public override bool SafeAI()
		{
			Spin(1.5f);
			TimeSpent++;
			SetDistance(150 + (float)Math.Sin(TimeSpent * (MathHelper.Pi / 180f)) * 40f);
			Projectile.rotation = rotation + (float)Math.Sin(TimeSpent * (MathHelper.Pi / 60f)) * 0.4f;

			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 5)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(10))
			{
				int type;
				switch(Projectile.frame)
				{
					case 1:
						type = DustID.RedTorch;
						break;
					case 2:
						type = DustID.GreenTorch;
						break;
					case 3:
						type = DustID.BlueTorch;
						break;
					default:
						type = DustID.YellowTorch;
						break;
				}
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, type, Scale: Main.rand.NextFloat(1f, 1.2f));
				dust.velocity *= 0.5f;
				dust.noGravity = true;
			}

			return true;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 60) colorMult *= Projectile.timeLeft / 60f;

			Rectangle rect = TextureMain.Bounds;
			rect.Height /= Main.projFrames[Projectile.type];
			rect.Y += rect.Height * Projectile.frame;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, rect, Color.White * 0.1f * (i + 1) * colorMult, OldRotation[i], rect.Size() * 0.5f, Projectile.scale * (i + 1) * 0.2f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			Vector2 drawPosition2 = Vector2.Transform(Projectile.Center - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
			spriteBatch.Draw(TextureMain, drawPosition2, rect, Color.White * colorMult, Projectile.rotation, rect.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}