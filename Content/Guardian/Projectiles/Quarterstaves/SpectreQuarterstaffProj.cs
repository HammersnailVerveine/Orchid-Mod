using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
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
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
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
				Projectile.ai[2] = -1f;

				SoundStyle soundStyle = Main.rand.NextBool(3) ? SoundID.Zombie83 : Main.rand.NextBool() ? SoundID.Zombie81 : SoundID.Zombie82;
				soundStyle.Pitch = Main.rand.NextFloat(1.4f, 1.8f);
				soundStyle.Volume *= 0.1f;
				SoundEngine.PlaySound(soundStyle, Projectile.Center);
			}

			if (Projectile.localAI[0] < 0.85f)
			{
				Projectile.localAI[0] += 0.04f;
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

			if (Projectile.ai[2] < 0f)
			{ // Hovering above the player
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

				if (Projectile.ai[1] > 0)
				{
					Projectile.ai[1]--;
					if (Projectile.ai[1] <= 0)
					{
						NPC closestTarget = null;
						float distanceClosest = 1600f;
						foreach (NPC npc in Main.npc)
						{
							float distance = Projectile.Center.Distance(npc.Center);
							if (IsValidTarget(npc) && distance < distanceClosest)
							{
								closestTarget = npc;
								distanceClosest = distance;
							}
						}

						if (closestTarget != null)
						{
							Projectile.ai[2] = closestTarget.whoAmI;
							Vector2 position = Owner.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(32f, 160f);
							Projectile.ai[0] = position.X;
							Projectile.ai[1] = position.Y;
							Projectile.velocity *= 0f;
							Projectile.netUpdate = true;
						}
						else
						{
							Projectile.Kill();
						}
					}
				}
			}
			else
			{ // homing
				Projectile.friendly = true;
				if (Projectile.ai[1] != 0f)
				{
					timerNoReach = 0;

					if (Projectile.ai[0] != 0f)
					{
						for (int i = 0; i < 8; i++)
						{
							Dust dust = Dust.NewDustDirect(Projectile.position + new Vector2((float)Math.Sin(Projectile.timeLeft * 0.05f) * 3f - 2 * Projectile.localAI[0], 0f), Projectile.width, Projectile.height, DustID.DungeonSpirit);
							dust.noGravity = true;
							dust.scale *= Main.rand.NextFloat(0.8f, 1.2f);
							dust.velocity *= Main.rand.NextFloat(1f, 5f);
						}

						Projectile.Center = new Vector2(Projectile.ai[0], Projectile.ai[1]);
						Projectile.ai[0] = 0f;
						Projectile.timeLeft = 180;

						SoundStyle soundStyle = SoundID.NPCDeath52;
						soundStyle.Pitch = Main.rand.NextFloat(1.5f, 2f);
						soundStyle.Volume *= 0.75f;
						SoundEngine.PlaySound(soundStyle, Projectile.Center);

						for (int i = 0; i < 5; i++)
						{
							Dust dust = Dust.NewDustDirect(Projectile.position + new Vector2((float)Math.Sin(Projectile.timeLeft * 0.05f) * 3f - 2 * Projectile.localAI[0], 0f), Projectile.width, Projectile.height, DustID.DungeonSpirit);
							dust.noGravity = true;
							dust.scale *= Main.rand.NextFloat(0.8f, 1.2f);
							dust.velocity *= Main.rand.NextFloat(1f, 5f);
						}
					}

					Projectile.ai[1] = 0f;
				}

				if (Main.rand.NextBool(2))
				{
					Dust dust = Dust.NewDustDirect(Projectile.position + new Vector2((float)Math.Sin(Projectile.timeLeft * 0.05f) * 3f - 2 * Projectile.localAI[0], 0f), Projectile.width, Projectile.height, DustID.DungeonSpirit);
					dust.noGravity = true;
					dust.velocity *= 0.2f;
					dust.scale *= Main.rand.NextFloat(0.8f, 1.2f);
				}

				NPC target = Main.npc[(int)Projectile.ai[2]];
				timerNoReach++;

				if (IsValidTarget(target))
				{
					Projectile.velocity += (target.Center - Projectile.Center) * (0.003f + timerNoReach * 0.0001f);
					Projectile.velocity = Vector2.Normalize(Projectile.velocity) * (3f + timerNoReach * 0.15f);
				}
				else
				{
					NPC closestTarget = null;
					float distanceClosest = 1600f;
					foreach (NPC npc in Main.npc)
					{
						float distance = Projectile.Center.Distance(npc.Center);
						if (IsValidTarget(npc) && distance < distanceClosest)
						{
							closestTarget = npc;
							distanceClosest = distance;
						}
					}

					if (closestTarget != null)
					{
						Projectile.ai[2] = closestTarget.whoAmI;
						Projectile.ai[1] = 1f; // to sync reset timerNoReach 
					}
					else
					{
						Projectile.Kill();
					}
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			float sineOffset = (float)Math.Sin(Projectile.timeLeft * 0.05f);
			Texture2D projTexture = TextureAssets.Projectile[Projectile.type].Value;
			float colorMult = Projectile.localAI[0] + 0.1f * sineOffset;
			if (Projectile.timeLeft < 20f) colorMult *= Projectile.timeLeft / 20f;

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			drawPosition.X += sineOffset * 3f;

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