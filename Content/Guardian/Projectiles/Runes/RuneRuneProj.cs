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
	public class RuneRuneProj : GuardianRuneProjectile
	{
		public int TimeSpent = 0;
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

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
		}

		public override void FirstFrame()
		{
			if (Projectile.ai[2] != 0) TimeSpent += 60;
		}

		public override bool SafeAI()
		{
			TimeSpent++;
			SetDistance(170 + (float)Math.Sin(TimeSpent * (MathHelper.Pi / 120f)) * 80f);

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 pos = OldPosition[i];
				pos.Y -= 4f;
				OldPosition[i] = pos;
			}

			OldPosition.Add(Projectile.Center + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, 3f)));
			OldRotation.Add(Projectile.rotation + Main.rand.NextFloat(MathHelper.Pi));

			if (OldPosition.Count > 13)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(5))
			{
				Dust dust =  Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RuneWizard, Scale: Main.rand.NextFloat(0.4f, 0.6f));
				dust.velocity.Y -= 4f;
			}
			return true;
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			target.AddBuff(BuffID.OnFire, 600);
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 60) colorMult *= Projectile.timeLeft / 60f;
			Color color = new Color(56, 255, 115); // to 255 127 39

			for (int i = 0; i < OldPosition.Count; i++)
			{
				if (i > 3)
				{
					color.R += 20;
					color.G -= 13;
					color.B -= 8;
				}

				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.075f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.09f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}