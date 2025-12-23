using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Core.Utils;
using OrchidMod.Content.Guardian.Projectiles.Quarterstaves;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Projectiles.Gauntlets
{
	public class GlowingMushroomGauntletProjectile : OrchidModGuardianProjectile
	{
		public List<GlowingMushroomProjectileSpore> Spores;
		public int TimeSpent = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox.X -= 10; // 20*20 -> 60*60
			hitbox.Y -= 10;
			hitbox.Width += Projectile.width * 3;
			hitbox.Height += Projectile.height * 3;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.timeLeft < 30 || !Collision.CanHitLine(Projectile.Center, 2, 2, target.position, target.width, target.height)) return false;
			return base.CanHitNPC(target);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity *= 0f;
			return false;
		}

		public override void AI()
		{
			TimeSpent++;
			if (!Initialized)
			{
				Initialized = true;
				SoundEngine.PlaySound(SoundID.Item34.WithPitchOffset(Main.rand.NextFloat(0.4f, 0.8f)), Projectile.Center);

				Spores = new List<GlowingMushroomProjectileSpore>();

				for (int i = 0; i < 12; i++)
				{
					Vector2 sporeOffset = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
					Vector2 sporeVelocity = Vector2.Normalize(sporeOffset) * Main.rand.NextFloat(0f, 1f);

					Spores.Add(new GlowingMushroomProjectileSpore(sporeOffset, sporeVelocity));
				}

			}

			if (Projectile.ai[0] != 0 && Projectile.timeLeft > 30)
			{
				Projectile.timeLeft = 30;
			}

			float colorMult = (float)Math.Sin(TimeSpent * 0.1f) * 0.2f + 0.8f; // Spore cloud emits light that gets dimmer as the projectile is about to expire
			if (Projectile.timeLeft < 30) colorMult *= Projectile.timeLeft / 30f;
			Lighting.AddLight(Projectile.Center, 0.443f * colorMult, 0.572f * colorMult, 1f * colorMult);

			if (Main.rand.NextBool(5))
			{
				Spores[Main.rand.Next(Spores.Count)].Kill = true;
				Vector2 sporeOffset = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
				Vector2 sporeVelocity = Vector2.Normalize(sporeOffset) * Main.rand.NextFloat(0f, 1f);

				Spores.Add(new GlowingMushroomProjectileSpore(sporeOffset, sporeVelocity));
			}

			for (int i = Spores.Count - 1; i >= 0; i --)
			{
				GlowingMushroomProjectileSpore spore = Spores[i];

				spore.Update();
				if (spore.Scale < 0f)
				{
					Spores.Remove(spore);
				}
			}

			Projectile.ai[1]++;
			if (Projectile.ai[1] > 10) Projectile.velocity *= 0.8f;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (!Initialized) return false;
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			Texture2D projTexture = TextureAssets.Projectile[Projectile.type].Value;

			float colorMult = 1f;
			if (Projectile.timeLeft < 30) colorMult *= Projectile.timeLeft / 30f;


			Vector2 drawPosition = Projectile.Center - Main.screenPosition;

			foreach (GlowingMushroomProjectileSpore spore in Spores)
			{
				Rectangle drawRectangle = projTexture.Bounds;
				drawRectangle.Height /= 3;
				drawRectangle.Y = spore.Frame * drawRectangle.Height;

				Vector2 sporeDrawPosition = drawPosition + spore.Offset;
				Color colorGlow = Color.White * spore.Glow * colorMult;

				SpriteEffects spriteEffects = spore.Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

				spriteBatch.Draw(projTexture, sporeDrawPosition, drawRectangle, colorGlow, spore.Rotation, drawRectangle.Size() * 0.5f, spore.Scale, spriteEffects, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			return false;
		}

		public class GlowingMushroomProjectileSpore
		{
			public Vector2 Offset;
			public Vector2 Velocity;
			public int Timer;
			public int Frame;
			public Vector2 TargetPosition;
			public float Scale;
			public float ScaleTarget;
			public float Rotation;
			public float RotationAdditive;
			public float Glow;
			public bool Flip;
			public bool Kill;

			public GlowingMushroomProjectileSpore(Vector2 offset_, Vector2 velocity_)
			{
				Kill = false;
				Offset = offset_;
				Velocity = velocity_;
				Timer = Main.rand.Next(30);
				Frame = Main.rand.Next(3);
				TargetPosition = Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(16f);
				Scale = Main.rand.NextFloat(0f, 0.1f);
				ScaleTarget = Main.rand.NextFloat(0.5f, 1f);
				Glow = Main.rand.NextFloat(0.35f, 0.7f);
				Flip = Main.rand.NextBool();
				Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
				RotationAdditive = Main.rand.NextFloat(-0.05f, 0.05f);
			}

			public void Update()
			{
				Timer++;
				if (Timer > 40)
				{
					Timer = Main.rand.Next(30);
					TargetPosition = Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(24f);
				}

				if (Kill)
				{
					Scale -= 0.05f;
				}
				else if (Scale < ScaleTarget)
				{
					Scale += 0.03f;
				}

				Velocity += (TargetPosition - Offset) * 0.01f;
				if (Velocity.Length() > 0.75f)
				{
					Velocity = Vector2.Normalize(Velocity) * 0.5f;
				}

				Offset += Velocity;
				Rotation += RotationAdditive;
			}
		}
	}
}