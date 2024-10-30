using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenSpiderWeb : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1845;
			Projectile.scale = 0.25f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void AI()
		{
			Projectile.ai[0]++;
			if (Projectile.ai[0] > 50f)
			{
				Projectile.friendly = false;
				Projectile.velocity *= 0.8f;
				Projectile.rotation += 0.005f + (float)Math.Sin(Projectile.ai[0] * 0.025f) * 0.01f;
			}
			else if (Projectile.ai[0] > 30 && Projectile.ai[1] == 0 && Projectile.ai[2] == 0)
			{
				Projectile.tileCollide = false;
				Projectile.velocity *= 0.85f;
				if (Projectile.scale < 1f) Projectile.scale *= 1.1f;
				if (Projectile.scale > 1.4f) Projectile.scale *= 1f; // Needs 15 frames from 0.25f;
				Projectile.rotation += 0.05f;

				if (OldPosition.Count > 0)
				{
					OldPosition.RemoveAt(0);
					OldRotation.RemoveAt(0);
				}

				if (!initialized)
				{ // Kills all other webs
					initialized = true;
					foreach (Projectile projectile in Main.projectile)
					{
						if (projectile.type == Type && projectile != Projectile && projectile.active && Projectile.owner == projectile.owner)
						{
							projectile.ai[1] = 1;
							projectile.netUpdate = true;
						}
					}
				}
			}
			else
			{
				Projectile.rotation += 0.2f;

				foreach (Projectile projectile in Main.projectile)
				{
					if (projectile.type == Type && projectile != Projectile && projectile.active && Projectile.owner == projectile.owner)
					{
						float distance = projectile.Center.Distance(Projectile.Center);
						if (Owner.Center.Distance(projectile.Center) > 64f)
						{
							if (distance < 48f)
							{
								Projectile.ai[2] = 1;
								projectile.netUpdate = true;
								SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
								break;
							}
							else if (distance < 128f)
							{
								Projectile.ai[0]--;
							}
						}
					}
				}

				OldPosition.Add(Projectile.Center);
				OldRotation.Add(Projectile.rotation);

				if (OldPosition.Count > 10)
				{
					OldPosition.RemoveAt(0);
					OldRotation.RemoveAt(0);
				}
			}

			if (Projectile.ai[1] > 0 && Projectile.timeLeft > 15f)
			{
				Projectile.friendly = false;
				Projectile.tileCollide = false;
				Projectile.timeLeft = 15;
			}

			if (Projectile.ai[2] > 0)
			{
				Projectile.friendly = false;
				Projectile.tileCollide = false;
				if (Projectile.timeLeft > 20)
				{
					Projectile.timeLeft = 20;
				}

				OrchidShapeshifter shapeshifter = Owner.GetModPlayer<OrchidShapeshifter>();
				shapeshifter.ShapeshiftAnchor.Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 12.5f;
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			Projectile.friendly = false;
			Projectile.ai[0] = 30;
			Projectile.netUpdate = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[2] = 1;
			Projectile.netUpdate = true;
			SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			if (Projectile.ai[2] > 0) return false;

			float colorMult = 1f;
			if (Projectile.timeLeft < 15) colorMult *= Projectile.timeLeft / 15f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, null, lightColor * 0.05f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.08f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);


			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, null, lightColor * 0.9f * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}