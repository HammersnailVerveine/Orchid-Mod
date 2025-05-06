using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageImpProj : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public int Timespent = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 120;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = 2;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.penetrate < 1) return false;
			return base.CanHitNPC(target);
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			if (Projectile.penetrate == 1)
			{
				Projectile.penetrate = -1;

				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Scale: Main.rand.NextFloat(1.2f, 1.5f));
					dust.noLight = true;
					dust.noGravity = true;
					dust.scale = Main.rand.NextFloat(1f, 1.5f);
					dust.velocity *= Main.rand.NextFloat(1.5f, 4f);
				}

				Projectile.timeLeft = 10;
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Scale: Main.rand.NextFloat(1.2f, 1.5f));
					dust.noLight = true;
					dust.noGravity = true;
					dust.scale = Main.rand.NextFloat(1f, 1.5f);
					dust.velocity *= Main.rand.NextFloat(1f, 3f);
				}
			}

			target.AddBuff(BuffID.OnFire, 300);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

			for (int i = 0; i < 15; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Scale: Main.rand.NextFloat(1.2f, 1.5f));
				dust.noLight = true;
				dust.noGravity = true;
				dust.scale = Main.rand.NextFloat(1f, 1.5f);
				dust.velocity *= Main.rand.NextFloat(1.5f, 4f);
			}
			return true;
		}

		public override void AI()
		{
			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);
			Timespent++;

			if (!Initialized)
			{
				Initialized = true;

				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
					dust.scale = Main.rand.NextFloat(1.5f, 2f);
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(5f, 8f);
				}

				for (int i = 0; i < 5; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
					dust.scale = Main.rand.NextFloat(1.5f, 2f);
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(20f)) * Main.rand.NextFloat(10f, 15f);
				}
			}

			if (Timespent < 30 && Timespent > 10)
			{
				Projectile.velocity *= 1.1f;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (OldPosition.Count > 10)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Projectile.penetrate < 1) Projectile.velocity *= 0.75f;

			if (Main.rand.NextBool(4 - (int)Projectile.ai[1]))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
				dust.velocity *= 0.25f;
				dust.velocity.Y -= 1f;
				dust.velocity += Projectile.velocity * 0.2f;
				dust.noLight = true;
			}

			if (Main.rand.NextBool(2))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
				dust.velocity *= 0.25f;
				dust.velocity += Projectile.velocity * 0.3f;
				dust.scale = Main.rand.NextFloat(1f, 1.5f);
				dust.noGravity = true;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			Color drawColor = new Color(255, 102, 20);
			float colorMult = 1f;
			float scaleMult = 1.1f + (float)Math.Sin(Timespent * 0.15f) * 0.25f;
			if (Projectile.timeLeft < 7) colorMult *= Projectile.timeLeft / 7f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, null, drawColor * 0.15f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.08f * scaleMult, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			return false;
		}
	}
}