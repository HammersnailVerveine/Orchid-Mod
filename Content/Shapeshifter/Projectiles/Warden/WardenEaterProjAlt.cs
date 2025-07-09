using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenEaterProjAlt : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureOutline;
		private static Texture2D TextureGlow;
		private bool SyncRipe = false;

		public override void SafeSetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 3600;
			Projectile.scale = 0.5f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureGlow ??= ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureOutline ??= ModContent.Request<Texture2D>(Texture + "_Outline", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.netImportant = true;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SyncRipe);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SyncRipe = reader.ReadBoolean();
		}

		public override void AI()
		{
			if (!Initialized)
			{
				Initialized = true;
				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
				SoundEngine.PlaySound(SoundID.DD2_OgreSpit, Projectile.Center);
			}

			/*
			OrchidShapeshifter shapeshifter = Owner.GetModPlayer<OrchidShapeshifter>();
			if (shapeshifter.IsShapeshifted)
			{
				if (shapeshifter.Shapeshift is not WardenEater && Projectile.frame == 0)
				{
					Projectile.ai[0] = 2f;
				}
			}
			else if (Projectile.frame == 0)
			{
				Projectile.ai[0] = 2f;
			}
			*/

			if (Projectile.ai[0] >= 1f)
			{
				if (Projectile.ai[0] == 1f)
				{
					SoundStyle sound = SoundID.Item2;
					sound.Pitch -= 0.2f;
					SoundEngine.PlaySound(sound, Projectile.Center);
				}
				else
				{
					SoundEngine.PlaySound(SoundID.Item50, Projectile.Center);
				}

				for (int i = 0; i < 5; i++)
				{
					Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
				}

				for (int i = 0; i < 8; i++)
				{
					Main.dust[Dust.NewDust(Projectile.position, 18, 18, DustID.RichMahogany)].velocity.Y -= 1.25f;
				}

				Projectile.Kill();
			}

			if (Projectile.ai[2] > 0 && Projectile.frame == 0)
			{
				Projectile.timeLeft -= 300 * (int)Projectile.ai[2];
				Projectile.ai[2] = 0f;

				if (Projectile.timeLeft < 1800)
				{
					Projectile.timeLeft = 1800;
				}
			}

			if (Projectile.scale < 1f)
			{
				Projectile.scale *= 1.025f;
				if (Projectile.scale > 1f)
				{
					 Projectile.scale = 1f;
				}
			}
			
			if (Projectile.ai[1] <= 0f)
			{
				foreach (Projectile projectile in Main.projectile)
				{
					if (projectile.type == Projectile.type && projectile.Hitbox.Intersects(Projectile.Hitbox) && projectile.whoAmI != Projectile.whoAmI && projectile.frame == 0)
					{
						SoundEngine.PlaySound(SoundID.Item50, Projectile.Center);
						Vector2 velocity = Vector2.Normalize(projectile.Center - Projectile.Center) * 0.5f;
						projectile.velocity = velocity;
						Projectile.velocity = -velocity;
						Projectile.ai[1] = 10f;
					}
				}
			}
			else
			{
				Projectile.ai[1]--;
			}

			Projectile.rotation += Projectile.velocity.Length() / 30.5f * (Projectile.velocity.X > 0 ? 1f : -1f) + (float)Math.Sin(Projectile.timeLeft * 0.025f) * 0.0065f;
			Projectile.velocity *= 0.975f;
			Projectile.friendly = Projectile.velocity.Length() > 1f;

			if (Projectile.frame == 1)
			{
				Lighting.AddLight(Projectile.position, 0.23f, 0.33f, 0f);
			}

			if ((Projectile.timeLeft <= 1800 || SyncRipe) && Projectile.frame == 0)
			{
				Projectile.timeLeft = 1800;
				Projectile.frame = 1;
				Projectile.velocity *= 0f;
				SyncRipe = true;
				Projectile.netUpdate = true;
				SoundEngine.PlaySound(SoundID.Item50, Projectile.Center);

				for (int i = 0; i < 5; i++)
				{
					Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
				}

				for (int i = 0; i < 8; i++)
				{
					Main.dust[Dust.NewDust(Projectile.position, 18, 18, DustID.RichMahogany)].velocity.Y -= 1.25f;
				}
			}

			if (Main.rand.NextBool(60))
			{
				if (Projectile.frame == 0)
				{
					Main.dust[Dust.NewDust(Projectile.position, 18, 18, DustID.RichMahogany)].velocity *= 0.1f;
				}
				else
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4f, 4f), 8, 8, DustID.CursedTorch);
					dust.velocity *= 0.2f;
					dust.scale = Main.rand.NextFloat(0.6f, 0.8f);
					dust.noLightEmittence = true;
				}
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			Vector2 vel = Projectile.Center - target.Center;
			vel.Normalize();
			vel *= (Projectile.velocity.Length() * 0.75f);
			Projectile.velocity = vel;
			SoundEngine.PlaySound(SoundID.Item50, Projectile.Center);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X / 2;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y / 2;
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			Rectangle drawRectangle = TextureMain.Bounds;
			drawRectangle.Height /= 2;
			drawRectangle.Y += drawRectangle.Height * Projectile.frame;

			float colorMult2 = 1f;
			if (Projectile.timeLeft < 20) colorMult2 *= Projectile.timeLeft / 20f;

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, drawRectangle, lightColor * colorMult2, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

			if (Projectile.frame == 1)
			{
				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				spriteBatch.Draw(TextureGlow, drawPosition, null, Color.White * 0.65f * colorMult2, Projectile.rotation, TextureGlow.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

				float colorMult = 1.5f * ((float)Math.Sin(Projectile.timeLeft * 0.034f) * 0.15f + 0.3f);
				spriteBatch.Draw(TextureOutline, drawPosition, null, Color.White * 0.5f * colorMult * colorMult2, Projectile.rotation, TextureOutline.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}

			return false;
		}
	}
}