using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Runes
{
	public class GoblinRuneProj : GuardianRuneProjectile
	{
		public int TimeSpent = 0;
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public float ToSpin = 0;
		public float TargetDistance = 0;

		public override void RuneSetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			TargetDistance = 100;
		}

		public override bool SafeAI()
		{
			TimeSpent++;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 pos = OldPosition[i];
				pos.Y -= Main.rand.NextFloat(4f);
				pos.X += Main.rand.NextFloat(4f) - 2f;
				OldPosition[i] = pos;
			}

			if (Main.rand.NextBool(90))
			{
				ToSpin = 30f + Main.rand.NextFloat(50) * (Main.rand.NextBool() ? 1f : -1f);
				TargetDistance = Main.rand.NextFloat(100f) + 50f;
			}

			Spin(ToSpin * 0.15f);
			ToSpin *= 0.95f;
			SetDistance(Distance + (TargetDistance - Distance) * 0.05f);

			OldPosition.Add(Projectile.Center + new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-8f, 8f)));
			OldRotation.Add(Projectile.rotation + Main.rand.NextFloat(MathHelper.Pi));

			if (OldPosition.Count > 10)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(10)) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Scale: Main.rand.NextFloat(0.8f, 1f));
			return true;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 60) colorMult *= Projectile.timeLeft / 60f;
			Color color = new Color(45, 1, 238);

			for (int i = 0; i < OldPosition.Count; i++)
			{
				color.R += 5;
				color.G += 5;
				color.B += 1;
				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.096f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.1f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}