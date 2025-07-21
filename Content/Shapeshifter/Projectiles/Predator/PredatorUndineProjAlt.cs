using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Weapons.Predator;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Predator
{
	public class PredatorUndineProjAlt : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureGlow;
		private static Texture2D TextureGlow2;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public Color DrawColor;

		public override void SafeSetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 300;
			Projectile.scale = 0.6f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 3;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureGlow ??= ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureGlow2 ??= ModContent.Request<Texture2D>(Texture + "_Glow2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			DrawColor = new Color(132, 234, 255);
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (target.whoAmI != (int)Projectile.ai[2] || Projectile.ai[1] == 1f) return false;
			return base.CanHitNPC(target);
		}

		public override void AI()
		{
			if (Projectile.timeLeft < 260)
			{
				Projectile.localAI[0]++;
			}

			if (Projectile.ai[0] > 0)
			{
				Projectile.ai[0]--;
				Projectile.position = Owner.Center;
				Projectile.timeLeft++;
				return;
			}

			if (!Initialized)
			{ // random texture
				Initialized = true;
				SoundStyle soundStyle = SoundID.Item21;
				soundStyle.Volume *= 0.33f;
				SoundEngine.PlaySound(soundStyle, Projectile.Center);
				Projectile.ai[1] = Main.rand.NextFloat(MathHelper.TwoPi);

				byte colorDiff = (byte)(Main.rand.Next(3) * 50);
				DrawColor.R -= colorDiff;
				DrawColor.G -= (byte)(colorDiff * 2);
			}

			if (Projectile.timeLeft <= 30)
			{
				Projectile.ai[1] = 1f;
				Projectile.friendly = false;
			}
			else if (Projectile.timeLeft <= 290)
			{
				Projectile.friendly = true;
			}

			if (Projectile.ai[1] == 1f)
			{
				if (Projectile.timeLeft > 30)
				{
					Projectile.timeLeft = 30;
				}

				Projectile.velocity *= 0.5f;
			}
			else
			{
				NPC target = Main.npc[(int)Projectile.ai[2]];

				if (IsValidTarget(target) && Projectile.timeLeft > 30)
				{
					Projectile.velocity += (target.Center - Projectile.Center) * (0.003f + Projectile.localAI[0] * 0.0001f);
					Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 3.8f;
				}
				else if (Projectile.timeLeft > 30)
				{
					Projectile.ai[1] = 1f;
					Projectile.netUpdate = true;
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);
			if (OldPosition.Count > 30)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			Projectile.ai[1] = 1f;
			Projectile.friendly = false;
			Projectile.tileCollide = false;

			if (shapeshifter.IsShapeshifted && hit.Crit)
			{
				if (shapeshifter.Shapeshift is PredatorUndine undine)
				{
					if (shapeshifter.ShapeshiftAnchor.Projectile.ai[1] < 10)
					{
						shapeshifter.ShapeshiftAnchor.Projectile.ai[1] ++;
					}

					shapeshifter.ShapeshiftAnchor.Projectile.ai[2] = 180;
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (Projectile.ai[0] > 0)
			{
				return false;
			}

			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 20) colorMult *= Projectile.timeLeft / 20f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureGlow, drawPosition2, null, DrawColor * 0.03f * (i + 1) * colorMult, OldRotation[i], TextureGlow.Size() * 0.5f, Projectile.scale * 0.8f, SpriteEffects.None, 0f);
			}

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureGlow2, drawPosition, null, DrawColor * colorMult * 0.6f, Projectile.ai[1], TextureGlow2.Size() * 0.5f, Projectile.scale * 1.5f, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureMain, drawPosition, null, Color.White * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			return false;
		}
	}
}