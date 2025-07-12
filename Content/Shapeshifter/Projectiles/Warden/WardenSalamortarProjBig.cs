using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenSalamortarProjBig : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureGlow;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public int TimeSpent = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 100;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureGlow ??= ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			TimeSpent = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 180;
		}

		public override void OnKill(int timeLeft)
		{
			SoundStyle soundStyle = SoundID.DD2_ExplosiveTrapExplode;
			soundStyle.Volume = 1.1f;
			SoundEngine.PlaySound(soundStyle, Projectile.Center);

			for (int i = 0; i < 15 - (3 - Projectile.ai[2]) * 2; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
				dust.scale = Main.rand.NextFloat(0.6f, 0.8f);
				dust.velocity *= Main.rand.NextFloat(0.25f, 0.75f);
			}

			for (int i = 0; i < 15 - (3 - Projectile.ai[2]) * 2; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
				dust.noGravity = true;
				dust.noLight = true;
				dust.scale = Main.rand.NextFloat(0.9f, 1.3f);
				dust.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3f, 8f);
			}

			for (int i = 0; i < 10 - (3 - Projectile.ai[2]) * 2; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
				dust.noGravity = true;
				dust.noLight = true;
				dust.scale = Main.rand.NextFloat(1f, 1.4f);
				dust.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(2f, 13f);
			}

			for (int i = 0; i < 15 - (3 - Projectile.ai[2]) * 2; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
				dust.noGravity = true;
				dust.noLight = true;
				dust.scale = Main.rand.NextFloat(0.8f, 1.2f);
				dust.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(5f, 11f);
			}

			for (int i = 0; i < 3 + (Projectile.ai[2] - 1) * 2; i++)
			{
				Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 61 + Main.rand.Next(3));
				gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
			}

			int projType = ModContent.ProjectileType<WardenSalamortarProjBigAlt>();
			ShapeshifterNewProjectile(Projectile.Center, Projectile.velocity * 0.01f, projType, Projectile.damage * 10f, Projectile.CritChance, 8f, Projectile.owner);

			for (int i = 0; i < (Projectile.ai[2] - 1) * 2 * 5; i ++)
			{
				int projectileType = ModContent.ProjectileType<WardenSalamortarProj>();
				Vector2 velocity = new Vector2(Main.rand.NextFloat(-2.66f, 2.66f), -15f + Main.rand.NextFloat(-2.5f, 2.5f));
				OrchidShapeshifter shapeshifter = Owner.GetModPlayer<OrchidShapeshifter>();
				ShapeshifterNewProjectile(Projectile.Center, velocity, projectileType, Projectile.damage * 0.3f, Projectile.CritChance, Projectile.knockBack * 0.5f, Projectile.owner, ai1: Main.rand.NextFloat(-6f, 6f), ai2: 1f);
			}
		}

		public override void AI()
		{
			TimeSpent++;
			Projectile.rotation += Projectile.velocity.Length() / 30.5f * (Projectile.velocity.X > 0 ? 1f : -1f);

			Projectile.tileCollide = TimeSpent > 50;

			if (Projectile.timeLeft > 45 - Projectile.ai[1])
			{
				OldPosition.Add(Projectile.Center);
				OldRotation.Add(Projectile.rotation);

				if (Projectile.timeLeft > 45)
				{
					Projectile.velocity.Y += 0.66f;
				}
			}
			else
			{ // vibrating before exploding
				Projectile.velocity *= 0.85f;
				Projectile.scale += Main.rand.NextFloat(-0.01f, 0.02f);
				Projectile.rotation += Main.rand.NextFloat(-0.1f, 0.1f);
				Projectile.position.X += Main.rand.NextFloat(-0.25f, 0.25f);
				Projectile.position.Y += Main.rand.NextFloat(-0.25f, 0.25f);
			}

			if (Main.rand.NextBool(4 - ((int)Projectile.ai[2] - 1)))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
				dust.scale = Main.rand.NextFloat(0.8f, 1.2f);
				dust.noGravity = true;
				dust.noLight = true;
				dust.velocity *= 0.35f;
			}

			if (!Initialized)
			{
				Initialized = true;
				SoundStyle soundStyle = SoundID.DD2_FlameburstTowerShot;
				soundStyle.Volume = 0.2f;
				SoundEngine.PlaySound(soundStyle, Projectile.Center);
				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
				Projectile.ai[0] = Main.rand.Next(2) + (Projectile.ai[2] - 1) * 3;

				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
					dust.scale = Main.rand.NextFloat(0.9f, 1.3f);
					dust.noGravity = true;
					dust.velocity *= 0.4f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(5f, 8f);
				}

				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Smoke);
					dust.scale = Main.rand.NextFloat(0.6f, 0.8f);
					dust.noGravity = true;
					dust.velocity *= 0.4f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(35f)) * Main.rand.NextFloat(5f, 8f);
				}

				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
					dust.scale = Main.rand.NextFloat(0.9f, 1.3f);
					dust.noGravity = true;
					dust.velocity *= 0.4f;
					dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(20f)) * Main.rand.NextFloat(9f, 12f);
				}
			}

			if (OldPosition.Count > 15 || (Projectile.timeLeft > 45 - Projectile.ai[1] && OldPosition.Count > 0))
			{
				OldPosition.RemoveAt(0);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			Rectangle drawRectangle = TextureMain.Bounds;
			drawRectangle.Height /= 9;
			drawRectangle.Y += drawRectangle.Height * (int)Projectile.ai[0];

			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, drawRectangle, lightColor * 0.075f * (i + 1), OldRotation[i], drawRectangle.Size() * 0.5f, Projectile.scale * (i + 1) * 0.065f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, drawRectangle, lightColor, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			spriteBatch.Draw(TextureGlow, drawPosition, drawRectangle, Color.White, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}