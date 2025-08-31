using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Projectiles.Quarterstaves
{
	public class SpectreQuarterstaffProj : OrchidModGuardianProjectile
	{
		float timerNoReach = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.scale = 0.66f;
			Projectile.penetrate = 5;
			Projectile.alpha = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (!Initialized)
			{
				Initialized = true;
				Projectile.frame = Main.rand.Next(4);
				Projectile.localAI[0] = Main.rand.Next(2);
				Projectile.localAI[1] = Main.rand.NextFloat(- 48f, 48f);
				Projectile.localAI[2] = Main.rand.NextFloat(-48f, 48f);
				Projectile.ai[0] += Main.rand.NextFloat(1f, 2f);
			}

			if (Projectile.localAI[0] < 1f)
			{
				Projectile.localAI[0] += 0.05f;
			}

			if (Projectile.timeLeft % 6 == 0)
			{
				Projectile.frame++;

				if (Projectile.frame > 3)
				{
					Projectile.frame = 0;
				}
			}

			if (Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position + new Vector2((float)Math.Sin(Projectile.timeLeft * 0.05f) * 3f - 2 * Projectile.localAI[0], 0f), Projectile.width, Projectile.height, DustID.DungeonSpirit);
				dust.noGravity = true;
				dust.velocity.X *= 0;
				dust.velocity.Y = Main.rand.NextFloat(-3.5f, -0.5f);
				dust.scale *= Main.rand.NextFloat(0.5f, 0.8f);
			}

			Vector2 target = Owner.Center - Vector2.UnitY * 128f + new Vector2(Projectile.localAI[1], Projectile.localAI[2]) + Owner.velocity * 15f;

			if (Projectile.Center.Distance(target) > 88f && timerNoReach >= 0)
			{
				timerNoReach++;
				Projectile.velocity += (target - Projectile.Center) * (0.005f + timerNoReach * 0.00025f);

				if (Projectile.velocity.Length() > 10f)
				{
					Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 16f;
				}
			}
			else
			{
				if (timerNoReach > 0 || timerNoReach < 0)
				{
					timerNoReach++;
					if (timerNoReach > 0)
					{
						timerNoReach = -Main.rand.Next(10, 20);
					}
				}
				else
				{
					timerNoReach = 0;
				}

				Projectile.velocity *= 0.9f;

				if (Projectile.velocity.Length() < Projectile.ai[0])
				{
					Projectile.velocity = Vector2.Normalize(Projectile.velocity) * Projectile.ai[0];
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			Texture2D projTexture = TextureAssets.Projectile[Projectile.type].Value;
			float colorMult = Projectile.localAI[0];
			if (Projectile.timeLeft < 20f) colorMult *= Projectile.timeLeft / 20f;

			float offsetX = (float)Math.Sin(Projectile.timeLeft * 0.05f) * 3f;

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			drawPosition.X += offsetX;

			Rectangle drawRectangle = projTexture.Bounds;
			drawRectangle.Height /= 4;
			drawRectangle.Y = Projectile.frame * drawRectangle.Height;

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.localAI[0] == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}

			spriteBatch.Draw(projTexture, drawPosition, drawRectangle, Color.White * colorMult, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, spriteEffects, 0f);
			return false;
		}
	}
}