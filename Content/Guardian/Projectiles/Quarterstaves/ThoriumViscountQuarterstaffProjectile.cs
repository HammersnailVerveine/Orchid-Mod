using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian.Weapons.Quarterstaves;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Quarterstaves
{
	public class ThoriumViscountQuarterstaffProjectile : OrchidModGuardianProjectile
	{
		private static Texture2D TextureGlow;
		public List<ViscountQuarterstaffBat> Bats;

		public override void SafeSetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Bats = new List<ViscountQuarterstaffBat>();
			TextureGlow ??= ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void AI()
		{
			Projectile.Center = Owner.Center;
			Projectile.velocity = Owner.velocity;

			if (!Initialized)
			{
				Initialized = true;

				SoundStyle soundStyle = SoundID.NPCDeath4;
				soundStyle.Pitch = Main.rand.NextFloat(0.4f, 0.8f);
				SoundEngine.PlaySound(soundStyle, Projectile.Center);

				Bats = new List<ViscountQuarterstaffBat>();

				for (int i = 0; i < 10; i ++)
				{
					Vector2 batOffset = new Vector2(Main.rand.NextFloat(-32f, 32f), Main.rand.NextFloat(-32f, 32f));
					Vector2 batVelocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
					int batTimer = Main.rand.Next(6);

					Bats.Add(new ViscountQuarterstaffBat(batOffset, batVelocity, batTimer));
				}

				for (int i = 0; i < 5; i++)
				{
					Gore gore = Gore.NewGoreDirect(Owner.GetSource_FromAI(), Owner.Center + new Vector2(Main.rand.NextFloat(-24f, 0f), Main.rand.NextFloat(-24f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 61 + Main.rand.Next(3));
					gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
				}

				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(Owner.Center, 0, 0, DustID.Smoke);
					dust.scale *= Main.rand.NextFloat(1f, 1.5f);
					dust.velocity *= Main.rand.NextFloat(0.5f, 0.75f);
				}
			}

			if (Projectile.timeLeft % 10 == 0 && Main.rand.NextBool())
			{ // random bat sounds
				SoundStyle soundStyle = SoundID.NPCDeath4;
				soundStyle.Pitch = Main.rand.NextFloat(1.4f, 1.8f);
				soundStyle.Volume *= 0.7f;
				SoundEngine.PlaySound(soundStyle, Projectile.Center);
			} 

			foreach (ViscountQuarterstaffBat bat in Bats)
			{
				bat.Update();
			}

			if (Owner.HeldItem.ModItem != null && Owner.HeldItem.ModItem is ThoriumViscountQuarterstaff quarterstaff && quarterstaff.IsCounterAttacking > 0)
			{
				return;
			}
			else if (IsLocalOwner)
			{ // Kill projectile if owner is not counterattacking with the viscount quarterstaff
				Projectile.Kill();
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (!Initialized) return false;
			Texture2D projTexture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			drawPosition.Y -= Owner.gfxOffY;

			foreach (ViscountQuarterstaffBat bat in Bats)
			{
				Rectangle drawRectangle = projTexture.Bounds;
				drawRectangle.Height /= 4;
				drawRectangle.Y = bat.Frame * drawRectangle.Height;

				Vector2 batDrawPosition = drawPosition + bat.Offset;

				SpriteEffects spriteEffects = SpriteEffects.None;
				if (bat.Velocity.X > 0)
				{
					spriteEffects = SpriteEffects.FlipHorizontally;
				}

				spriteBatch.Draw(projTexture, batDrawPosition, drawRectangle, lightColor, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, spriteEffects, 0f);
				spriteBatch.Draw(TextureGlow, batDrawPosition, drawRectangle, Color.White, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, spriteEffects, 0f);
			}

			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Gore gore = Gore.NewGoreDirect(Owner.GetSource_FromAI(), Owner.Center + new Vector2(Main.rand.NextFloat(-24f, 0f), Main.rand.NextFloat(-24f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 61 + Main.rand.Next(3));
				gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
			}

			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(Owner.Center, 0, 0, DustID.Smoke);
				dust.scale *= Main.rand.NextFloat(1f, 1.5f);
				dust.velocity *= Main.rand.NextFloat(0.5f, 0.75f);
			}
		}
	}

	public class ViscountQuarterstaffBat
	{
		public Vector2 Offset;
		public Vector2 Velocity;
		public int Timer;
		public int Frame;
		public Vector2 TargetPosition;

		public ViscountQuarterstaffBat(Vector2 offset_, Vector2 velocity_, int timer_)
		{
			Offset = offset_;
			Velocity = velocity_;
			Timer = timer_;
			Frame = Main.rand.Next(4);
			TargetPosition = Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(16f);
		}

		public void Update()
		{
			Timer++;
			if (Timer > 5)
			{
				Timer = 0;
				Frame++;
				if (Frame > 3)
				{
					Frame = 0;
					TargetPosition = Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(24f);
				}
			}

			Velocity += (TargetPosition - Offset) * 0.01f;
			if (Velocity.Length() > 5f)
			{
				Velocity = Vector2.Normalize(Velocity) * 5f;
			}

			Offset += Velocity;
		}
	}
}