using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Misc
{
	public class ShapeshifterHarnessProj : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureGlow;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 130;
			Projectile.scale = 0.8f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 2;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureGlow ??= ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void AI()
		{
			if (Projectile.ai[0] > 0)
			{ // slight spawn delay for 2nd and 3rd kunai
				Projectile.ai[0]--;
				Projectile.position -= Projectile.velocity;
				Projectile.timeLeft++;
				return;
			}

			if (!Initialized)
			{ // random texture
				Projectile.friendly = true;
				Initialized = true;
				SoundStyle soundStyle = SoundID.Item65;
				soundStyle.Volume *= 0.33f;
				SoundEngine.PlaySound(soundStyle, Projectile.Center);
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			}

			if (OldPosition.Count > 20 || (Projectile.ai[1] == 1f && OldPosition.Count > 0))
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Projectile.timeLeft <= 90)
			{
				Projectile.velocity *= 0.9f;
			}

			if (Projectile.timeLeft <= 30)
			{
				Projectile.ai[1] = 1f;
				Projectile.friendly = false;
			}

			if (Projectile.ai[1] == 1f)
			{
				if (Projectile.timeLeft > 90)
				{
					Projectile.timeLeft = 90;
				}

				Projectile.velocity *= 0.575f;
			}
			else
			{
				OldPosition.Add(Projectile.Center);
				OldRotation.Add(Projectile.rotation);
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			Projectile.ai[1] = 1f;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[1] = 1f;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			SoundStyle soundStyle = SoundID.Dig;
			soundStyle.Volume *= 0.33f;
			SoundEngine.PlaySound(soundStyle, Projectile.Center);
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (Projectile.ai[0] > 0)
			{ // slight spawn delay for 2nd and 3rd kunai
				return false;
			}

			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 30) colorMult *= Projectile.timeLeft / 30f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureGlow, drawPosition2, null, lightColor * 0.03f * (i + 1) * colorMult, OldRotation[i], TextureGlow.Size() * 0.5f, Projectile.scale * (i + 1) * 0.04f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, null, lightColor * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}